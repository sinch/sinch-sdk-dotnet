using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStartTests : VerificationTestBase
    {
        private readonly string _id = "123";

        private readonly Identity _identity = new()
        {
            Endpoint = "+48000000",
            Type = IdentityType.Number
        };

        private readonly List<Links> _links = new()
        {
            new Links
            {
                Method = "put",
                Href = "href",
                Rel = "status"
            }
        };

        // oas file based
        [Fact]
        public async Task StartSmsVerificationOas()
        {
            var startVerificationRequest = new StartVerificationRequest
            {
                Custom = "456",
                Reference = "123",
                Method = VerificationMethodEx.Sms,
                Identity = new Identity
                {
                    Endpoint = "+49000000",
                    Type = IdentityType.Number
                }
            };

            var response = await VerificationClient.Verification.StartSms(new StartSmsVerificationRequest
            {
                Custom = startVerificationRequest.Custom,
                Reference = startVerificationRequest.Reference,
                Identity = startVerificationRequest.Identity
            });
            response.Should().BeEquivalentTo(new StartSmsVerificationResponse
            {
                Id = "some_string_value",
                Method = VerificationMethodEx.Sms,
                Sms = new SmsInfo
                {
                    Template = "some_string_value",
                    InterceptionTimeout = 0
                },
                Links = new List<Links>
                {
                    new()
                    {
                        Method = "some_string_value",
                        Href = "some_string_value",
                        Rel = "some_string_value"
                    }
                }
            });
        }

        [Fact]
        public async Task StartSmsVerification()
        {
            var startVerificationRequest = new StartVerificationRequest
            {
                Custom = "456",
                Reference = "123",
                Method = VerificationMethodEx.Sms,
                Identity = Identity.Number("+49000000")
            };

            var response = await VerificationClient.Verification.StartSms(new StartSmsVerificationRequest
            {
                Custom = startVerificationRequest.Custom,
                Reference = startVerificationRequest.Reference,
                Identity = startVerificationRequest.Identity,
                AcceptLanguage = "en-US",
                CodeType = CodeType.Alphanumeric,
                Template = "{{CODE}} - to access all secrets",
                Expiry = new TimeOnly(17, 32, 11)
            });
            response.Should().BeEquivalentTo(new StartSmsVerificationResponse
            {
                Id = "123",
                Method = VerificationMethodEx.Sms,
                Sms = new SmsInfo
                {
                    Template = "{{CODE}} - to access all secrets",
                    InterceptionTimeout = 32
                },
                Links = new List<Links>
                {
                    new()
                    {
                        Method = "put",
                        Href = "href",
                        Rel = "status"
                    }
                }
            });
        }

        [Fact]
        public async Task StartSmsVerificationNoOptions()
        {
            var response = await VerificationClient.Verification.StartSms(new StartSmsVerificationRequest
            {
                Custom = "456",
                Reference = "123",
                Identity = Identity.Number("+49000000"),
                AcceptLanguage = "en-US"
            });
            response.Should().BeEquivalentTo(new StartSmsVerificationResponse
            {
                Id = "123",
                Method = VerificationMethodEx.Sms,
                Sms = new SmsInfo
                {
                    Template = "{{CODE}} - to access all secrets",
                    InterceptionTimeout = 32
                },
                Links = new List<Links>
                {
                    new()
                    {
                        Method = "put",
                        Href = "href",
                        Rel = "status"
                    }
                }
            });
        }

        [Fact]
        public async Task StartFlashCallVerification()
        {
            var startVerificationRequest = new StartVerificationRequest
            {
                Identity = _identity,
                Method = VerificationMethodEx.FlashCall,
                FlashCallOptions = new FlashCallOptions
                {
                    DialTimeout = 12
                }
            };

            var response = await VerificationClient.Verification.StartFlashCall(new StartFlashCallVerificationRequest
            {
                Identity = startVerificationRequest.Identity,
                Reference = startVerificationRequest.Reference,
                FlashCallOptions = startVerificationRequest.FlashCallOptions,
                Custom = startVerificationRequest.Custom
            });
            response.Should().BeEquivalentTo(new StartFlashCallVerificationResponse
            {
                Id = _id,
                Method = VerificationMethodEx.FlashCall,
                FlashCall = new FlashCallDetails
                {
                    InterceptionTimeout = 50,
                    CliFilter = "cli-filter",
                    ReportTimeout = 5,
                    DenyCallAfter = 72
                },
                Links = _links
            });
        }

        [Fact]
        public async Task StartCalloutVerification()
        {
            var startVerificationRequest = new StartVerificationRequest
            {
                Identity = _identity,
                Method = VerificationMethodEx.Callout
            };

            var response = await VerificationClient.Verification.StartCallout(new StartCalloutVerificationRequest
            {
                Identity = startVerificationRequest.Identity,
                Reference = startVerificationRequest.Reference,
                Custom = startVerificationRequest.Custom,
                Locale = "en-CA"
            });
            response.Should().BeEquivalentTo(new StartCalloutVerificationResponse
            {
                Id = _id,
                Method = VerificationMethodEx.Callout,
                Links = _links
            });
        }

        [Fact]
        public async Task StartSeamlessVerification()
        {
            var startVerificationRequest = new StartVerificationRequest
            {
                Identity = _identity,
                Method = VerificationMethodEx.Seamless
            };

            var response = await VerificationClient.Verification.StartSeamless(new StartDataVerificationRequest
            {
                Identity = startVerificationRequest.Identity,
                Reference = startVerificationRequest.Reference,
                Custom = startVerificationRequest.Custom
            });
            response.Should().BeEquivalentTo(new StartDataVerificationResponse
            {
                Id = _id,
                Method = VerificationMethodEx.Seamless,
                Links = _links,
                Seamless = new Seamless
                {
                    TargetUri = "uri-target"
                }
            });
        }
    }
}
