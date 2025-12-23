using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.SMS;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class SmsConfigurationTests
    {
        public record SmsServicePlanIdTestCase(
            string TestName,
            SmsServicePlanIdRegion Region,
            string UrlOverride,
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
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(SmsServicePlanIdTestCase.TestCasesData), MemberType = typeof(SmsServicePlanIdTestCase))]
        public void ResolveSmsServicePlanIdUrl(SmsServicePlanIdTestCase testCase)
        {
            var smsServicePlanIdConfig =
                SinchSmsConfiguration.WithServicePlanId("service-plan-id", "token", testCase.Region,
                    testCase.UrlOverride);
            smsServicePlanIdConfig.ServicePlanIdConfiguration!.ResolveUrl().ToString().Should()
                .BeEquivalentTo(testCase.ExpectedUrl);
        }

        public record SmsUrlTestCase(
            string TestName,
            SmsRegion Region,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly SmsUrlTestCase[] TestCases =
            {
                new("Default EU SMS region", SmsRegion.Eu, null, "https://zt.eu.sms.api.sinch.com/"),
                new("Default US SMS region with null override", SmsRegion.Us, null, "https://zt.us.sms.api.sinch.com/"),
                new("EU region with custom override", SmsRegion.Eu, "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(SmsUrlTestCase.TestCasesData), MemberType = typeof(SmsUrlTestCase))]
        public void ResolveSmsUrl(SmsUrlTestCase testCase)
        {
            var smsConfig = new SinchSmsConfiguration()
            {
                Region = testCase.Region,
                UrlOverride = testCase.UrlOverride,
            };
            smsConfig.ResolveUrl().ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }
    }
}
