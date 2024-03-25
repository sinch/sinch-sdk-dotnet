﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS;
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
            var sinchClient = new SinchClient("TEST", "TESTKEY", "TESTSECRET",
                options =>
                {
                    options.HttpClient = httpClient;
                    options.UseServicePlanIdWithSms(servicePlanId, SmsServicePlanIdHostingRegion.Au, apiToken);
                });

            var batchId = "b1";
            // in url AU region should be set and path param contain service plan id
            httpMessageHandlerMock.When(HttpMethod.Delete,
                    $"https://au.sms.api.sinch.com/xms/v1/{servicePlanId}/batches/{batchId}")
                // bearer with provided token
                .WithHeaders("Authorization", $"Bearer {apiToken}")
                .Respond(HttpStatusCode.OK);

            var op = () => sinchClient.Sms.Batches.Cancel(batchId);
            await op.Should().NotThrowAsync();
        }
    }
}
