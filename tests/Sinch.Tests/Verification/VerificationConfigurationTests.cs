using FluentAssertions;
using Sinch.Verification;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationConfigurationTests
    {
        [Theory]
        [InlineData(null, "https://verification.api.sinch.com/")]
        [InlineData("https://hello.world", "https://hello.world/")]
        public void ResolveUrl(string urlOverride, string expectedUrl)
        {
            var config = new SinchVerificationConfiguration()
            {
                AppKey = "key",
                AppSecret = "secret",
                UrlOverride = urlOverride,
            };
            config.ResolveUrl().ToString().Should().Be(expectedUrl);
        }
    }
}
