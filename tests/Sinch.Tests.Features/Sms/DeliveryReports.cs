using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.DeliveryReports.Get;
using Sinch.SMS.DeliveryReports.List;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class DeliveryReports
    {
        private ISinchSmsDeliveryReports _deliveryReports;
        private GetDeliveryReportResponse _deliveryReport;
        private DeliveryReport _recipientDeliveryReport;
        private ListDeliveryReportsResponse _deliveryReportsList;
        private List<DeliveryReport> _deliveryReportsListAll;
        private int _totalPages;

        [Given(@"the SMS service ""Delivery Reports"" is available")]
        public void GivenTheSmsServiceIsAvailable()
        {
            _deliveryReports = Utils.SinchClient.Sms.DeliveryReports;
        }

        [When(@"I send a request to retrieve a summary SMS delivery report")]
        public async Task WhenISendARequestToRetrieveASummarySmsDeliveryReport()
        {
            _deliveryReport = await _deliveryReports.Get(new GetDeliveryReportRequest
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH1",
                Statuses = [DeliveryReportStatus.Delivered, DeliveryReportStatus.Failed,],
                Code = ["15", "0"]
            });
        }

        [Then(@"the response contains a summary SMS delivery report")]
        public void ThenTheResponseContainsASummarySmsDeliveryReport()
        {
            _deliveryReport.Should().BeEquivalentTo(new GetDeliveryReportResponse
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH1",
                ClientReference = "reference_e2e",
                TotalMessageCount = 2,
                Type = DeliveryReportType.Sms,
                Statuses = new[]
                {
                    new DeliveryReportStatusVerbose
                    {
                        Code = 15,
                        Count = 1,
                        Recipients = null,
                        Status = DeliveryReportStatus.Failed
                    },
                    new DeliveryReportStatusVerbose
                    {
                        Code = 0,
                        Count = 1,
                        Recipients = null,
                        Status = DeliveryReportStatus.Delivered
                    }
                }
            });
        }

        [When(@"I send a request to retrieve a full SMS delivery report")]
        public async Task WhenISendARequestToRetrieveAFullSmsDeliveryReport()
        {
            _deliveryReport = await _deliveryReports.Get(new GetDeliveryReportRequest()
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH1",
                DeliveryReportType = DeliveryReportVerbosityType.Full
            });
        }

        [Then(@"the response contains a full SMS delivery report")]
        public void ThenTheResponseContainsAFullSmsDeliveryReport()
        {
            _deliveryReport.Should().BeEquivalentTo(new GetDeliveryReportResponse
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH1",
                Type = DeliveryReportType.Sms,
                TotalMessageCount = 2,
                ClientReference = "reference_e2e",
                Statuses = new[]
                {
                    new DeliveryReportStatusVerbose
                    {
                        Code = 0,
                        Count = 1,
                        Recipients = ["12017777777"],
                        Status = DeliveryReportStatus.Delivered
                    },
                    new DeliveryReportStatusVerbose
                    {
                        Code = 15,
                        Count = 1,
                        Recipients = ["12018888888"],
                        Status = DeliveryReportStatus.Failed
                    }
                }
            });
        }

        [When(@"I send a request to retrieve a recipient's delivery report")]
        public async Task WhenISendARequestToRetrieveARecipientsDeliveryReport()
        {
            _recipientDeliveryReport = await _deliveryReports.GetForNumber("01W4FFL35P4NC4K35SMSBATCH1", "12017777777");
        }

        [Then(@"the response contains the recipient's delivery report details")]
        public void ThenTheResponseContainsTheRecipientsDeliveryReportDetails()
        {
            _recipientDeliveryReport.Should().BeEquivalentTo(new DeliveryReport
            {
                BatchId = "01W4FFL35P4NC4K35SMSBATCH1",
                ClientReference = "reference_e2e",
                Status = DeliveryReportStatus.Delivered,
                Type = RecipientDeliveryReportType.Sms,
                Code = 0,
                At = Helpers.ParseUtc("2024-06-06T13:06:27.833Z"),
                OperatorStatusAt = Helpers.ParseUtc("2024-06-06T13:06:00Z")
            });
        }

        [When(@"I send a request to list the SMS delivery reports")]
        public async Task WhenISendARequestToListTheSmsDeliveryReports()
        {
            _deliveryReportsList = await _deliveryReports.List(new ListDeliveryReportsRequest());
        }

        [Then(@"the response contains ""(.*)"" SMS delivery reports")]
        public void ThenTheResponseContainsSmsDeliveryReports(int count)
        {
            _deliveryReportsList.DeliveryReports.Should().HaveCount(count);
        }

        [When(@"I send a request to list all the SMS delivery reports")]
        public async Task WhenISendARequestToListAllTheSmsDeliveryReports()
        {
            _deliveryReportsListAll = new List<DeliveryReport>();
            await foreach (var report in _deliveryReports.ListAuto(new ListDeliveryReportsRequest()
            {
                PageSize = 2
            }))
            {
                _deliveryReportsListAll.Add(report);
            }
        }

        [Then(@"the SMS delivery reports list contains ""(.*)"" SMS delivery reports")]
        public void ThenTheSmsDeliveryReportsListContainsSmsDeliveryReports(int count)
        {
            _deliveryReportsListAll.Should().HaveCount(count);
        }

        [When(@"I iterate manually over the SMS delivery reports pages")]
        public async Task WhenIIterateManuallyOverTheSmsDeliveryReportsPages()
        {
            // TODO(DEVEXP-992): implement iterator
            _deliveryReportsListAll = new List<DeliveryReport>();
            _totalPages = 0;
            ListDeliveryReportsResponse listResponse = null;
            while (true)
            {
                listResponse = await _deliveryReports.List(new ListDeliveryReportsRequest()
                {
                    PageSize = 2,
                    Page = listResponse?.Page + 1
                });
                if (!(listResponse?.DeliveryReports!).Any()) break;
                _deliveryReportsListAll.AddRange(listResponse.DeliveryReports);
                _totalPages++;
            }
        }

        [Then(@"the SMS delivery reports iteration result contains the data from ""(.*)"" pages")]
        public void ThenTheSmsDeliveryReportsIterationResultContainsTheDataFromPages(int totalPages)
        {
            totalPages.Should().Be(_totalPages);
        }
    }
}
