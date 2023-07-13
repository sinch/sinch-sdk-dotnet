using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using Sinch.Auth;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class HttpTests
    {
        private readonly Mock<IAuth> _tokenManagerMock;
        private readonly MockHttpMessageHandler _httpMessageHandlerMock;

        public HttpTests()
        {
            _tokenManagerMock = new Mock<IAuth>();
            _httpMessageHandlerMock = new MockHttpMessageHandler();
        }

        [Fact]
        public async Task ForceNewToken()
        {
            _tokenManagerMock
                .Setup(x => x.GetToken(It.Is<bool>(s => !s)))
                .ReturnsAsync("first_token");
            _tokenManagerMock
                .Setup(x => x.GetToken(It.Is<bool>(s => s)))
                .ReturnsAsync("second_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock.Object, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<object>> response = () => http.Send<object>(uri, HttpMethod.Get);

            await response.Should().NotThrowAsync();
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task ForceNewTokenOnlyOnce()
        {
            _tokenManagerMock.SetupSequence(x => x.GetToken(It.IsAny<bool>()))
                .ReturnsAsync("first_token")
                .ReturnsAsync("second_token")
                .ReturnsAsync("third_token");

            var uri = new Uri("http://sinch.com/items");
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer first_token")
                .Respond(HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.Expect(HttpMethod.Get, uri.ToString())
                .WithHeaders("Authorization", "Bearer second_token")
                .Respond(HttpStatusCode.Unauthorized);
            var httpClient = new HttpClient(_httpMessageHandlerMock);
            var http = new Http(_tokenManagerMock.Object, httpClient, null, new SnakeCaseNamingPolicy());

            Func<Task<object>> response = () => http.Send<object>(uri, HttpMethod.Get);

            var ex = await response.Should().ThrowAsync<ApiException>();
            ex.Where(x => x.StatusCode == HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }
    }
}
