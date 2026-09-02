using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Fax.Faxes;
using Xunit;

namespace Sinch.Tests.Core
{
    public class HttpTests
    {
        private readonly ISinchAuth _tokenManagerMock;
        private readonly MockHttpMessageHandler _httpMessageHandlerMock;

        private KeyValuePair<string, string>[] _expiredHeader = new KeyValuePair<string, string>[]
        {
            new("www-authenticate",
                "Bearer error=\"invalid_token\", error_description=\"Jwt expired at 2024-07-08T22:12:28Z\", error_uri=\"https://tools.ietf.org/html/rfc6750#section-3.1\"")
        };

        public HttpTests()
        {
            _tokenManagerMock = Substitute.For<ISinchAuth>();
            _tokenManagerMock.Scheme.Returns("Bearer");
            _httpMessageHandlerMock = new MockHttpMessageHandler();
        }

        private sealed class TrackingHttpContent : HttpContent
        {
            private readonly byte[] _bytes;

            public bool IsDisposed { get; private set; }

            public TrackingHttpContent(string text)
            {
                _bytes = System.Text.Encoding.UTF8.GetBytes(text);
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return stream.WriteAsync(_bytes, 0, _bytes.Length);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = _bytes.Length;
                return true;
            }

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }

        private sealed class TrackingStream : MemoryStream
        {
            public bool IsDisposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }

        [Fact]
        public async Task ForceNewToken()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var response = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            await response.Should().NotThrowAsync();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task ForceNewTokenOnlyOnce()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token", "second_token", "third_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.Unauthorized);
            var httpClient = new HttpClient(_httpMessageHandlerMock);

            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());
            Func<Task<object>> response = () => http.Send<object>(uri, HttpMethod.Get);

            var ex = await response.Should().ThrowAsync<SinchApiException>();
            ex.Where(x => x.StatusCode == HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task OauthThrowExceptionIfTokenNotExpired()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized, new KeyValuePair<string, string>[]
                {
                    new("www-authenticate", "no")
                }, (HttpContent)null);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<object>> response = () => http.Send<object>(uri, HttpMethod.Get);

            var ex = await response.Should().ThrowAsync<SinchApiException>();
            ex.Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task NewTokenFetchedIfWwwExpiredIsPresent()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var response = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            await response.Should().NotThrowAsync();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SinchRequestShouldContainAuthorizationHeader()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, SnakeCaseNamingPolicy.Instance);

            await http.Send<EmptyResponse>(uri, HttpMethod.Get);

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SinchRequestShouldContainUserAgentHeader()
        {

            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .With(request => request.Headers.UserAgent.ToString() == Http.UserAgent)
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, SnakeCaseNamingPolicy.Instance);

            await http.Send<EmptyResponse>(uri, HttpMethod.Get);

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public void UserAgentShouldHaveCorrectFormat()
        {
            var userAgent = Http.UserAgent;

            userAgent.Should().MatchRegex(@"^sinch-sdk/.* \(csharp/.*; .*;\)$");
        }

        [Fact]
        public async Task AddOwnHeaders()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .WithHeaderExact("Accept-Language", new[]
                {
                    "en-US",
                    "uk-UA"
                })
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            await http.Send<EmptyResponse>(uri, HttpMethod.Get, headers: new Dictionary<string, IEnumerable<string>>()
            {
                { "Accept-Language", new[] { "en-US", "uk-UA" } }
            });

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SendMultipartFormData()
        {
            var uri = new Uri("http://hello.fax");
            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithPartialContent("To\r\n\r\n123,456")
                .WithPartialContent("MaxRetries\r\n\r\n3")
                .WithPartialContent("\"Labels[hello]\"\r\n\r\nworld")
                .WithPartialContent("\"Labels[no]\"\r\n\r\nidea")
                .WithPartialContent("HeaderPageNumbers\r\n\r\nTrue")
                .Respond(HttpStatusCode.OK);
            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());
            var faxRequest = new SendFaxRequest(new MemoryStream(), "file.pdf")
            {
                MaxRetries = 3,
                Labels = new Dictionary<string, string>()
                {
                    { "hello", "world" },
                    { "no", "idea" }
                },
                HeaderPageNumbers = true,
            };
            faxRequest.SetTo(new List<string>() { "123", "456" });

            await http.SendMultipart<SendFaxRequest, EmptyResponse>(uri, faxRequest,
                faxRequest.FileContent!, faxRequest.FileName!);

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        // Reproduces https://github.com/sinch/sinch-sdk-dotnet/issues/214 for multipart requests:
        // the file stream is fully consumed (positioned at EOF) after the first send, so if the
        // retried request reuses the same StreamContent instance, its body would wrongly come back empty.
        [Fact]
        public async Task SendMultipartFormData_RetryAfterExpiredToken_ResendsFileContent()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var uri = new Uri("http://hello.fax");
            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .WithPartialContent("some-pdf-bytes")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .WithPartialContent("some-pdf-bytes")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());
            var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("some-pdf-bytes"));
            var faxRequest = new SendFaxRequest(fileStream, "file.pdf");
            faxRequest.SetTo(new List<string>() { "123" });

            Func<Task<EmptyResponse>> response = () => http.SendMultipart<SendFaxRequest, EmptyResponse>(
                uri, faxRequest, faxRequest.FileContent!, faxRequest.FileName!);

            await response.Should().NotThrowAsync();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task UnauthorizedAndNoSecondAuthCallIfExpiredHeaderIsNotPresent()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            // first token expires
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());
            Func<Task<EmptyResponse>> op1 = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            await op1.Should().ThrowAsync<SinchApiException>();
            // only one function call, to read the token stored in cache, is performed
            _tokenManagerMock.Received(requiredNumberOfCalls: 1);
        }

        // This test is testing a positive scenario when server was holding previously fetched token for a while,
        // and it became expired.
        // Also tests next request, simulating the hold of token for some time again, 
        // making sure the scenario have the same behaviour between two *independent* requests.
        // 
        // send expired token -> sinch api
        // 401 with expired header <- respond  
        // request new token -> sinch auth
        // save new token <- sinch auth returns new token
        // use new token -> sinch api success
        // idle some time, latest token become expired
        // repeat the above
        [Fact]
        public async Task NewTokenIsFetchedBetweenTwoRequestsStartingFromExpired()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token", "second_token", "second_token", "third_token");

            var uri = new Uri("http://sinch.com/items");

            // first token expires, simulating state when server had token beforehand for some time already
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            // internally auth fetches a new valid token, and request to same endpoint now good
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.OK);

            // simulating the hold of token for some time here, the latest token is expired again for second request
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            // and should be the same scenario, internally auth fetched new token which is used in this request
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer third_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<EmptyResponse>> op1 = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);
            Func<Task<EmptyResponse>> op2 = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            // first call sees that token it's holding is expired, and fetches new token
            await op1.Should().NotThrowAsync();
            // now this call see a latest token is expired and should re fetched
            await op2.Should().NotThrowAsync();

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        // Reproduces https://github.com/sinch/sinch-sdk-dotnet/issues/214:
        // the HttpContent instance is reused across retry iterations in SendHttpContent, but
        // was "disposed" before retrying request: So the retried request was throwing
        // ObjectDisposedException
        [Fact]
        public async Task RetryAfterExpiredTokenWithBody_RecreatesContentAndSucceeds()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .WithContent(JsonSerializer.Serialize("body"))
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .WithContent(JsonSerializer.Serialize("body"))
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<EmptyResponse>> response = () => http.Send<string, EmptyResponse>(uri, HttpMethod.Post, "body");

            await response.Should().NotThrowAsync();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task RetryAfterExpiredToken_DisposesRetryingResponse()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var firstResponseContent = new TrackingHttpContent("expired");
            var uri = new Uri("http://sinch.com/items");

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = firstResponseContent,
                    Headers =
                    {
                        { "www-authenticate", "Bearer error=\"invalid_token\", error_description=\"Jwt expired\"" }
                    }
                });

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            await http.Send<string, EmptyResponse>(uri, HttpMethod.Post, "body");

            firstResponseContent.IsDisposed.Should().BeTrue();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task UnauthorizedNoRetry_DisposesResponseEvenWhenThrowing()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var failureContent = new TrackingHttpContent("{}");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = failureContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<EmptyResponse>> response = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            await response.Should().ThrowAsync<SinchApiException>();
            failureContent.IsDisposed.Should().BeTrue();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SendMultipartFormData_DoesNotDisposeCallerOwnedStream()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://hello.fax");
            var trackingStream = new TrackingStream();
            var faxRequest = new SendFaxRequest(trackingStream, "file.pdf");
            faxRequest.SetTo(new List<string>() { "123" });

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            await http.SendMultipart<SendFaxRequest, EmptyResponse>(uri, faxRequest, faxRequest.FileContent!, faxRequest.FileName!);

            trackingStream.IsDisposed.Should().BeFalse();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task PdfResponse_StreamsContentAndDisposesOnResult()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var trackingContent = new TrackingHttpContent("pdf-bytes");
            trackingContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            trackingContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "report.pdf"
            };

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = trackingContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var contentResult = await http.Send<ContentResult>(uri, HttpMethod.Get);

            contentResult.FileName.Should().Be("report.pdf");

            using var reader = new StreamReader(contentResult.Stream, System.Text.Encoding.UTF8, leaveOpen: true);
            (await reader.ReadToEndAsync()).Should().Be("pdf-bytes");

            trackingContent.IsDisposed.Should().BeFalse();

            contentResult.Dispose();

            trackingContent.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task PdfResponse_DisposeAsync_DisposesUnderlyingResponse()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var trackingContent = new TrackingHttpContent("pdf-bytes");
            trackingContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            trackingContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "report.pdf"
            };

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = trackingContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var contentResult = await http.Send<ContentResult>(uri, HttpMethod.Get);

            trackingContent.IsDisposed.Should().BeFalse();

            await contentResult.DisposeAsync();

            trackingContent.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task UnexpectedContentType_ThrowsAndDisposesResponse()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var textContent = new TrackingHttpContent("plain text, not json or pdf");
            textContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = textContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<string>> response = () => http.Send<string>(uri, HttpMethod.Get);

            await response.Should().ThrowAsync<InvalidOperationException>();
            textContent.IsDisposed.Should().BeTrue();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task JsonResponse_DisposesHttpContent()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var jsonContent = new TrackingHttpContent(JsonSerializer.Serialize("json-value"));
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = jsonContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var response = await http.Send<string>(uri, HttpMethod.Get);

            response.Should().Be("json-value");
            jsonContent.IsDisposed.Should().BeTrue();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task EmptyResponse_DisposesHttpContent()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            var emptyContent = new TrackingHttpContent("{}");
            emptyContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = emptyContent
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            var response = await http.Send<EmptyResponse>(uri, HttpMethod.Get);

            response.Should().NotBeNull();
            emptyContent.IsDisposed.Should().BeTrue();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SendReceiveUnicode()
        {
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            // first token expires
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .WithContent(JsonSerializer.Serialize("😼"))
                .Respond(HttpStatusCode.OK, JsonContent.Create("😼"));

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());
            var op1 = await http.Send<string, string>(uri, HttpMethod.Get, "😼");

            op1.Should().BeEquivalentTo("😼");
        }
    }
}
