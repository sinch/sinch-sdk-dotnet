using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Hooks;
using Sinch.SMS.Inbounds;
using Xunit;
using DeliveryReport = Sinch.SMS.Hooks.DeliveryReport;

namespace Sinch.Tests.Sms
{
    public class HooksTests
    {
        [Fact]
        public void DeserializeDeliveryReport()
        {
            var json = @"{
                            ""batch_id"": ""01FC66621XXXXX119Z8PMV1QPQ"",
                            ""statuses"": [
                                {
                                    ""code"": 0,
                                    ""count"": 1,
                                    ""recipients"": [
                                        ""44231235674""
                                    ],
                                    ""status"": ""Delivered""
                                }
                            ],
                            ""total_message_count"": 1,
                            ""type"": ""delivery_report_sms""
                        }";
            var report = JsonSerializer.Deserialize<DeliveryReport>(json);
            report!.BatchId.Should().Be("01FC66621XXXXX119Z8PMV1QPQ");
            report.Type.Should().Be(DeliveryReportType.Sms);
            report.Statuses.First().Status.Should().Be(DeliveryReportStatus.Delivered);
        }

        [Fact]
        public void DeserializeRecipientDeliveryReport()
        {
            var json = @"{
                            ""type"": ""recipient_delivery_report_sms"",
                            ""batch_id"": ""01FC66621XXXXX119Z8PMV1QPQ"",
                            ""recipient"": ""+44231235674"",
                            ""code"": 401,
                            ""status"": ""Dispatched"",
                            ""at"": ""2022-08-30T08:16:08.930Z""
                        }";
            var report = JsonSerializer.Deserialize<RecipientDeliveryReport>(json);
            report!.Code.Should().Be(401);
            report.Status.Should().Be(DeliveryReportStatus.Dispatched);
        }

        [Fact]
        public void DeserializeIncomingBinarySms()
        {
            string json = @"{
                            ""body"": ""VGV4dCBtZXNzYWdl"",
                            ""from"": ""16051234567"",
                            ""id"": ""01XXXXX21XXXXX119Z8P1XXXXX"",
                            ""operator_id"": ""operator"",
                            ""received_at"": ""2022-08-24T14:15:22Z"",
                            ""sent_at"": ""2022-08-24T14:15:22Z"",
                            ""to"": ""13185551234"",
                            ""type"": ""mo_binary"",
                            ""udh"": ""10010203040506070809000a0b0c0d0e0f""
                        }";
            var report = JsonSerializer.Deserialize<IncomingBinarySms>(json);
            report!.Type.Should().Be(SmsType.Binary);
        }

        [Fact]
        public void DeserializeIncomingTextSms()
        {
            string json = @"{
                                ""body"": ""This is a test message."",
                                ""from"": ""16051234567"",
                                ""id"": ""01XXXXX21XXXXX119Z8P1XXXXX"",
                                ""operator_id"": ""string"",
                                ""received_at"": ""2022-08-24T14:15:22Z"",
                                ""sent_at"": ""2022-08-24T14:15:22Z"",
                                ""to"": ""13185551234"",
                                ""type"": ""mo_text""
                            }";
            var report = JsonSerializer.Deserialize<IncomingTextSms>(json);
            report!.Type.Should().Be(SmsType.Text);
        }
    }
}
