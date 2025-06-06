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

        [Given(@"the Verification service ""Report"" is available")]
        public void GivenTheVerificationServiceIsAvailable()
        {
            _sinchVerifications = Utils.SinchVerificationClient.Verification;
        }


        [When(@"I send a request to report an SMS verification with the verification ID")]
        public async Task WhenISendARequestToReportAnSmsVerificationWithTheVerificationId()
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

        [Then(@"the response contains the details of an SMS verification report")]
        public void ThenTheResponseContainsTheDetailsOfAnSmsVerificationReport()
        {
            _smsReport.Should().BeEquivalentTo(new ReportSmsVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Status = VerificationStatus.Successful,
            });
        }

        [When(@"I send a request to report an SMS verification with the phone number")]
        public async Task WhenISendARequestToReportAnSmsVerificationWithThePhoneNumber()
        {
            _smsReport = await _sinchVerifications.ReportSmsByIdentity("+46123456789",
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "OQP1"
                    }
                });
        }

        [When(@"I send a request to report a Phone Call verification with the verification ID")]
        public async Task WhenISendARequestToReportAPhoneCallVerificationWithTheVerificationId()
        {
            _phoneCallReport = await _sinchVerifications.ReportCalloutById("1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                new ReportCalloutVerificationRequest()
                {
                    Callout = new Callout()
                    {
                        Code = "123456"
                    }
                });
        }

        [Then(@"the response contains the details of a Phone Call verification report")]
        public void ThenTheResponseContainsTheDetailsOfAPhoneCallVerificationReport()
        {
            _phoneCallReport.Should().BeEquivalentTo(new ReportCalloutVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                CallComplete = true,
            });
        }

        [When(@"I send a request to report a Phone Call verification with the phone number")]
        public async Task WhenISendARequestToReportAPhoneCallVerificationWithThePhoneNumber()
        {
            _phoneCallReport = await _sinchVerifications.ReportCalloutByIdentity("+33612345678",
                new ReportCalloutVerificationRequest()
                {
                    Callout = new Callout()
                    {
                        Code = "123456"
                    }
                });
        }

        [When(@"I send a request to report a Flash Call verification with the verification ID")]
        public async Task WhenISendARequestToReportAFlashCallVerificationWithTheVerificationId()
        {
            _flashCallReport = await _sinchVerifications.ReportFlashCallById("1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "+18156540001"
                    }
                });
        }

        [Then(@"the response contains the details of a Flash Call verification report")]
        public void ThenTheResponseContainsTheDetailsOfAFlashCallVerificationReport()
        {
            _flashCallReport.Should().BeEquivalentTo(new ReportFlashCallVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                Reference = "flashcall-verification-test-e2e",
                CallComplete = true,
            });
        }

        [When(@"I send a request to report a Flash Call verification with the phone number")]
        public async Task WhenISendARequestToReportAFlashCallVerificationWithThePhoneNumber()
        {
            _flashCallReport = await _sinchVerifications.ReportFlashCallByIdentity("+33612345678",
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "+18156540001"
                    }
                });
        }

        [Then(@"the response contains the details of a failed Flash Call verification report")]
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
    }
}
