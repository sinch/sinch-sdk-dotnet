using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
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
        public async Task SendSinchHeader()
        {
            _tokenManagerMock
                .GetAuthToken(Arg.Any<bool>())
                .Returns("first_token");

            var uri = new Uri("http://sinch.com/items");

            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo().Assembly.FullName!).Version!.ToString();

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
            // only one call, returning cached token, is performed
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
