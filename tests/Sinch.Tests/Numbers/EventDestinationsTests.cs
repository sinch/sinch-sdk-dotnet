using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Numbers.EventDestinations;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class EventDestinationsTests : NumberTestBase
    {
        [Fact]
        public async Task Get()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/callbackConfiguration")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", Helpers.LoadResources("Numbers/EventDestinationResponse.json"));

            var response = await Numbers.EventDestinations.Get();

            response.Should().BeEquivalentTo(new EventDestination()
            {
                ProjectId = "Project ID value",
                HmacSecret = "HMAC value"
            });
        }

        [Fact]
        public async Task Update()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/callbackConfiguration")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(new
                {
                    hmacSecret = "HMAC value"
                }))
                .Respond("application/json", Helpers.LoadResources("Numbers/EventDestinationResponse.json"));

            var response = await Numbers.EventDestinations.Update("HMAC value");

            response.Should().BeEquivalentTo(new EventDestination()
            {
                ProjectId = "Project ID value",
                HmacSecret = "HMAC value"
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task UpdateThrowIfHmacIsEmpty(string hmacSecret)
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/callbackConfiguration")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(new
                {
                    hmacSecret = "HMAC value"
                }))
                .Respond("application/json", Helpers.LoadResources("Numbers/EventDestinationResponse.json"));

            var responseOp = () => Numbers.EventDestinations.Update(hmacSecret);

            await responseOp.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
