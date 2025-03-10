using System;
using System.Collections.Generic;
using System.Linq;
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

        public record VerificationCredentialsMissingTestCaseData(
            string TestName,
            SinchVerificationConfiguration VerificationConfiguration,
            string ParamName,
            string Message)
        {
            private static readonly VerificationCredentialsMissingTestCaseData[] TestCases =
            {
                new("AppKey is null", new SinchVerificationConfiguration()
                    {
                        AppKey = null!,
                        AppSecret = "some"
                    },
                    $"{nameof(SinchVerificationConfiguration.AppKey)}",
                    "The value should be present (Parameter 'AppKey')"),
                new("AppKey is empty", new SinchVerificationConfiguration()
                    {
                        AppKey = string.Empty,
                        AppSecret = "some"
                    },
                    $"{nameof(SinchVerificationConfiguration.AppKey)}",
                    "The value should be present (Parameter 'AppKey')"),
                new("AppSecret is null", new SinchVerificationConfiguration()
                    {
                        AppKey = "some",
                        AppSecret = null!
                    },
                    $"{nameof(SinchVerificationConfiguration.AppSecret)}",
                    "The value should be present (Parameter 'AppSecret')"),
                new("AppSecret is empty", new SinchVerificationConfiguration()
                    {
                        AppKey = "aaa",
                        AppSecret = string.Empty
                    },
                    $"{nameof(SinchVerificationConfiguration.AppSecret)}",
                    "The value should be present (Parameter 'AppSecret')"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(VerificationCredentialsMissingTestCaseData.TestCasesData),
            MemberType = typeof(VerificationCredentialsMissingTestCaseData))]
        public void ThrowIfVerificationCredentialsAreMissing(VerificationCredentialsMissingTestCaseData data)
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = data.VerificationConfiguration
            });
            var op = () => client.Verification;
            var which = op.Should().ThrowExactly<ArgumentNullException>().Which;
            which.ParamName.Should().Be(data.ParamName);
            which.Message.Should().Be(data.Message);
        }


        [Fact]
        public void ThrowIfVerificationConfigIsNull()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = null
            });
            var op = () => client.Verification;
            op.Should().ThrowExactly<InvalidOperationException>().Which.Message.Should()
                .Be("SinchVerificationConfiguration is not set.");
        }
    }
}
