using System;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Inbounds;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class SinchEventsTests : SmsTestBase
    {
        private const string HmacSecret = "my_secret_key";
        private const string SignedPayload = "{\"event\":\"test\"}";
        private const string Timestamp = "1736760161";
        private const string Nonce = "01JHFFHWYY7HSS4FWTMDTQEK8V";
        private const string Algorithm = "HmacSHA256";

        [Fact]
        public void DeserializeDeliveryReport()
        {
            var json = Helpers.LoadResources("Sms/SinchEvents/DeliveryReportSms.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<BatchDeliveryReportSms>();
            AssertDeliveryReport(parsed);

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
            var json = Helpers.LoadResources("Sms/SinchEvents/RecipientDeliveryReportSms.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<RecipientDeliveryReportSms>();
            AssertRecipient(parsed);

            void AssertRecipient(RecipientDeliveryReportSms report)
            {
                report!.Should().BeEquivalentTo(new RecipientDeliveryReportSms
                {
                    BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                    Code = DeliveryReceiptStatusCode.Dispatched,
                    Recipient = "+44231235674",
                    Status = DeliveryReportStatus.Dispatched,
                    At = System.DateTime.Parse("2022-08-30T08:16:08.930Z").ToUniversalTime(),
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
            var json = Helpers.LoadResources("Sms/SinchEvents/InboundBinary.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<BinaryInbound>();
            AssertBinary(parsed);

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
            var json = Helpers.LoadResources("Sms/SinchEvents/InboundText.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<SmsInbound>();
            AssertText(parsed);

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
            var json = Helpers.LoadResources("Sms/SinchEvents/InboundMedia.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<MediaInbound>();
            AssertMedia(parsed);

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
            var json = Helpers.LoadResources("Sms/SinchEvents/BatchDeliveryReportMms.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<BatchDeliveryReportMms>();
            AssertBatch(parsed);

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
            var json = Helpers.LoadResources("Sms/SinchEvents/RecipientDeliveryReportMms.json");

            var parsed = Sms.SinchEvents.ParseEvent(json).As<RecipientDeliveryReportMms>();
            AssertRecipient(parsed);

            void AssertRecipient(RecipientDeliveryReportMms report)
            {
                report!.Should().BeEquivalentTo(new RecipientDeliveryReportMms
                {
                    BatchId = "01FC66621XXXXX119Z8PMV1QPQ",
                    Code = DeliveryReceiptStatusCode.Dispatched,
                    Recipient = "+44231235674",
                    Status = "Dispatched",
                    At = System.DateTime.Parse("2022-08-30T08:16:08.930Z").ToUniversalTime(),
                    Operator = "operator",
                    AppliedOriginator = "applied originator",
                    ClientReference = "client reference",
                    Encoding = Encoding.Unicode,
                    NumberOfMessageParts = 123,
                    OperatorStatusAt = System.DateTime.Parse("2022-08-30T08:16:08.150Z").ToUniversalTime()
                });
            }
        }

        [Fact]
        public void ParseEvent_ThrowsOnRandomObject()
        {
            // Arrange: an object with no "type" field
            var payload = "{\"unknownProperty\":\"anyValue\"}";

            // Act
            Action act = () => Sms.SinchEvents.ParseEvent(payload);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*Deserialization of SMS Sinch Event failed*");
        }

        [Fact]
        public void ParseEvent_ThrowsOnUnknownType()
        {
            // Arrange: an object with an unknown type
            var payload = "{\"type\":\"unknown\"}";

            // Act
            Action act = () => Sms.SinchEvents.ParseEvent(payload);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*Deserialization of SMS Sinch Event failed*");
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsTrue_WhenHeaderKeyIsUpperCase()
        {
            var headers = CreateAuthenticationHeaders(key => key.ToUpperInvariant());

            var result = Sms.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, SignedPayload);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsTrue_WhenHeaderKeyIsMixedCase()
        {
            var headers = CreateAuthenticationHeaders(ToMixedCaseHeaderKey);

            var result = Sms.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, SignedPayload);

            result.Should().BeTrue();
        }

        private static Dictionary<string, IEnumerable<string>> CreateAuthenticationHeaders(Func<string, string> transformHeaderKey)
        {
            var signature = CreateSignature();

            return new Dictionary<string, IEnumerable<string>>
            {
                [transformHeaderKey("x-sinch-webhook-signature-timestamp")] = new[] { Timestamp },
                [transformHeaderKey("x-sinch-webhook-signature-nonce")] = new[] { Nonce },
                [transformHeaderKey("x-sinch-webhook-signature-algorithm")] = new[] { Algorithm },
                [transformHeaderKey("x-sinch-webhook-signature")] = new[] { signature }
            };
        }

        private static string CreateSignature()
        {
            var toBeSigned = $"{SignedPayload}.{Nonce}.{Timestamp}";
            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(HmacSecret));
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toBeSigned));
            return Convert.ToBase64String(hash);
        }

        private static string ToMixedCaseHeaderKey(string headerKey)
        {
            return headerKey switch
            {
                "x-sinch-webhook-signature-timestamp" => "X-Sinch-Webhook-Signature-Timestamp",
                "x-sinch-webhook-signature-nonce" => "X-Sinch-Webhook-Signature-Nonce",
                "x-sinch-webhook-signature-algorithm" => "X-Sinch-Webhook-Signature-Algorithm",
                "x-sinch-webhook-signature" => "X-Sinch-Webhook-Signature",
                _ => headerKey
            };
        }
    }
}
