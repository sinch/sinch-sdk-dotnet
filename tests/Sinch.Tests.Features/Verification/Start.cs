using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;

namespace Sinch.Tests.Features.Verification
{
    [Binding]
    public class Start
    {
        private ISinchVerification _sinchVerification;
        private StartSmsVerificationResponse _startSms;
        private StartCalloutVerificationResponse _startPhoneCallResponse;
        private StartFlashCallVerificationResponse _startFlashCall;
        private Func<Task<StartDataVerificationResponse>> _dataResponseOp;

        [Given(@"the Verification service ""Start"" is available")]
        public void GivenTheVerificationServiceIsAvailable()
        {
            _sinchVerification = Utils.SinchVerificationClient.Verification;
        }

        [When(@"I send a request to start a verification with a SMS")]
        public async Task WhenISendARequestToStartAVerificationWithAsms()
        {
            _startSms = await _sinchVerification.StartSms(new StartSmsVerificationRequest()
            {
                Identity = Identity.Number("+46123456789"),
                CodeType = CodeType.Alphanumeric,
                AcceptLanguage = "sv-SE"
            });
        }

        [Then(@"the response contains the details of a verification started with a SMS")]
        public void ThenTheResponseContainsTheDetailsOfAVerificationStartedWithAsms()
        {
            _startSms.Should().BeEquivalentTo(new StartSmsVerificationResponse()
            {
                Method = VerificationMethodEx.Sms,
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Sms = new SmsInfo()
                {
                    Template = "Din verifieringskod är {{CODE}}.",
                    InterceptionTimeout = 198,
                },
                Links = new List<Links>()
                {
                    new Links()
                    {
                        Rel = "status",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                        Method = "GET"
                    },
                    new Links()
                    {
                        Rel = "report",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                        Method = "PUT"
                    }
                }
            });
        }

        [When(@"I send a request to start a verification with a Phone Call")]
        public async Task WhenISendARequestToStartAVerificationWithAPhoneCall()
        {
            _startPhoneCallResponse = await _sinchVerification.StartCallout(new StartCalloutVerificationRequest()
            {
                Identity = Identity.Number("+33612345678"),
                Locale = "fr-FR"
            });
        }

        [Then(@"the response contains the details of a verification started with a Phone Call")]
        public void ThenTheResponseContainsTheDetailsOfAVerificationStartedWithAPhoneCall()
        {
            _startPhoneCallResponse.Should().BeEquivalentTo(new StartCalloutVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                Method = VerificationMethodEx.Callout,
                Links = new List<Links>()
                {
                    new Links()
                    {
                        Rel = "status",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                        Method = "GET",
                    },
                    new Links()
                    {
                        Rel = "report",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                        Method = "PUT"
                    }
                }
            });
        }

        [When(@"I send a request to start a verification with a Flash Call")]
        public async Task WhenISendARequestToStartAVerificationWithAFlashCall()
        {
            _startFlashCall = await _sinchVerification.StartFlashCall(new StartFlashCallVerificationRequest()
            {
                Identity = Identity.Number("+33612345678"),
                FlashCallOptions = new FlashCallOptions()
                {
                    DialTimeout = 10,
                },
                Reference = "flashcall-verification-test-e2e"
            });
        }

        [Then(@"the response contains the details of a verification started with a Flash Call")]
        public void ThenTheResponseContainsTheDetailsOfAVerificationStartedWithAFlashCall()
        {
            _startFlashCall.Should().BeEquivalentTo(new StartFlashCallVerificationResponse()
            {
                Id = "1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                Method = VerificationMethodEx.FlashCall,
                FlashCall = new FlashCallDetails()
                {
                    CliFilter = "(.*)8156(.*)",
                    InterceptionTimeout = 45,
                    ReportTimeout = 75,
                    DenyCallAfter = 0,
                },
                Links = new List<Links>()
                {
                    new Links()
                    {
                        Rel = "status",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                        Method = "GET",
                    },
                    new Links()
                    {
                        Rel = "report",
                        Href =
                            "http://localhost:3018/verification/v1/verifications/id/1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                        Method = "PUT"
                    }
                }
            });
        }


        [When(@"I send a request to start a Data verification for a not available destination")]
        public void WhenISendARequestToStartADataVerificationForANotAvailableDestination()
        {
            _dataResponseOp = () => _sinchVerification.StartSeamless(new StartDataVerificationRequest()
            {
                Identity = Identity.Number("+17818880008")
            });
        }

        [Then(@"the response contains the error details of a Data verification")]
        public async Task ThenTheResponseContainsTheErrorDetailsOfADataVerification()
        {
            var ex = await _dataResponseOp.Should().ThrowAsync<SinchApiException>();
            ex.Which.Message.Should()
                .BeEquivalentTo("Forbidden:Seamless verification not available for given destination.");
            ex.Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
