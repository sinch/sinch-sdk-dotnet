using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.DeliveryReports.Get;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class DeliveryReportsTests : SmsTestBase
    {
        [Fact]
        public void DeliveryReportStatusToString()
        {
            var enumStr = DeliveryReportStatus.Aborted;

            enumStr.Value.Should().Be("Aborted");
        }

        [Fact]
        public void DeliveryReportStatusFromString()
        {
            var @enum = new DeliveryReportStatus("Delivered");

            @enum.Should().Be(DeliveryReportStatus.Delivered);
        }

        [Fact]
        public async Task Get()
        {
            var batchId = "123";
            var uri =
                $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/{batchId}/delivery_report?type=full&status=Aborted%2CDelivered&code=312%2C420";
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    type = "delivery_report_sms",
                    batch_id = "123",
                    total_message_count = "15231",
                    client_reference = "jojo",
                    statuses = new[]
                    {
                        new
                        {
                            code = 14,
                            status = "Expired",
                            count = 4,
                            recipients = (string[])null
                        },
                        new
                        {
                            code = 14,
                            status = "Rejected",
                            count = 4,
                            recipients = new[] { "1", "2" }
                        }
                    }
                }));

            var response = await Sms.DeliveryReports.Get(new GetDeliveryReportRequest
            {
                BatchId = batchId,
                DeliveryReportType = DeliveryReportVerbosityType.Full,
                Statuses = new List<DeliveryReportStatus>
                {
                    DeliveryReportStatus.Aborted,
                    DeliveryReportStatus.Delivered
                },
                Code = new List<string> { "312,420" }
            });

            response.Should().NotBeNull();
            response.Type.Should().Be(DeliveryReportType.Sms);
            response.BatchId.Should().Be("123");
            response.TotalMessageCount.Should().Be(15231);
            response.Statuses.Should().HaveCount(2);
            response.Statuses.ElementAt(0).Recipients.Should().BeNull();
            response.Statuses.ElementAt(0).Status.Should().Be(DeliveryReportStatus.Expired);
            response.Statuses.ElementAt(1).Recipients.Should().HaveCount(2);
        }


        [Fact]
        public async Task GetForNumber()
        {
            var batchId = "123";
            var uri =
                $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/{batchId}/delivery_report/+123456789";
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    at = "2022-08-30T08:16:08.930Z",
                    batch_id = batchId,
                    code = 15,
                    recipient = "+123456789",
                    status = "Failed"
                }));

            var response = await Sms.DeliveryReports.GetForNumber(batchId, "+123456789");

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task List()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/delivery_reports";
            uri += "?page=1";
            uri += "&page_size=30";
            uri += "&start_date=2023-08-16T03%3A14%3A17.0000000Z";
            uri += "&end_date=2023-08-16T03%3A14%3A17.0000000Z";
            uri += "&status=Delivered%2CDispatched";
            uri += "&code=400%2C405";
            uri += "&client_reference=xyz";
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    count = 2,
                    page = 3,
                    page_size = 4,
                    delivery_reports = new[]
                    {
                        new
                        {
                            at = "2022-08-30T08:16:08.930Z",
                            batch_id = "123",
                            code = 15,
                            recipient = "+123456789",
                            status = "Failed"
                        }
                    }
                }));

            var response = await Sms.DeliveryReports.List(new SMS.DeliveryReports.List.ListDeliveryReportsRequest
            {
                Page = 1,
                PageSize = 30,
                StartDate = new DateTime(2023, 8, 16, 3, 14, 17, DateTimeKind.Utc),
                EndDate = new DateTime(2023, 8, 16, 3, 14, 17, DateTimeKind.Utc),
                Status = new List<DeliveryReportStatus>
                {
                    DeliveryReportStatus.Delivered, DeliveryReportStatus.Dispatched
                },
                Code = new List<string> { "400", "405" },
                ClientReference = "xyz"
            });

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task ListAuto()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/delivery_reports";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    uri)
                .WithQueryString("page", "0")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    count = 3,
                    page = 0,
                    page_size = 1,
                    delivery_reports = new[]
                    {
                        new
                        {
                            at = "2022-08-30T08:16:08.930Z",
                            batch_id = "123",
                            code = 15,
                            recipient = "+123456789",
                            status = "Failed"
                        }
                    }
                }));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    uri)
                .WithQueryString("page", "1")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    count = 3,
                    page = 1,
                    page_size = 1,
                    delivery_reports = new[]
                    {
                        new
                        {
                            at = "2022-08-30T08:16:08.930Z",
                            batch_id = "123",
                            code = 15,
                            recipient = "+123456789",
                            status = "Failed"
                        }
                    }
                }));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    uri)
                .WithQueryString("page", "2")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    count = 3,
                    page = 2,
                    page_size = 1,
                    delivery_reports = new[]
                    {
                        new
                        {
                            at = "2022-08-30T08:16:08.930Z",
                            batch_id = "123",
                            code = 15,
                            recipient = "+123456789",
                            status = "Failed"
                        }
                    }
                }));

            var response = Sms.DeliveryReports.ListAuto(new SMS.DeliveryReports.List.ListDeliveryReportsRequest
            {
                Page = 0,
            });
            await foreach (var report in response)
            {
                report.Should().NotBeNull();
            }

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }
    }
}
