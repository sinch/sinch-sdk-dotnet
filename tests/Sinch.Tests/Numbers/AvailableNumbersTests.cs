using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Numbers;
using Sinch.Numbers.Available;
using Sinch.Numbers.Available.Rent;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class AvailableNumbersTests : NumberTestBase
    {
        [Fact]
        public async Task Rent()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableNumbers/+12025550134:rent")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("service_plan")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.ActiveNumber));

            var request = new RentActiveNumberRequest
            {
                SmsConfiguration = new SmsConfiguration
                {
                    ServicePlanId = "service_plan",
                    CampaignId = "campaign"
                },
                VoiceConfiguration = new VoiceConfiguration
                {
                    AppId = "app_id"
                }
            };

            var response = await Numbers.Available.Rent("+12025550134", request);
            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task RentAny()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableNumbers:rentAny")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("1208")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.ActiveNumber));

            var request = new Sinch.Numbers.Available.RentAny.RentAnyNumberRequest
            {
                RegionCode = "US",
                NumberPattern = new NumberPattern
                {
                    Pattern = "+1208",
                    SearchPattern = SearchPattern.Start
                },
                Type = Types.TollFree,
                Capabilities = new List<Product> { Product.Sms },
                SmsConfiguration = new SmsConfiguration
                {
                    ServicePlanId = "plan_id",
                    CampaignId = "campaign_id"
                }
            };
            var response = await Numbers.Available.RentAny(request);

            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task Search()
        {
            var url =
                $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableNumbers?regionCode=US&type=MOBILE";
            url += "&numberPattern.pattern=%2b2020";
            url += "&numberPattern.searchPattern=START";
            url += "&capabilities=SMS&capabilities=VOICE";
            url += "&size=10";
            HttpMessageHandlerMock.When(HttpMethod.Get, url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    availableNumbers = new[] { TestData.AvailableNumber }
                }));

            var response = await Numbers.Available.List(
                new Sinch.Numbers.Available.List.ListAvailableNumbersRequest
                {
                    RegionCode = "US",
                    Type = Types.Mobile,
                    NumberPattern = new NumberPattern
                    {
                        Pattern = "+2020",
                        SearchPattern = SearchPattern.Start
                    },
                    Capabilities = new List<Product> { Product.Sms, Product.Voice },
                    Size = 10
                });

            response.Should().NotBeNull();
            response.AvailableNumbers.Count().Should().Be(1);
        }

        [Fact]
        public async Task CheckAvailability()
        {
            var url = $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableNumbers/+12025550134";

            HttpMessageHandlerMock.Expect(HttpMethod.Get, url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.AvailableNumber));

            var response = await Numbers.Available.CheckAvailability("+12025550134");

            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task AvailableError()
        {
            var url = $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/availableNumbers/+12025550";
            HttpMessageHandlerMock.When(HttpMethod.Get, url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.BadRequest, JsonContent.Create(new
                {
                    error = new
                    {
                        code = 404,
                        message = "",
                        status = "NOT_FOUND",
                        details = new[]
                        {
                            new
                            {
                                type = "ResourceInfo",
                                resourceType = "AvailableNumber",
                                resourceName = "+341123",
                                owner = "",
                                description = ""
                            }
                        }
                    }
                }));

            Func<Task<AvailableNumber>> response = () => Numbers.Available.CheckAvailability("+12025550");

            var exception = await response.Should().ThrowAsync<SinchApiException>();
            var node = exception.And.Details!.First();
            node["type"]!.GetValue<string>().Should().Be("ResourceInfo");
            node["resourceType"]!.GetValue<string>().Should().Be("AvailableNumber");
            node["resourceName"]!.GetValue<string>().Should().Be("+341123");
            node["owner"]!.GetValue<string>().Should().Be("");
            node["description"]!.GetValue<string>().Should().Be("");
        }
    }
}
