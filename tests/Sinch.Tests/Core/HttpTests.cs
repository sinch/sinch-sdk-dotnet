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

        private Lazy<ISinchAuth> GetMock => new Lazy<ISinchAuth>(_tokenManagerMock);

        public HttpTests()
        {
            _tokenManagerMock = Substitute.For<ISinchAuth>();
            _tokenManagerMock.Scheme.Returns("Bearer");
            _httpMessageHandlerMock = new MockHttpMessageHandler();
        }

        private Func<HttpClient> CreateHttpClientAccessor(HttpClient httpClient) => () => httpClient;

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

            public TrackingStream(byte[] bytes) : base(bytes)
            {
            }

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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

            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);
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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

            Func<Task<object>> response = () => http.Send<object>(uri, HttpMethod.Get);

            var ex = await response.Should().ThrowAsync<SinchApiException>();
            ex.Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        // The retry-on-401 logic is meant only for a Bearer (OAuth) token the server reports as
        // expired. A non-Bearer scheme must never retry, even if the response happens to carry a
        // www-authenticate header that contains "expired" - retrying there would duplicate a
        // request against an endpoint that isn't using stale-token semantics at all.
        [Fact]
        public async Task NonBearerScheme_DoesNotRetryOnUnauthorizedEvenWithExpiredHeader()
        {
            _tokenManagerMock.Scheme.Returns("Basic");
            _tokenManagerMock.GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Basic first_token")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

            await http.Send<EmptyResponse>(uri, HttpMethod.Get, headers: new Dictionary<string, IEnumerable<string>>()
            {
                { "Accept-Language", new[] { "en-US", "uk-UA" } }
            });

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SendWithStream_BuildsMultipartContentAndSendsPostRequest()
        {
            var uri = new Uri("http://hello.fax");
            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .Respond(HttpStatusCode.OK);
            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

            HttpContent BuildContent(Stream fileStream)
            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent("123"), "to");
                multipartContent.Add(new StringContent("456"), "to");
                multipartContent.Add(new StreamContent(fileStream), "file", "file.pdf");
                return multipartContent;
            }

            await http.Send<EmptyResponse>(uri, HttpMethod.Post, new MemoryStream(), BuildContent);

            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        // Covers the case where there's no caller-owned stream at all: buildContent must still be 
        // called, with null, and Core must not try to rewind/wrap a stream that doesn't exist.
        [Fact]
        public async Task SendWithStream_NullStream_CallsBuildContentWithNullAndSendsPostRequest()
        {
            var uri = new Uri("http://hello.fax");
            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithPartialContent("no-file-here")
                .Respond(HttpStatusCode.OK);
            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

            Stream receivedStream = new MemoryStream(); // sentinel to prove it gets overwritten with null

            HttpContent BuildContent(Stream fileStream)
            {
                receivedStream = fileStream;
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent("no-file-here"), "contentUrl");
                return multipartContent;
            }

            await http.Send<EmptyResponse>(uri, HttpMethod.Post, null, BuildContent);

            receivedStream.Should().BeNull();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        // Reproduces https://github.com/sinch/sinch-sdk-dotnet/issues/214, generically (not tied
        // to any specific domain/request type): a retry must rebuild content rather than reuse
        // the disposed content from the first attempt, and the caller-owned stream handed to
        // buildContent must survive being wrapped in StreamContent that gets disposed after each
        // attempt - the caller never touches that disposal mechanic, Core owns it.
        [Fact]
        public async Task Send_WithStream_RetryAfterExpiredToken_RebuildsAndResendsStreamContent()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Is<bool>(x => !x))
                .Returns("first_token");
            _tokenManagerMock
                .GetAuthToken(true)
                .Returns("second_token");

            var uri = new Uri("http://hello.generic");
            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .WithPartialContent("some-stream-bytes")
                .Respond(HttpStatusCode.Unauthorized, _expiredHeader, (HttpContent)null);

            _httpMessageHandlerMock.Expect(HttpMethod.Post, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .WithPartialContent("some-stream-bytes")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);
            var callerOwnedStream = new TrackingStream(System.Text.Encoding.UTF8.GetBytes("some-stream-bytes"));

            // The caller only knows how to build content from a stream - it doesn't rewind it,
            // doesn't wrap it against disposal, and isn't aware a retry can even happen.
            HttpContent BuildContent(Stream fileStream) => new StreamContent(fileStream);

            Func<Task<EmptyResponse>> response = () =>
                http.Send<EmptyResponse>(uri, HttpMethod.Post, callerOwnedStream, BuildContent);

            await response.Should().NotThrowAsync();
            callerOwnedStream.IsDisposed.Should().BeFalse();
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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);
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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

            Func<Task<EmptyResponse>> response = () => http.Send<EmptyResponse>(uri, HttpMethod.Get);

            await response.Should().ThrowAsync<SinchApiException>();
            failureContent.IsDisposed.Should().BeTrue();
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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);

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
            var http = new Http(GetMock, CreateHttpClientAccessor(httpClient), null, SnakeCaseNamingPolicy.Instance);
            var op1 = await http.Send<string, string>(uri, HttpMethod.Get, "😼");

            op1.Should().BeEquivalentTo("😼");
        }
    }
}
