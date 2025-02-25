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
            FaxRegion? Region,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly FaxUrlTestCase[] TestCases =
            {
                new("Default Europe Fax region", FaxRegion.Europe, null, "https://eu1.fax.api.sinch.com/"),
                new("Default US East Coast Fax region", FaxRegion.UsEastCost, null, "https://use1.fax.api.sinch.com/"),
                new("No region specified", null, null, "https://fax.api.sinch.com/"),
                new("Europe region with null override", FaxRegion.Europe, null, "https://eu1.fax.api.sinch.com/"),
                new("Europe region with custom override", FaxRegion.Europe, "https://new-fax.url", "https://new-fax.url/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(FaxUrlTestCase.TestCasesData), MemberType = typeof(FaxUrlTestCase))]
        public void ResolveFaxUrl(FaxUrlTestCase testCase)
        {
            var faxConfig = new SinchFaxConfiguration()
            {
                Region = testCase.Region,
                UrlOverride = testCase.UrlOverride,
            };
            faxConfig.ResolveUrl().ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }
    }
}
