using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;

namespace Sinch.Tests.Features.Verification
{
    [Binding]
    public class Report
    {
        private ISinchVerification _sinchVerifications;
        private ReportSmsVerificationResponse _smsReport;
        private ReportCalloutVerificationResponse _phoneCallReport;
        private ReportFlashCallVerificationResponse _flashCallReport;
        private ReportWhatsAppVerificationResponse _whatsAppReport;

        [Given(@"the Verification service ""Report"" is available")]
        public void GivenTheVerificationServiceIsAvailable()
        {
            _sinchVerifications = Utils.SinchVerificationClient.Verification;
        }


        [When("I send a request to report an SMS verification by {string} with the verification ID {string}")]
        public async Task WhenISendARequestToReportAnSmsVerificationWithTheVerificationId(string byType, string id)
        {
            _smsReport = await _sinchVerifications.ReportSmsById("1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "OQP1"
                    }
                });
        }

        [Then("the response by {string} contains the details of an SMS verification report")]
        public void ThenTheResponseContainsTheDetailsOfAnSmsVerificationReport(string byType)
        {
            _smsReport.Should().BeEquivalentTo(new ReportSmsVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Status = VerificationStatus.Successful,
            });
        }

        [When("I send a request to report an SMS verification by {string} with the phone number {string}")]
        public async Task WhenISendARequestToReportAnSmsVerificationWithThePhoneNumber(string byType, string phoneNumber)
        {
            _smsReport = await _sinchVerifications.ReportSmsByIdentity(phoneNumber,
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "OQP1"
                    }
                });
        }

        [When("I send a request to report a Phone Call verification by {string} with the verification ID {string}")]
        public async Task WhenISendARequestToReportAPhoneCallVerificationWithTheVerificationId(string byType, string id)
        {
            _phoneCallReport = await _sinchVerifications.ReportCalloutById(id,
                new ReportCalloutVerificationRequest()
                {
                    Callout = new Callout()
                    {
                        Code = "123456"
                    }
                });
        }

        [Then("the response by {string} contains the details of a Phone Call verification report")]
        public void ThenTheResponseContainsTheDetailsOfAPhoneCallVerificationReport(string byType)
        {
            _phoneCallReport.Should().BeEquivalentTo(new ReportCalloutVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                CallComplete = true,
            });
        }

        [When("I send a request to report a Phone Call verification by {string} with the phone number {string}")]
        public async Task WhenISendARequestToReportAPhoneCallVerificationWithThePhoneNumber(string byType, string phoneNumber)
        {
            _phoneCallReport = await _sinchVerifications.ReportCalloutByIdentity(phoneNumber,
                new ReportCalloutVerificationRequest()
                {
                    Callout = new Callout()
                    {
                        Code = "123456"
                    }
                });
        }

        [When("I send a request to report a Flash Call verification by {string} with the verification ID {string}")]
        public async Task WhenISendARequestToReportAFlashCallVerificationWithTheVerificationId(string byType, string id)
        {
            _flashCallReport = await _sinchVerifications.ReportFlashCallById(id,
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "+18156540001"
                    }
                });
        }

        [Then("the response by {string} contains the details of a Flash Call verification report")]
        public void ThenTheResponseContainsTheDetailsOfAFlashCallVerificationReport(string byType)
        {
            _flashCallReport.Should().BeEquivalentTo(new ReportFlashCallVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                Reference = "flashcall-verification-test-e2e",
                CallComplete = true,
            });
        }

        [When("I send a request to report a Flash Call verification by {string} with the phone number {string}")]
        public async Task WhenISendARequestToReportAFlashCallVerificationWithThePhoneNumber(string identity, string phoneNumber)
        {
            _flashCallReport = await _sinchVerifications.ReportFlashCallByIdentity(phoneNumber,
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "+18156540001"
                    }
                });
        }

        [Then(@"the response by ""identity"" contains the details of a failed Flash Call verification report")]
        public void ThenTheResponseContainsTheDetailsOfAFailedFlashCallVerificationReport()
        {
            _flashCallReport.Should().BeEquivalentTo(new ReportFlashCallVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                Status = VerificationStatus.Fail,
                Reason = Reason.Expired,
                CallComplete = true,
                Reference = "verification-tests-e2e"
            });
        }

        [When("I send a request to report a WhatsApp verification by {string} with the verification ID {string}")]
        public async Task WhenISendARequestToReportAWhatsAppVerificationWithTheVerificationId(string byType, string id)
        {
            _whatsAppReport = await _sinchVerifications.ReportWhatsAppById(id,
                new ReportWhatsAppVerificationRequest()
                {
                    WhatsApp = new WhatsApp()
                    {
                        Code = "1234"
                    }
                });
        }

        [Then("the response by {string} contains the details of a WhatsApp verification report")]
        public void ThenTheResponseContainsTheDetailsOfAWhatsAppVerificationReport(string byType)
        {
            _whatsAppReport.Should().BeEquivalentTo(new ReportWhatsAppVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d33d-f00dfeed1337",
                Status = VerificationStatus.Successful,
            });
        }

        [When(@"I send a request to report a WhatsApp verification by ""(.*)"" with the phone number ""(.*)""")]
        public async Task WhenISendARequestToReportAWhatsAppVerificationWithThePhoneNumber(string byType, string phoneNumber)
        {
            _whatsAppReport = await _sinchVerifications.ReportWhatsAppByIdentity(phoneNumber,
                new ReportWhatsAppVerificationRequest()
                {
                    WhatsApp = new WhatsApp()
                    {
                        Code = "5678"
                    }
                });
        }

    }
}
