using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Sinch.Tests.e2e.Auth
{
    public class AuthTests : TestBase
    {
        [Fact]
        public async Task GetTokenValue()
        {
            var token = await SinchClientMockServer.Auth.GetAuthToken();
            token.Length.Should().BeGreaterThan(10);
        }
    }
}
