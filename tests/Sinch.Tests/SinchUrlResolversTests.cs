#nullable enable
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;
using Sinch.Voice;
using Xunit;

namespace Sinch.Tests
{
    public class SinchUrlResolversTests
    {
        #region Auth

        public record AuthUrlTestCase(string TestName, string? UrlOverride, string ExpectedUrl)
        {
            private static readonly AuthUrlTestCase[] TestCases =
            {
                new("Default Auth URL", null, "https://auth.sinch.com/"),
                new("Custom override", "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(AuthUrlTestCase.TestCasesData), MemberType = typeof(AuthUrlTestCase))]
        public void ResolveAuthUrl(AuthUrlTestCase testCase)
        {
            var config = new SinchOAuthConfiguration { UrlOverride = testCase.UrlOverride };
            SinchUrlResolvers.ResolveAuthUrl(config).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        #endregion

        #region Numbers

        public record NumbersUrlTestCase(string TestName, string? UrlOverride, string ExpectedUrl)
        {
            private static readonly NumbersUrlTestCase[] TestCases =
            {
                new("Default Numbers URL", null, "https://numbers.api.sinch.com/"),
                new("Custom override", "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(NumbersUrlTestCase.TestCasesData), MemberType = typeof(NumbersUrlTestCase))]
        public void ResolveNumbersUrl(NumbersUrlTestCase testCase)
        {
            var config = new SinchNumbersConfiguration { UrlOverride = testCase.UrlOverride };
            SinchUrlResolvers.ResolveNumbersUrl(config).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        #endregion

        #region Conversation

        public record ConversationUrlTestCase(
            string TestName,
            ConversationRegion Region,
            string? UrlOverride,
            string Expected)
        {
            private static readonly ConversationUrlTestCase[] TestCases =
            {
                new("Brazil region", ConversationRegion.Br, null, "https://br.conversation.api.sinch.com/"),
                new("Europe region", ConversationRegion.Eu, null, "https://eu.conversation.api.sinch.com/"),
                new("Us region", ConversationRegion.Us, null, "https://us.conversation.api.sinch.com/"),
                new("Url override", ConversationRegion.Us, "https://hello.world", "https://hello.world/"),
                new("Custom Region", new ConversationRegion("CA"), null, "https://ca.conversation.api.sinch.com/"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(ConversationUrlTestCase.TestCasesData), MemberType = typeof(ConversationUrlTestCase))]
        public void ResolveConversationUrl(ConversationUrlTestCase testCase)
        {
            var config = new SinchConversationConfiguration
            {
                Region = testCase.Region,
                ConversationUrlOverride = testCase.UrlOverride,
            };
            SinchUrlResolvers.ResolveConversationUrl(config).ToString().Should().Be(testCase.Expected);
        }

        #endregion

        #region Fax

        public record FaxUrlTestCase(string TestName, string? UrlOverride, string ExpectedUrl)
        {
            private static readonly FaxUrlTestCase[] TestCases =
            {
                new("Global url", null, "https://fax.api.sinch.com/"),
                new("Custom url", "https://custom.fax.api.sinch.com/", "https://custom.fax.api.sinch.com/"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(FaxUrlTestCase.TestCasesData), MemberType = typeof(FaxUrlTestCase))]
        public void ResolveFaxUrl(FaxUrlTestCase testCase)
        {
            var config = new SinchFaxConfiguration { UrlOverride = testCase.UrlOverride };
            SinchUrlResolvers.ResolveFaxUrl(config).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        #endregion

        #region Verification

        [Theory]
        [InlineData(null, "https://verification.api.sinch.com/")]
        [InlineData("https://hello.world", "https://hello.world/")]
        public void ResolveVerificationUrl(string? urlOverride, string expectedUrl)
        {
            var config = new SinchVerificationConfiguration
            {
                AppKey = "key",
                AppSecret = "secret",
                UrlOverride = urlOverride,
            };
            SinchUrlResolvers.ResolveVerificationUrl(config).ToString().Should().Be(expectedUrl);
        }

        #endregion

        #region Voice

        public record VoiceUrlTestCase(
            string TestName,
            VoiceRegion Region,
            string? UrlOverride,
            string ExpectedUrl)
        {
            private static readonly VoiceUrlTestCase[] TestCases =
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
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(VoiceUrlTestCase.TestCasesData), MemberType = typeof(VoiceUrlTestCase))]
        public void ResolveVoiceUrl(VoiceUrlTestCase testCase)
        {
            var config = new SinchVoiceConfiguration
            {
                AppKey = "key",
                AppSecret = "secret",
                VoiceUrlOverride = testCase.UrlOverride,
                Region = testCase.Region
            };
            SinchUrlResolvers.ResolveVoiceUrl(config).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        [Theory]
        [InlineData(null, "https://callingapi.sinch.com/")]
        [InlineData("https://hello.world", "https://hello.world/")]
        public void ResolveVoiceApplicationManagementUrl(string? urlOverride, string expected)
        {
            var config = new SinchVoiceConfiguration
            {
                AppKey = "key",
                AppSecret = "secret",
                ApplicationManagementUrlOverride = urlOverride,
            };
            SinchUrlResolvers.ResolveVoiceApplicationManagementUrl(config).ToString().Should().BeEquivalentTo(expected);
        }

        #endregion

        #region SMS

        public record SmsServicePlanIdTestCase(
            string TestName,
            SmsServicePlanIdRegion Region,
            string? UrlOverride,
            string ExpectedUrl)
        {
            private static readonly SmsServicePlanIdTestCase[] TestCases =
            {
                new("Default US SMS region", SmsServicePlanIdRegion.Us, null, "https://us.sms.api.sinch.com/"),
                new("Default EU SMS region", SmsServicePlanIdRegion.Eu, null, "https://eu.sms.api.sinch.com/"),
                new("Default AU SMS region", SmsServicePlanIdRegion.Au, null, "https://au.sms.api.sinch.com/"),
                new("Default BR SMS region", SmsServicePlanIdRegion.Br, null, "https://br.sms.api.sinch.com/"),
                new("Default CA SMS region", SmsServicePlanIdRegion.Ca, null, "https://ca.sms.api.sinch.com/"),
                new("US region with null override", SmsServicePlanIdRegion.Us, null, "https://us.sms.api.sinch.com/"),
                new("EU region with custom override", SmsServicePlanIdRegion.Eu, "https://hello.world",
                    "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(SmsServicePlanIdTestCase.TestCasesData), MemberType = typeof(SmsServicePlanIdTestCase))]
        public void ResolveSmsServicePlanIdUrl(SmsServicePlanIdTestCase testCase)
        {
            var config = SinchSmsConfiguration.WithServicePlanId("service-plan-id", "token", testCase.Region,
                testCase.UrlOverride);
            SinchUrlResolvers.ResolveSmsServicePlanIdUrl(config.ServicePlanIdConfiguration!)
                .ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        public record SmsUrlTestCase(string TestName, SmsRegion? Region, string? UrlOverride, string ExpectedUrl)
        {
            private static readonly SmsUrlTestCase[] TestCases =
            {
                new("Default EU SMS region", SmsRegion.Eu, null, "https://zt.eu.sms.api.sinch.com/"),
                new("Default US SMS region with null override", SmsRegion.Us, null,
                    "https://zt.us.sms.api.sinch.com/"),
                new("EU region with custom override", SmsRegion.Eu, "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(t => new object[] { t });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(SmsUrlTestCase.TestCasesData), MemberType = typeof(SmsUrlTestCase))]
        public void ResolveSmsUrl(SmsUrlTestCase testCase)
        {
            var config = new SinchSmsConfiguration
            {
                Region = testCase.Region,
                UrlOverride = testCase.UrlOverride,
            };
            SinchUrlResolvers.ResolveSmsUrl(config).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }

        #endregion
    }
}
