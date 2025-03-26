using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Callouts;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class VoiceClientTests
    {
        [Fact]
        public void InitVoiceWithGlobalRegion()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "key",
                    AppSecret = "secret",
                }
            });
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(client.Voice.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling.api.sinch.com/"));
        }

        [Fact]
        public void InitVoiceWithEastAsia1Region()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "key",
                    AppSecret = "secret",
                    Region = VoiceRegion.SouthEastAsia1
                }
            });
            var voiceClient = client.Voice;
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling-apse1.api.sinch.com/"));
        }


        public record VoiceCredentialsMissingTestCaseData(
            string TestName,
            SinchVoiceConfiguration VoiceConfiguration,
            string ParamName,
            string Message)
        {
            private static readonly VoiceCredentialsMissingTestCaseData[] TestCases =
            {
                new("AppKey is null", new SinchVoiceConfiguration()
                    {
                        AppKey = null!,
                        AppSecret = "some"
                    },
                    $"{nameof(SinchVoiceConfiguration.AppKey)}",
                    "The value should be present (Parameter 'AppKey')"),
                new("AppKey is empty", new SinchVoiceConfiguration()
                    {
                        AppKey = string.Empty,
                        AppSecret = "some"
                    },
                    $"{nameof(SinchVoiceConfiguration.AppKey)}",
                    "The value should be present (Parameter 'AppKey')"),
                new("AppSecret is null", new SinchVoiceConfiguration()
                    {
                        AppKey = "some",
                        AppSecret = null!
                    },
                    $"{nameof(SinchVoiceConfiguration.AppSecret)}",
                    "The value should be present (Parameter 'AppSecret')"),
                new("AppSecret is empty", new SinchVoiceConfiguration()
                    {
                        AppKey = "aaa",
                        AppSecret = string.Empty
                    },
                    $"{nameof(SinchVoiceConfiguration.AppSecret)}",
                    "The value should be present (Parameter 'AppSecret')"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(VoiceCredentialsMissingTestCaseData.TestCasesData),
            MemberType = typeof(VoiceCredentialsMissingTestCaseData))]
        public void ThrowIfVoiceCredentialsAreMissing(VoiceCredentialsMissingTestCaseData data)
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VoiceConfiguration = data.VoiceConfiguration
            });
            var voiceOp = () => client.Voice;
            var which = voiceOp.Should().ThrowExactly<ArgumentNullException>().Which;
            which.ParamName.Should().Be(data.ParamName);
            which.Message.Should().Be(data.Message);
        }

        [Fact]
        public void ThrowIfVoiceConfigIsNull()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                VoiceConfiguration = null
            });
            var voiceOp = () => client.Voice;
            voiceOp.Should().ThrowExactly<InvalidOperationException>().Which.Message.Should()
                .Be("SinchVoiceConfiguration is not set.");
        }
    }
}
