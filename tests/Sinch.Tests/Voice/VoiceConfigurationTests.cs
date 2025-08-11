using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Voice;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class VoiceConfigurationTests
    {
        public record ResolveUrlData(
            string TestName,
            VoiceRegion Region,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly ResolveUrlData[] TestCases =
            {
                new("Default Global voice region", VoiceRegion.Global, null, "https://calling.api.sinch.com/"),
                new("Default Europe voice region", VoiceRegion.Europe, null, "https://calling-euc1.api.sinch.com/"),
                new("Default North America voice region", VoiceRegion.NorthAmerica, null,
                    "https://calling-use1.api.sinch.com/"),
                new("Default South America voice region", VoiceRegion.SouthAmerica, null,
                    "https://calling-sae1.api.sinch.com/"),
                new("Default SouthEast Asia 2 voice region", VoiceRegion.SouthEastAsia2, null,
                    "https://calling-apse2.api.sinch.com/"),
                new("Default SouthEast Asia 1 voice region", VoiceRegion.SouthEastAsia1, null,
                    "https://calling-apse1.api.sinch.com/"),
                new("Override URL if present", VoiceRegion.SouthEastAsia1, "https://hello.world",
                    "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString()
            {
                return TestName;
            }
        }

        [Theory]
        [MemberData(nameof(ResolveUrlData.TestCasesData), MemberType = typeof(ResolveUrlData))]
        public void ResolveVoiceUrl(ResolveUrlData testCase)
        {
            var voiceConfig = new SinchVoiceConfiguration()
            {
                AppKey = "key",
                AppSecret = "secret",
                VoiceUrlOverride = testCase.UrlOverride,
                Region = testCase.Region
            };
            var actual = voiceConfig.ResolveUrl().ToString();
            actual.Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        [Theory]
        [InlineData(null, "https://callingapi.sinch.com/")]
        [InlineData("https://hello.world", "https://hello.world/")]
        public void ResolveVoiceApplicationManagementUrl(string apiUrlOverride, string expected)
        {
            var voiceConfig = new SinchVoiceConfiguration()
            {
                AppKey = "key",
                AppSecret = "secret",
                ApplicationManagementUrlOverride = apiUrlOverride,
            };
            var actual = voiceConfig.ResolveApplicationManagementUrl().ToString();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
