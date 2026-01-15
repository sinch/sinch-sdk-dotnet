using System;
using FluentAssertions;
using Sinch.Fax;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class FaxClientTests
    {
        [Fact]
        public void FaxConfiguration_ShouldResolveDefaultUrl_WhenNoRegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldResolveEuropeUrl_WhenEuropeRegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.Europe
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://eu1.fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldResolveUsEastCoastUrl_WhenUsEastCoastRegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.UsEastCost
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://use1.fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldResolveSouthAmericaUrl_WhenSouthAmericaRegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.SouthAmerica
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://sae1.fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldResolveSouthEastAsia1Url_WhenSouthEastAsia1RegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.SouthEastAsia1
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://apse1.fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldResolveSouthEastAsia2Url_WhenSouthEastAsia2RegionSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    Region = FaxRegion.SouthEastAsia2
                }
            };
            var faxUrl = config.FaxConfiguration.ResolveUrl();
            faxUrl.Should().BeEquivalentTo(new Uri("https://apse2.fax.api.sinch.com/"));
        }

        [Fact]
        public void FaxConfiguration_ShouldUseUrlOverride_WhenUrlOverrideSpecified()
        {
            var config = new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
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

