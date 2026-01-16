using System;
using FluentAssertions;
using Sinch.Fax;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class FaxClientTests
    {
        public static TheoryData<FaxRegion, string> RegionUrlTestData => new()
        {
            { null, "https://fax.api.sinch.com/" },
            { FaxRegion.Europe, "https://eu1.fax.api.sinch.com/" },
            { FaxRegion.UsEastCost, "https://use1.fax.api.sinch.com/" },
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
    }
}

