using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Fax;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class FaxConfigurationTests
    {
        public record FaxUrlTestCase(
            string TestName,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly FaxUrlTestCase[] TestCases =
            {
                new("Global url", null, "https://fax.api.sinch.com/"),
                new("Custom url", "https://custom.fax.api.sinch.com/", "https://custom.fax.api.sinch.com/"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(FaxUrlTestCase.TestCasesData), MemberType = typeof(FaxUrlTestCase))]
        public void ResolveFaxUrl(FaxUrlTestCase testCase)
        {
            var faxConfig = new SinchFaxConfiguration
            {
                UrlOverride = testCase.UrlOverride,
            };
            SinchUrlResolvers.ResolveFaxUrl(faxConfig).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }


        [Fact]
        public void FaxConfiguration_ShouldUseUrlOverride_WhenUrlOverrideSpecified()
        {
            var config = new SinchFaxConfiguration
            {
                UrlOverride = "https://custom.fax.api.sinch.com/"
            };
            var faxUrl = SinchUrlResolvers.ResolveFaxUrl(config);
            faxUrl.Should().BeEquivalentTo(new Uri("https://custom.fax.api.sinch.com/"));
        }
    }
}
