using System.Text.Json;
using FluentAssertions;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Hooks;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class HooksTests
    {
        [Fact]
        public void DeserializeDeliveryReport()
        {
            var json = Helpers.LoadResources("Sms/Hooks/DeliveryReportSms.json");
            var report = JsonSerializer.Deserialize<BatchDeliveryReportSms>(json);
            report!.Should().BeEquivalentTo(new BatchDeliveryReportSms
            {
                BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                TotalMessageCount = 1,
                Type = DeliveryReportType.Sms,
                Statuses = new System.Collections.Generic.List<DeliveryReportStatusVerbose>
                {
                    new DeliveryReportStatusVerbose
                    {
                        Code = 0,
                        Count = 1,
                        Status = DeliveryReportStatus.Delivered,
                        Recipients = new System.Collections.Generic.List<string>{ "44231235674" }
                    }
                },
                ClientReference = "a client reference"
            });
        }

        [Fact]
        public void DeserializeRecipientDeliveryReport()
        {
            var json = Helpers.LoadResources("Sms/Hooks/RecipientDeliveryReportSms.json");
            var report = JsonSerializer.Deserialize<RecipientDeliveryReportSms>(json);
            report!.Should().BeEquivalentTo(new RecipientDeliveryReportSms
            {
                BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                Code = 401,
                Recipient = "+44231235674",
                Status = DeliveryReportStatus.Dispatched,
                At = System.DateTime.Parse("2022-08-30T08:16:08.930Z").ToUniversalTime(),
                Type = RecipientDeliveryReportType.Sms,
                Operator = "operator",
                AppliedOriginator = "applied originator",
                ClientReference = "client reference",
                Encoding = Sinch.SMS.Hooks.Encoding.Gsm,
                NumberOfMessageParts = 2,
                OperatorStatusName = System.DateTime.Parse("2022-08-30T08:16:08.150Z").ToUniversalTime()
            });
        }

        [Fact]
        public void DeserializeBinaryMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundBinary.json");
            var report = JsonSerializer.Deserialize<BinaryMessage>(json);
            report!.Should().BeOfType<BinaryMessage>().Which.Should().BeEquivalentTo(new BinaryMessage
            {
                Id = "01XXXXX21XXXXX119Z8P1XXXXX",
                Body = "VGV4dCBtZXNzYWdl",
                From = "16051234567",
                To = "13185551234",
                OperatorId = "operator",
                ClientReference = "ccc",
                ReceivedAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                SentAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                Udh = "10010203040506070809000a0b0c0d0e0f",
                Type = Sinch.SMS.Inbounds.SmsType.Binary
            });
        }

        [Fact]
        public void DeserializeTextMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundText.json");
            var report = JsonSerializer.Deserialize<TextMessage>(json);
            report!.Should().BeOfType<TextMessage>().Which.Should().BeEquivalentTo(new TextMessage
            {
                Id = "01XXXXX21XXXXX119Z8P1XXXXX",
                Body = "This is a test message.",
                From = "16051234567",
                To = "13185551234",
                OperatorId = "string",
                ClientReference = "text-client-ref",
                ReceivedAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                SentAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                Type = Sinch.SMS.Inbounds.SmsType.Text
            });
        }

        [Fact]
        public void DeserializeMediaMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundMedia.json");
            var evt = JsonSerializer.Deserialize<MediaMessage>(json);
            evt!.Should().BeOfType<MediaMessage>().Which.Should().BeEquivalentTo(new MediaMessage
            {
                Id = "01FC66621XXXXX119Z8PMV1QPA",
                From = "+11203494390",
                To = "11203453453",
                OperatorId = "35000",
                ClientReference = "a client reference",
                ReceivedAt = System.DateTime.Parse("2019-08-24T14:17:22Z").ToUniversalTime(),
                SentAt = System.DateTime.Parse("2019-08-24T14:15:22Z").ToUniversalTime(),
                MessageBody = new MediaMessageBody
                {
                    Subject = "mmy subject",
                    Message = "my message",
                    Media = new System.Collections.Generic.List<MediaMessageBodyDetails>
                    {
                        new MediaMessageBodyDetails
                        {
                            Url = "https://foo.url",
                            ContentType = "content/type",
                            Status = MediaStatus.Uploaded,
                            Code = 1234
                        }
                    }
                }
            });
        }

        [Fact]
        public void DeserializeBatchDeliveryReportMms()
        {
            var json = Helpers.LoadResources("Sms/Hooks/BatchDeliveryReportMms.json");
            var report = JsonSerializer.Deserialize<BatchDeliveryReportMms>(json);
            report!.Should().BeEquivalentTo(new BatchDeliveryReportMms
            {
                BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                ClientReference = "a client reference",
                TotalMessageCount = 1,
                Type = "delivery_report_mms",
                Statuses = new System.Collections.Generic.List<DeliveryReportStatus>
                {
                    DeliveryReportStatus.Delivered
                }
            });
        }

        [Fact]
        public void DeserializeDeliveryReportRecipientMms()
        {
            var json = Helpers.LoadResources("Sms/Hooks/RecipientDeliveryReportMms.json");
            var report = JsonSerializer.Deserialize<RecipientDeliveryReportMms>(json);
            report!.Should().BeEquivalentTo(new RecipientDeliveryReportMms
            {
                BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                Code = 401,
                Recipient = "+44231235674",
                Status = "Dispatched",
                At = System.DateTime.Parse("2022-08-30T08:16:08.930Z").ToUniversalTime(),
                Operator = "operator",
                AppliedOriginator = "applied originator",
                ClientReference = "client reference",
                Encoding = "encoding",
                NumberOfMessageParts = 123,
                OperatorStatusAt = System.DateTime.Parse("2022-08-30T08:16:08.150Z").ToUniversalTime(),
                Type = "recipient_delivery_report_mms"
            });
        }
    }
}
