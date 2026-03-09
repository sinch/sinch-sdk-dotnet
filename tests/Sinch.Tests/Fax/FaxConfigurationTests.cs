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
            FaxRegion Region,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly FaxUrlTestCase[] TestCases =
            {
                new("Default Europe Fax region", FaxRegion.Europe, null, "https://eu1.fax.api.sinch.com/"),
                new("Default US East Coast Fax region", FaxRegion.UsEastCoast, null, "https://use1.fax.api.sinch.com/"),
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

        public static TheoryData<FaxRegion, string> RegionUrlTestData => new()
        {
            { FaxRegion.Europe, "https://eu1.fax.api.sinch.com/" },
            { FaxRegion.UsEastCoast, "https://use1.fax.api.sinch.com/" },
            { FaxRegion.SouthAmerica, "https://sae1.fax.api.sinch.com/" },
            { FaxRegion.SouthEastAsia1, "https://apse1.fax.api.sinch.com/" },
            { FaxRegion.SouthEastAsia2, "https://apse2.fax.api.sinch.com/" },
        };

        [Theory]
        [MemberData(nameof(RegionUrlTestData))]
        public void FaxConfiguration_ShouldResolveCorrectUrl_WhenRegionSpecified(FaxRegion region, string expectedUrl)
        {
            var config = new SinchClientConfiguration()
            {
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = region
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri(expectedUrl));
        }

        [Fact]
        public void FaxConfiguration_ShouldUseUrlOverride_WhenUrlOverrideSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.Europe, // This should be ignored when UrlOverride is set
                    UrlOverride = "https://custom.fax.api.sinch.com/"
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://custom.fax.api.sinch.com/"));
        }

        [Fact]
        public void Validate_ThrowsWhenRegionNotSet()
        {
            var config = new SinchFaxConfiguration();
            var act = () => config.Validate();
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Region*required*");
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenRegionIsSet()
        {
            var config = new SinchFaxConfiguration { Region = FaxRegion.Europe };
            var act = () => config.Validate();
            act.Should().NotThrow();
        }
    }
}
