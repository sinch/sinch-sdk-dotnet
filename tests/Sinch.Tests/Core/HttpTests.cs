using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
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

        public HttpTests()
        {
            _tokenManagerMock = Substitute.For<ISinchAuth>();
            _tokenManagerMock.Scheme.Returns("Bearer");
            _httpMessageHandlerMock = new MockHttpMessageHandler();
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
                .Respond(HttpStatusCode.Unauthorized);
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
                .Respond(HttpStatusCode.Unauthorized);
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
                .Respond(HttpStatusCode.Unauthorized, new KeyValuePair<string, string>[]
                {
                    new("www-authenticate", "expired")
                }, (HttpContent)null);

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
        public async Task SendSinchHeader()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo()!.Assembly!.FullName!).Version!.ToString();

            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                // net framework splits header value at whitespace and returns list of values
                // so we check for exact sequence of header value
                .WithHeaderExact("User-Agent", new[]
                {
                    $"sinch-sdk/{sdkVersion}",
                    $"(csharp/{RuntimeInformation.FrameworkDescription};;)"
                })
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock, httpClient, null, new SnakeCaseNamingPolicy());

            await http.Send<EmptyResponse>(uri, HttpMethod.Get);

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
    }
}
