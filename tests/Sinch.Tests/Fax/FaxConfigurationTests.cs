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
            SinchUrlResolvers.ResolveFaxUrl(faxConfig).ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
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
            var config = new SinchFaxConfiguration() { Region = region };
            var faxUrl = SinchUrlResolvers.ResolveFaxUrl(config);
            faxUrl.Should().BeEquivalentTo(new Uri(expectedUrl));
        }

        [Fact]
        public void FaxConfiguration_ShouldUseUrlOverride_WhenUrlOverrideSpecified()
        {
            var config = new SinchFaxConfiguration()
            {
                Region = FaxRegion.Europe,
                UrlOverride = "https://custom.fax.api.sinch.com/"
            };
            var faxUrl = SinchUrlResolvers.ResolveFaxUrl(config);
            faxUrl.Should().BeEquivalentTo(new Uri("https://custom.fax.api.sinch.com/"));
        }

        [Fact]
        public void Validate_ThrowsWhenRegionNotSet()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    KeyId = "key-id",
                    KeySecret = "key-secret",
                    ProjectId = "project-id"
                }
            });
            var act = () => client.Fax;
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Region*required*");
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenRegionIsSet()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    KeyId = "key-id",
                    KeySecret = "key-secret",
                    ProjectId = "project-id"
                },
                FaxConfiguration = new SinchFaxConfiguration { Region = FaxRegion.Europe }
            });
            var act = () => client.Fax;
            act.Should().NotThrow<InvalidOperationException>();
        }
    }
}
