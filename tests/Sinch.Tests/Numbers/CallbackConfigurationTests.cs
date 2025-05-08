using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Numbers.Callbacks;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class CallbackConfigurationTests : NumberTestBase
    {
        [Fact]
        public async Task Get()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/callbackConfiguration")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", Helpers.LoadResources("Numbers/CallbackConfigurationResponse.json"));

            var response = await Numbers.Callbacks.Get();

            response.Should().BeEquivalentTo(new CallbackConfiguration()
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
                .Respond("application/json", Helpers.LoadResources("Numbers/CallbackConfigurationResponse.json"));

            var response = await Numbers.Callbacks.Update("HMAC value");

            response.Should().BeEquivalentTo(new CallbackConfiguration()
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
                .Respond("application/json", Helpers.LoadResources("Numbers/CallbackConfigurationResponse.json"));

            var responseOp = () => Numbers.Callbacks.Update(hmacSecret);

            await responseOp.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
