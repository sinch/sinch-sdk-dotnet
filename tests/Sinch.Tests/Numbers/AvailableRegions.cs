using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Numbers;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class AvailableRegions : NumberTestBase
    {
        [Fact]
        public async Task List()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableRegions")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    availableRegions = new[]
                    {
                        new { regionCode = "US", regionName = "United States", types = new[] { "MOBILE" } },
                        new { regionCode = "UK", regionName = "United Kingdom", types = new[] { "LOCAL" } },
                        new { regionCode = "FR", regionName = "France", types = new[] { "TOLL_FREE", "LOCAL" } }
                    }
                }));

            var response = await Numbers.Regions.List();

            response.Count().Should().Be(3);
        }

        [Fact]
        public async Task ListWithParams()
        {
            HttpMessageHandlerMock
                .When(
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableRegions?types=LOCAL&types=TOLL_FREE")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    availableRegions = new[]
                    {
                        new { regionCode = "UK", regionName = "United Kingdom", types = new[] { "LOCAL" } },
                        new { regionCode = "FR", regionName = "France", types = new[] { "TOLL_FREE", "LOCAL" } }
                    }
                }));

            var response = await Numbers.Regions.List(new List<Types> { Types.Local, Types.TollFree });

            response.Count().Should().Be(2);
        }
    }
}
