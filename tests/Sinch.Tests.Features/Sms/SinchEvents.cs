using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Inbounds;
using Sinch.SMS.SinchEvents;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class SinchEvents
    {
        private const string SinchEventsSecret = "KayakingTheSwell";
        private const string SinchEventsUrlPrefix = Helpers.MOCKSERVER_SMS_URL + "webhooks/sms";

        private readonly HttpClient _httpClient = new();
        private static ISmsSinchEvents _smsSinchEvents;
        private HttpResponseMessage _incomingSmsResponse;
        private HttpResponseMessage _deliveryReportResponse;
        private HttpResponseMessage _recipientDeliveryReportDeliveredResponse;
        private HttpResponseMessage _recipientDeliveryReportAbortedResponse;
        private string _rawBody;

        [Given(@"the SMS Webhooks handler is available")]
        public void GivenTheSmsSinchEventsHandlerIsAvailable()
        {
            _smsSinchEvents = new SinchClient().Sms.SinchEvents;
        }

        [When(@"I send a request to trigger an ""incoming SMS"" event")]
        public async Task WhenISendARequestToTriggerAnIncomingSmsEvent()
        {
            _incomingSmsResponse = await _httpClient.GetAsync($"{SinchEventsUrlPrefix}/incoming-sms");
        }

        [When(@"I send a request to trigger an ""SMS delivery report"" event")]
        public async Task WhenISendARequestToTriggerAnSmsDeliveryReportEvent()
        {
            _deliveryReportResponse = await _httpClient.GetAsync($"{SinchEventsUrlPrefix}/delivery-report-sms");
        }

        [When(@"I send a request to trigger an ""SMS recipient delivery report"" event with the status ""Delivered""")]
        public async Task WhenISendARequestToTriggerAnSmsRecipientDeliveryReportEventWithStatusDelivered()
        {
            _recipientDeliveryReportDeliveredResponse = await _httpClient.GetAsync($"{SinchEventsUrlPrefix}/recipient-delivery-report-sms-delivered");
        }

        [When(@"I send a request to trigger an ""SMS recipient delivery report"" event with the status ""Aborted""")]
        public async Task WhenISendARequestToTriggerAnSmsRecipientDeliveryReportEventWithStatusAborted()
        {
            _recipientDeliveryReportAbortedResponse = await _httpClient.GetAsync($"{SinchEventsUrlPrefix}/recipient-delivery-report-sms-aborted");
        }

        [Then(@"the header of the event ""IncomingSMS"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventIncomingSmsContainsAValidSignature()
        {
            _rawBody = await _incomingSmsResponse.Content.ReadAsStringAsync();
            _smsSinchEvents.ValidateAuthenticationHeader(SinchEventsSecret, _incomingSmsResponse.GetAllHeaders(), _rawBody).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportContainsAValidSignature()
        {
            _rawBody = await _deliveryReportResponse.Content.ReadAsStringAsync();
            _smsSinchEvents.ValidateAuthenticationHeader(SinchEventsSecret, _deliveryReportResponse.GetAllHeaders(), _rawBody).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" with the status ""Delivered"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportWithStatusDeliveredContainsAValidSignature()
        {
            _rawBody = await _recipientDeliveryReportDeliveredResponse.Content.ReadAsStringAsync();
            _smsSinchEvents.ValidateAuthenticationHeader(SinchEventsSecret, _recipientDeliveryReportDeliveredResponse.GetAllHeaders(), _rawBody).Should().BeTrue();
        }

        [Then(@"the header of the event ""DeliveryReport"" with the status ""Aborted"" contains a valid signature")]
        public async Task ThenTheHeaderOfTheEventDeliveryReportWithStatusAbortedContainsAValidSignature()
        {
            _rawBody = await _recipientDeliveryReportAbortedResponse.Content.ReadAsStringAsync();
            _smsSinchEvents.ValidateAuthenticationHeader(SinchEventsSecret, _recipientDeliveryReportAbortedResponse.GetAllHeaders(), _rawBody).Should().BeTrue();
        }

        [Then(@"the SMS event describes an ""incoming SMS"" event")]
        public void ThenTheSmsEventDescribesAnIncomingSmsEvent()
        {
            var smsEvent = _smsSinchEvents.ParseEvent(_rawBody);
            smsEvent.As<SmsInbound>().Should().BeEquivalentTo(new SmsInbound
            {
                Body = "Hello John! 👋",
                From = "12015555555",
                Id = "01W4FFL35P4NC4K35SMSBATCH8",
                OperatorId = "311071",
                ReceivedAt = Helpers.ParseUtc("2024-06-06T07:52:37.386Z"),
                To = "12017777777"
            });
        }

        [Then(@"the SMS event describes an ""SMS delivery report"" event")]
        public void ThenTheSmsEventDescribesAnSmsDeliveryReportEvent()
        {
            var smsEvent = _smsSinchEvents.ParseEvent(_rawBody);
            smsEvent.As<BatchDeliveryReportSms>().Should().BeEquivalentTo(new BatchDeliveryReportSms
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH8",
                ClientReference = "client-ref",
                TotalMessageCount = 2,
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
            var smsEvent = _smsSinchEvents.ParseEvent(_rawBody);
            smsEvent.As<RecipientDeliveryReportSms>().Should().BeEquivalentTo(new RecipientDeliveryReportSms
            {
                At = Helpers.ParseUtc("2024-06-06T08:17:19.210Z"),
                BatchId = "01W4FFL35P4NC4K35SMSBATCH9",
                ClientReference = "client-ref",
                Code = 0,
                OperatorStatusAt = Helpers.ParseUtc("2024-06-06T08:17:00Z"),
                Recipient = "12017777777",
                Status = DeliveryReportStatus.Delivered
            });
        }

        [Then(@"the SMS event describes an SMS recipient delivery report event with the status ""Aborted""")]
        public void ThenTheSmsEventDescribesAnSmsRecipientDeliveryReportEventWithStatusAborted()
        {
            var smsEvent = _smsSinchEvents.ParseEvent(_rawBody);
            smsEvent.As<RecipientDeliveryReportSms>().Should().BeEquivalentTo(new RecipientDeliveryReportSms
            {
                At = Helpers.ParseUtc("2024-06-06T08:17:15.603Z"),
                BatchId = "01W4FFL35P4NC4K35SMSBATCH9",
                ClientReference = "client-ref",
                Code = DeliveryReceiptStatusCode.UnprovisionedRegion,
                Recipient = "12010000000",
                Status = DeliveryReportStatus.Aborted
            });
        }

    }
}
