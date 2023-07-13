using System.Net.Http;
using System.Text.Json;
using Moq;
using RichardSzalay.MockHttp;
using Sinch.Auth;
using Sinch.Core;

namespace Sinch.Tests
{
    public class TestBase
    {
        protected const string ProjectId = "MOCK_PROJECT_ID";
        protected const string Token = "to-ke-n";
        private readonly Mock<IAuth> _tokenManager = new();
        internal readonly Http HttpCamelCase;
        protected readonly MockHttpMessageHandler HttpMessageHandlerMock = new();
        internal readonly Http HttpSnakeCase;

        protected TestBase()
        {
            var httpClient = new HttpClient(HttpMessageHandlerMock);
            _tokenManager.Setup(x => x.GetToken(false)).ReturnsAsync(Token);
            HttpCamelCase = new Http(_tokenManager.Object, httpClient, null, JsonNamingPolicy.CamelCase);
            HttpSnakeCase = new Http(_tokenManager.Object, httpClient, null, SnakeCaseNamingPolicy.Instance);
        }
    }
}
