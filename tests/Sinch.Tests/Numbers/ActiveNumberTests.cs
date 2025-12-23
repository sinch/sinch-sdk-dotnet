using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Numbers;
using Sinch.Numbers.Active;
using Sinch.Numbers.Active.List;
using Sinch.Numbers.Active.Update;
using Sinch.Numbers.VoiceConfigurations;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class ActiveNumberTests : NumberTestBase
    {
        [Fact]
        public async Task Get()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers/+12025550134")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.ActiveNumber));

            var response = await Numbers.Get("+12025550134");

            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task List()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers?regionCode=US&type=MOBILE")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    activeNumbers = new[]
                    {
                        TestData.ActiveNumber
                    },
                    nextPageToken = "004",
                    totalSize = 5
                }));

            var response = await Numbers.List(new ListActiveNumbersRequest
            {
                RegionCode = "US",
                Type = Types.Mobile,
                NumberPattern = null,
                Capability = null,
                PageSize = null,
                PageToken = null,
                OrderBy = null
            });

            response.Should().NotBeNull();
            response.ActiveNumbers.Should().HaveCount(1);
            var firstActiveNumber = response.ActiveNumbers.First();
            firstActiveNumber.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task ListWithFullParams()
        {
            var url = $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers?regionCode=US&type=LOCAL";
            url += "&numberPattern.pattern=2020&numberPattern.searchPattern=CONTAINS";
            url += "&capability=SMS&capability=VOICE";
            url += "&pageSize=128";
            url += "&pageToken=i32dsan";
            url += "&orderBy=displayName";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    activeNumbers = new[]
                    {
                        TestData.ActiveNumber
                    },
                    nextPageToken = "004",
                    totalSize = 5
                }));

            var request = new ListActiveNumbersRequest
            {
                RegionCode = "US",
                Type = Types.Local,
                PageToken = "i32dsan",
                OrderBy = OrderBy.DisplayName,
                PageSize = 128,
                Capability = new List<Product> { Product.Sms, Product.Voice },
                NumberPattern = new NumberPattern { Pattern = "2020", SearchPattern = SearchPattern.Contain }
            };
            var response = await Numbers.List(request);

            response.Should().NotBeNull();
            response.ActiveNumbers.Should().HaveCount(1);
            var firstActiveNumber = response.ActiveNumbers.First();
            firstActiveNumber.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task Release()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers/+12025550134:release")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.ActiveNumber));

            var response = await Numbers.Release("+12025550134");

            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Fact]
        public async Task Update()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers/+12025550134")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("Name")
                .Respond(HttpStatusCode.OK, JsonContent.Create(TestData.ActiveNumber));


            var response = await Numbers.Update("+12025550134",
                new UpdateActiveNumberRequest
                {
                    DisplayName = "Name",
                    SmsConfiguration = new SmsConfiguration
                    {
                        ServicePlanId = "SERVICEPLAN"
                    }
                });

            response.Should().NotBeNull();
            response.PhoneNumber.Should().Be("+12025550134");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ListAuto(string emptyTokenVariation)
        {
            var baseUri =
                $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers?regionCode=US&type=MOBILE";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    activeNumbers = new[]
                    {
                        TestData.ActiveNumber
                    },
                    nextPageToken = "1",
                    totalSize = 3
                }));
            HttpMessageHandlerMock
                .Expect(baseUri + "&pageToken=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    activeNumbers = new[]
                    {
                        TestData.ActiveNumber
                    },
                    nextPageToken = "2",
                    totalSize = 3
                }));
            HttpMessageHandlerMock
                .Expect(baseUri + "&pageToken=2")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    activeNumbers = new[]
                    {
                        TestData.ActiveNumber
                    },
                    nextPageToken = emptyTokenVariation,
                    totalSize = 3
                }));


            var res = Numbers.ListAuto(new ListActiveNumbersRequest
            {
                RegionCode = "US",
                Type = Types.Mobile,
                NumberPattern = null,
                Capability = null,
                PageSize = null,
                PageToken = null,
                OrderBy = null
            });

            await foreach (var activeNumber in res)
            {
                activeNumber.Should().NotBeNull();
            }

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        private readonly ActiveNumber _activeNumber = new ActiveNumber
        {
            PhoneNumber = "+447520651116XYZ",
            ProjectId = "project id",
            DisplayName = "a display",
            RegionCode = "GB",
            Type = Types.Mobile,
            Capability = new List<Product> { Product.Sms, Product.Voice },
            Money = new Money
            {
                CurrencyCode = "EUR",
                Amount = 0.8m
            },
            PaymentIntervalMonths = 1,
            NextChargeDate = Helpers.ParseUtc("2023-09-22T15:49:58.813424Z"),
            ExpireAt = Helpers.ParseUtc("2023-10-06T15:49:58.813381Z"),
            SmsConfiguration = new SmsConfiguration
            {
                ServicePlanId = "service plan id",
                CampaignId = "campaign id",
                ScheduledProvisioning = new ScheduledProvisioning
                {
                    ServicePlanId = "service plan id from scheduled",
                    CampaignId = "campaign id from scheduled",
                    Status = ProvisioningStatus.Unspecified,
                    LastUpdatedTime = Helpers.ParseUtc("2023-09-25T12:08:02.115Z"),
                    ErrorCodes = new List<string> { "ERROR_CODE_UNSPECIFIED" }
                }
            },
            VoiceConfiguration = new VoiceRtcConfiguration
            {
                AppId = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEE",
                LastUpdatedTime = Helpers.ParseUtc("2024-06-30T07:08:09.100Z"),
                ScheduledVoiceProvisioning = new ScheduledVoiceRtcProvisioning()
                {
                    AppId = "EEEEEEEEEE-DDDD-CCCC-BBBB-AAAAAAAA",
                    Status = ProvisioningStatus.Waiting,
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                }
            },
            CallbackUrl = "foo callback"
        };


        [Fact]
        public void DeserializeActiveNumber()
        {
            var json = Helpers.LoadResources("Numbers/Active/ActiveNumber.json");
            var activeNumber = DeserializeAsNumbersClient<ActiveNumber>(json);
            activeNumber.Should().BeOfType<ActiveNumber>().And.BeEquivalentTo(_activeNumber);
        }

        [Fact]
        public async Task UpdateActiveNumber()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Patch,
                    $"https://numbers.api.sinch.com/v1/projects/{ProjectId}/activeNumbers/+12025550134")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Numbers/Active/ActiveNumberUpdateRequest.json"))
                .Respond("application/json",
                    Helpers.LoadResources("Numbers/Active/ActiveNumber.json"));

            var response = await Numbers.Update("+12025550134", new UpdateActiveNumberRequest()
            {
                DisplayName = "a display",
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "service plan id",
                    CampaignId = "campaign id"
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEE",
                },
                CallbackUrl = "foo callback"
            });

            response.Should().BeEquivalentTo(_activeNumber);
        }
    }
}
