using System;
using System.Net.Http;
using System.Text.Json;
using NSubstitute;
using RichardSzalay.MockHttp;
using Sinch.Auth;
using Sinch.Core;

namespace Sinch.Tests
{
    public class TestBase
    {
        protected const string ProjectId = "MOCK_PROJECT_ID";
        protected const string Token = "to-ke-n";
        private readonly ISinchAuth _tokenManager = Substitute.For<ISinchAuth>();
        internal readonly Http HttpCamelCase;
        protected readonly MockHttpMessageHandler HttpMessageHandlerMock = new();
        internal readonly Http HttpSnakeCase;

        protected TestBase()
        {
            var httpClient = new HttpClient(HttpMessageHandlerMock);
            _tokenManager.GetAuthToken().Returns(Token);
            _tokenManager.Scheme.Returns("Bearer");
            HttpCamelCase = new Http(new Lazy<ISinchAuth>(_tokenManager), httpClient, null, JsonNamingPolicy.CamelCase);
            HttpSnakeCase = new Http(new Lazy<ISinchAuth>(_tokenManager), httpClient, null, SnakeCaseNamingPolicy.Instance);
        }
    }
}
