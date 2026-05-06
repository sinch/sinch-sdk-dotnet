using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Sinch.SMS;
using Sinch.SMS.DeliveryReports;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class SmsClientTests
    {
        [Fact]
        public async Task UseSmsWithServicePlanId()
        {
            const string servicePlanId = "SERVICE_PLAN_ID";
            const string apiToken = "api_token_x";
            MockHttpMessageHandler httpMessageHandlerMock = new();
            var httpClient = new HttpClient(httpMessageHandlerMock);
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);

            var sinchClient = new SinchClient(new SinchClientConfiguration()
            {
                SmsConfiguration =
                    SinchSmsConfiguration.WithServicePlanId(servicePlanId, apiToken, SmsServicePlanIdRegion.Au),
                SinchOptions = new SinchOptions()
                {
                    HttpClientFactory = httpClientFactory
                }
            });

            var batchId = "b1";
            // in url AU region should be set and path param contain service plan id
            httpMessageHandlerMock.When(HttpMethod.Delete,
                    $"https://au.sms.api.sinch.com/xms/v1/{servicePlanId}/batches/{batchId}")
                // bearer with provided token
                .WithHeaders("Authorization", $"Bearer {apiToken}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "123",
                    type = SmsType.MtText.Value,
                    body = "hello",
                    to = new List<string>() { "+4800000" }
                }));

            var op = () => sinchClient.Sms.Batches.Cancel(batchId);
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public void ResolveSmsUrl_ThrowsWhenRegionNotSet()
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
            var act = () => client.Sms;
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
                SmsConfiguration = new SinchSmsConfiguration { Region = SmsRegion.Us }
            });
            var act = () => client.Sms;
            act.Should().NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void SmsWithServicePlanId_ThrowsWhenRegionNotSet()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("servicePlanId", "apiToken", null!)
            });
            var act = () => sinch.Sms;
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*ServicePlanIdRegion*required*");
        }

        [Fact]
        public void SmsWithServicePlanId_DoesNotThrow_WhenRegionIsSet()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("servicePlanId", "apiToken", SmsServicePlanIdRegion.Eu)
            });
            var act = () => sinch.Sms;
            act.Should().NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void Sms_SinchEvents_ParseEvent_DoesNotRequireCredentials()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SmsConfiguration = new SinchSmsConfiguration
                {
                    Region = SmsRegion.Us
                }
            });
            var json = Helpers.LoadResources("Sms/Hooks/DeliveryReportSms.json");

            var smsEvent = client.Sms.SinchEvents.ParseEvent(json);

            smsEvent.Should().BeOfType<BatchDeliveryReportSms>();
        }
    }
}
