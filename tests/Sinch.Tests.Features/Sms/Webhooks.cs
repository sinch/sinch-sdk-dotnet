using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Hooks;
using Sinch.SMS.Inbounds;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class Webhooks
    {
        private const string WebhookSecret = "KayakingTheSwell";
        private const string WebhooksUrlPrefix = "http://localhost:3017/webhooks/sms";

        private readonly HttpClient _httpClient = new();
        private static ISmsWebhooks _webhooks;
        private HttpResponseMessage _incomingSmsResponse;
        private HttpResponseMessage _deliveryReportResponse;
        private HttpResponseMessage _recipientDeliveryReportDeliveredResponse;
        private HttpResponseMessage _recipientDeliveryReportAbortedResponse;
        private string _rawIncomingSmsContent;
        private string _rawDeliveryReportContent;
        private string _rawRecipientDeliveryReportDeliveredContent;
        private string _rawRecipientDeliveryReportAbortedContent;

        [Given(@"the SMS Webhooks handler is available")]
        public void GivenTheSmsWebhooksHandlerIsAvailable()
        {
            _webhooks = Utils.SinchClient.Sms.Webhooks;
        }

        [When(@"I send a request to trigger an ""incoming SMS"" event")]
        public async Task WhenISendARequestToTriggerAnIncomingSmsEvent()
        {
            _incomingSmsResponse = await _httpClient.GetAsync($"{WebhooksUrlPrefix}/incoming-sms");
        }

        [When(@"I send a request to trigger an ""SMS delivery report"" event")]
        public async Task WhenISendARequestToTriggerAnSmsDeliveryReportEvent()
        {
            _deliveryReportResponse = await _httpClient.GetAsync($"{WebhooksUrlPrefix}/delivery-report-sms");
        }

        [When(@"I send a request to trigger an ""SMS recipient delivery report"" event with the status ""Delivered""")]
        public async Task WhenISendARequestToTriggerAnSmsRecipientDeliveryReportEventWithStatusDelivered()
        {
            _recipientDeliveryReportDeliveredResponse = await _httpClient.GetAsync($"{WebhooksUrlPrefix}/recipient-delivery-report-sms-delivered");
        }

        [When(@"I send a request to trigger an ""SMS recipient delivery report"" event with the status ""Aborted""")]
        public async Task WhenISendARequestToTriggerAnSmsRecipientDeliveryReportEventWithStatusAborted()
        {
            _recipientDeliveryReportAbortedResponse = await _httpClient.GetAsync($"{WebhooksUrlPrefix}/recipient-delivery-report-sms-aborted");
        }

        [Then(@"the header of the event ""IncomingSMS"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventIncomingSmsContainsAValidSignature()
        {
            _rawIncomingSmsContent = await _incomingSmsResponse.Content.ReadAsStringAsync();
            (await ValidateWebhookSignatureHeadersPresent(_incomingSmsResponse)).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportContainsAValidSignature()
        {
            _rawDeliveryReportContent = await _deliveryReportResponse.Content.ReadAsStringAsync();
            (await ValidateWebhookSignatureHeadersPresent(_deliveryReportResponse)).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" with the status ""Delivered"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportWithStatusDeliveredContainsAValidSignature()
        {
            _rawRecipientDeliveryReportDeliveredContent = await _recipientDeliveryReportDeliveredResponse.Content.ReadAsStringAsync();
            (await ValidateWebhookSignatureHeadersPresent(_recipientDeliveryReportDeliveredResponse)).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" with the status ""Aborted"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportWithStatusAbortedContainsAValidSignature()
        {
            _rawRecipientDeliveryReportAbortedContent = await _recipientDeliveryReportAbortedResponse.Content.ReadAsStringAsync();
            (await ValidateWebhookSignatureHeadersPresent(_recipientDeliveryReportAbortedResponse)).Should().BeTrue();
        }

        [Then(@"the SMS event describes an ""incoming SMS"" event")]
        public void ThenTheSmsEventDescribesAnIncomingSmsEvent()
        {
            var incomingSms = JsonSerializer.Deserialize<TextMessage>(_rawIncomingSmsContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            incomingSms.Should().BeEquivalentTo(new TextMessage
            {
                Body = "Hello John! 👋",
                From = "12015555555",
                Id = "01W4FFL35P4NC4K35SMSBATCH8",
                OperatorId = "311071",
                ReceivedAt = Helpers.ParseUtc("2024-06-06T07:52:37.386Z"),
                To = "12017777777",
                Type = SmsType.Text
            });
        }

        [Then(@"the SMS event describes an ""SMS delivery report"" event")]
        public void ThenTheSmsEventDescribesAnSmsDeliveryReportEvent()
        {
            var deliveryReport = JsonSerializer.Deserialize<BatchDeliveryReportSms>(_rawDeliveryReportContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            deliveryReport.Should().BeEquivalentTo(new BatchDeliveryReportSms
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH8",
                ClientReference = "client-ref",
                TotalMessageCount = 2,
                Type = DeliveryReportType.Sms,
                Statuses = new List<DeliveryReportStatusVerbose>
                {
                    new DeliveryReportStatusVerbose
                    {
                        Code = 0,
                        Count = 2,
                        Recipients = new List<string> { "12017777777", "33612345678" },
                        Status = DeliveryReportStatus.Delivered
                    }
                }
            });
        }

        [Then(@"the SMS event describes an SMS recipient delivery report event with the status ""Delivered""")]
        public void ThenTheSmsEventDescribesAnSmsRecipientDeliveryReportEventWithStatusDelivered()
        {
            var recipientDeliveryReport = JsonSerializer.Deserialize<RecipientDeliveryReportSms>(_rawRecipientDeliveryReportDeliveredContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            recipientDeliveryReport.Should().BeEquivalentTo(new RecipientDeliveryReportSms
            {
                At = Helpers.ParseUtc("2024-06-06T08:17:19.210Z"),
                BatchId = "01W4FFL35P4NC4K35SMSBATCH9",
                ClientReference = "client-ref",
                Code = 0,
                OperatorStatusName = Helpers.ParseUtc("2024-06-06T08:17:00Z"),
                Recipient = "12017777777",
                Status = DeliveryReportStatus.Delivered,
                Type = RecipientDeliveryReportType.Sms
            });
        }

        [Then(@"the SMS event describes an SMS recipient delivery report event with the status ""Aborted""")]
        public void ThenTheSmsEventDescribesAnSmsRecipientDeliveryReportEventWithStatusAborted()
        {
            var recipientDeliveryReport = JsonSerializer.Deserialize<RecipientDeliveryReportSms>(_rawRecipientDeliveryReportAbortedContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            recipientDeliveryReport.Should().BeEquivalentTo(new RecipientDeliveryReportSms
            {
                At = Helpers.ParseUtc("2024-06-06T08:17:15.603Z"),
                BatchId = "01W4FFL35P4NC4K35SMSBATCH9",
                ClientReference = "client-ref",
                Code = 412,
                Recipient = "12010000000",
                Status = DeliveryReportStatus.Aborted,
                Type = RecipientDeliveryReportType.Sms
            });
        }

        /// <summary>
        /// Validate webhook authentication using HMAC signature.
        /// </summary>
        private static async Task<bool> ValidateWebhookSignatureHeadersPresent(HttpResponseMessage response)
        {
            var headers = response.Headers.ToDictionary(
                x => x.Key,
                x => x.Value.FirstOrDefault(),
                StringComparer.OrdinalIgnoreCase);

            var body = await response.Content.ReadAsStringAsync();

            return _webhooks.ValidateAuthenticationHeader(WebhookSecret, headers, body);
        }
    }
}
