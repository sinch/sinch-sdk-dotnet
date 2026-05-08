using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationConfigurationTests
    {
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

        [Fact]
        public void InitVerificationSuccess()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = new SinchVerificationConfiguration()
                {
                    AppKey = "key",
                    AppSecret = "secret",
                }
            });
            var op = () => client.Verification;
            op.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(VerificationCredentialsMissingTestCaseData.TestCasesData),
            MemberType = typeof(VerificationCredentialsMissingTestCaseData))]
        public void VerificationWithMissingCredentials_ParseEvent_DoesNotRequireValidCredentials(
            VerificationCredentialsMissingTestCaseData data)
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = data.VerificationConfiguration
            });
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationRequestEvent.json");

            var sinchEvent = client.Verification.SinchEvents.ParseEvent(json);

            sinchEvent.Should().BeOfType<Sinch.Verification.SinchEvents.VerificationRequestEvent>();
        }

        [Fact]
        public async Task VerificationWithoutConfiguration_ThrowsWhenApiOperationNeedsAuthentication()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = null
            });

            var op = () => client.Verification.Verification.StartSms("+15551234567");

            (await op.Should().ThrowAsync<InvalidOperationException>()).Which.Message.Should()
                .Be("SinchVerificationConfiguration is not set.");
        }
    }
}
