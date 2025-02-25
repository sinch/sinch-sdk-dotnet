using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Auth;
using Xunit;

namespace Sinch.Tests
{
    public class AuthConfigurationTests
    {
        public record AuthUrlTestCase(
            string TestName,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly AuthUrlTestCase[] TestCases =
            {
                new("Default Auth URL", null, "https://auth.sinch.com"),
                new("Custom override", "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(AuthUrlTestCase.TestCasesData), MemberType = typeof(AuthUrlTestCase))]
        public void ResolveAuthUrl(AuthUrlTestCase testCase)
        {
            var authConfig = new SinchOAuthConfiguration()
            {
                UrlOverride = testCase.UrlOverride
            };
            authConfig.ResolveUrl().ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }
    }
}
