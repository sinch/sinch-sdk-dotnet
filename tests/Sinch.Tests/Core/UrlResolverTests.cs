using System;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Core;
using Sinch.Fax;
using Sinch.SMS;
using Xunit;

namespace Sinch.Tests.Core
{
    public class UrlResolverTests
    {
        

       

        public static IEnumerable<object[]> FaxUrlData => new List<object[]>
        {
            new object[]
            {
                FaxRegion.Europe,
                null,
            },
            new object[]
            {
                FaxRegion.UsEastCost,
                null,
            },
            new object[]
            {
                null,
                null,
            },
            new object[]
            {
                FaxRegion.Europe,
                new ApiUrlOverrides()
                {
                    FaxUrl = null
                }
            },
            new object[]
            {
                FaxRegion.Europe,
                new ApiUrlOverrides()
                {
                    FaxUrl = "https://new-fax.url"
                }
            },
        };

        [Theory]
        [MemberData(nameof(FaxUrlData))]
        public void ResolveFaxUrl(FaxRegion faxRegion, ApiUrlOverrides apiUrlOverrides)
        {
            var faxUrl = new UrlResolver(apiUrlOverrides).ResolveFaxUrl(faxRegion);
            if (apiUrlOverrides?.FaxUrl != null)
            {
                faxUrl.Should().BeEquivalentTo(new Uri(apiUrlOverrides.FaxUrl));
            }
            else if (faxRegion != null)
            {
                faxUrl.Should().BeEquivalentTo(new Uri($"https://{faxRegion.Value}.fax.api.sinch.com/"));
            }
            else
            {
                faxUrl.Should().BeEquivalentTo(new Uri("https://fax.api.sinch.com/"));
            }
        }
    }
}
