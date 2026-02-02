using System;
using System.Text.Json;
using FluentAssertions;
using Sinch.SMS;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Inbounds;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class WebhooksTests : SmsTestBase
    {
        [Fact]
        public void DeserializeDeliveryReport()
        {
            var json = Helpers.LoadResources("Sms/Hooks/DeliveryReportSms.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<BatchDeliveryReportSms>();
            AssertDeliveryReport(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<BatchDeliveryReportSms>();
            AssertDeliveryReport(deserialized);

            void AssertDeliveryReport(BatchDeliveryReportSms report)
            {
                report!.Should().BeEquivalentTo(new BatchDeliveryReportSms
                {
                    BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                    TotalMessageCount = 1,
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
        }

        [Fact]
        public void DeserializeRecipientDeliveryReport()
        {
            var json = Helpers.LoadResources("Sms/Hooks/RecipientDeliveryReportSms.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<RecipientDeliveryReportSms>();
            AssertRecipient(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<RecipientDeliveryReportSms>();
            AssertRecipient(deserialized);

            void AssertRecipient(RecipientDeliveryReportSms report)
            {
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
                    Encoding = Sinch.SMS.DeliveryReports.Encoding.Gsm,
                    NumberOfMessageParts = 2,
                    OperatorStatusAt = System.DateTime.Parse("2022-08-30T08:16:08.150Z").ToUniversalTime()
                });
            }
        }

        [Fact]
        public void DeserializeBinaryMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundBinary.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<BinaryInbound>();
            AssertBinary(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<BinaryInbound>();
            AssertBinary(deserialized);

            void AssertBinary(BinaryInbound report)
            {
                report!.Should().BeOfType<BinaryInbound>().Which.Should().BeEquivalentTo(new BinaryInbound
                {
                    Id = "01XXXXX21XXXXX119Z8P1XXXXX",
                    Body = "VGV4dCBtZXNzYWdl",
                    From = "16051234567",
                    To = "13185551234",
                    OperatorId = "operator",
                    ClientReference = "ccc",
                    ReceivedAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                    SentAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                    Udh = "10010203040506070809000a0b0c0d0e0f"
                });
            }
        }

        [Fact]
        public void DeserializeTextMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundText.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<SmsInbound>();
            AssertText(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<SmsInbound>();
            AssertText(deserialized);

            void AssertText(SmsInbound report)
            {
                report!.Should().BeOfType<SmsInbound>().Which.Should().BeEquivalentTo(new SmsInbound
                {
                    Id = "01XXXXX21XXXXX119Z8P1XXXXX",
                    Body = "This is a test message.",
                    From = "16051234567",
                    To = "13185551234",
                    OperatorId = "string",
                    ClientReference = "text-client-ref",
                    ReceivedAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime(),
                    SentAt = System.DateTime.Parse("2022-08-24T14:15:22Z").ToUniversalTime()
                });
            }
        }

        [Fact]
        public void DeserializeMediaMessage()
        {
            var json = Helpers.LoadResources("Sms/Hooks/InboundMedia.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<MediaInbound>();
            AssertMedia(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<MediaInbound>();
            AssertMedia(deserialized);

            void AssertMedia(MediaInbound evt)
            {
                evt!.Should().BeOfType<MediaInbound>().Which.Should().BeEquivalentTo(new MediaInbound
                {
                    Id = "01FC66621XXXXX119Z8PMV1QPA",
                    From = "+11203494390",
                    To = "11203453453",
                    OperatorId = "35000",
                    ClientReference = "a client reference",
                    ReceivedAt = System.DateTime.Parse("2019-08-24T14:17:22Z").ToUniversalTime(),
                    SentAt = System.DateTime.Parse("2019-08-24T14:15:22Z").ToUniversalTime(),
                    Body = new MmsMoBody()
                    {
                        Subject = "mmy subject",
                        Message = "my message",
                        Media = new System.Collections.Generic.List<MmsMedia>
                        {
                            new MmsMedia
                            {
                                Url = "https://foo.url",
                                ContentType = "content/type",
                                Status = MmsMedia.StatusEnum.Uploaded,
                                Code = 1234
                            }
                        }
                    }
                });
            }
        }

        [Fact]
        public void DeserializeBatchDeliveryReportMms()
        {
            var json = Helpers.LoadResources("Sms/Hooks/BatchDeliveryReportMms.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<BatchDeliveryReportMms>();
            AssertBatch(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<BatchDeliveryReportMms>();
            AssertBatch(deserialized);

            void AssertBatch(BatchDeliveryReportMms report)
            {
                report!.Should().BeEquivalentTo(new BatchDeliveryReportMms
                {
                    BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                    ClientReference = "a client reference",
                    TotalMessageCount = 1,
                    Statuses = new System.Collections.Generic.List<DeliveryReportStatus>
                    {
                        DeliveryReportStatus.Delivered
                    }
                });
            }
        }

        [Fact]
        public void DeserializeDeliveryReportRecipientMms()
        {
            var json = Helpers.LoadResources("Sms/Hooks/RecipientDeliveryReportMms.json");

            var parsed = Sms.Webhooks.ParseEvent(json).As<RecipientDeliveryReportMms>();
            AssertRecipient(parsed);

            var deserialized = JsonSerializer.Deserialize<ISmsEvent>(json, Sms.Webhooks.JsonSerializerOptions).As<RecipientDeliveryReportMms>();
            AssertRecipient(deserialized);

            void AssertRecipient(RecipientDeliveryReportMms report)
            {
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
                    Type = RecipientDeliveryReportType.Mms
                });
            }
        }

        [Fact]
        public void ParseEvent_ThrowsOnRandomObject()
        {
            // Arrange: an object with no "type" field
            var payload = "{\"unknownProperty\":\"anyValue\"}";

            // Act
            Action act = () => Sms.Webhooks.ParseEvent(payload);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*Deserialization of SMS webhook event failed*");
        }

        [Fact]
        public void ParseEvent_ThrowsOnUnknownType()
        {
            // Arrange: an object with an unknown type
            var payload = "{\"type\":\"unknown\"}";

            // Act
            Action act = () => Sms.Webhooks.ParseEvent(payload);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*Deserialization of SMS webhook event failed*");
        }
    }
}
