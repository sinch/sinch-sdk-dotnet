using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.DeliveryReports.Get;
using Xunit;

namespace Sinch.Tests.e2e.Sms
{
    public class DeliveryReportsTests : TestBase
    {
        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockStudio.Sms.DeliveryReports.Get(new GetDeliveryReportRequest
            {
                BatchId = "01GRY6XP44CBK211XY3HFG09KR",
                DeliveryReportType = DeliveryReportVerbosityType.Summary,
                Statuses = new List<DeliveryReportStatus>()
                {
                    DeliveryReportStatus.Queued,
                    DeliveryReportStatus.Dispatched,
                    DeliveryReportStatus.Delivered
                },
                Code = new List<string>()
                {
                    "400",
                    "405"
                }
            });
            response.Type.Should().Be(DeliveryReportType.Sms);
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockStudio.Sms.DeliveryReports.List(new SMS.DeliveryReports.List.ListDeliveryReportsRequest()
            {

                Page = 0,
            });
            response.DeliveryReports.Count().Should().Be(5);
        }

        [Fact]
        public async Task ForNumber()
        {
            var response =
                await SinchClientMockStudio.Sms.DeliveryReports.GetForNumber("01GJJWEXCVD0CT1VVTMRE20C31", "48737532793");
            response.Type.Should().Be(RecipientDeliveryReportType.Sms);
        }
    }
}
