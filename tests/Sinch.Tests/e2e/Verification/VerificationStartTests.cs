﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;
using Xunit;
using VerificationMethod = Sinch.Verification.Start.Request.VerificationMethod;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStartTests : VerificationTestBase
    {
        private List<Links> _links = new List<Links>()
        {
            new()
            {
                Method = "put",
                Href = "href",
                Rel = "status"
            }
        };

        private string _id = "123";

        private Identity _identity = new Identity()
        {
            Endpoint = "+48000000",
            Type = IdentityType.Number
        };

        [Fact]
        public async Task StartSmsVerification()
        {
            var response = await VerificationClient.Verification.Start(new VerificationStartRequest()
            {
                Custom = "456",
                Reference = "123",
                Method = VerificationMethod.Sms,
                Identity = new Identity()
                {
                    Endpoint = "+49000000",
                    Type = IdentityType.Number
                },
            });

            response.Should().BeOfType<SmsVerificationStartResponse>().Which.Should().BeEquivalentTo(
                new SmsVerificationStartResponse()
                {
                    Id = "1234567890",
                    Method = VerificationMethod.Sms,
                    Sms = new SmsInfo()
                    {
                        Template = "Your verification code is {{CODE}}",
                        InterceptionTimeout = 32,
                    },
                    Links = new List<Links>()
                    {
                        new()
                        {
                            Method = "GET",
                            Href = "some_string_value",
                            Rel = "status"
                        }
                    }
                });
        }

        [Fact]
        public async Task StartFlashCallVerification()
        {
            var response = await VerificationClient.Verification.Start(new VerificationStartRequest()
            {
                Identity = _identity,
                Method = VerificationMethod.FlashCall,
                FlashCallOptions = new FlashCallOptions()
                {
                    DialTimeout = 12,
                }
            });
            response.Should().BeOfType<FlashCallVerificationStartResponse>().Which.Should().BeEquivalentTo(
                new FlashCallVerificationStartResponse()
                {
                    Id = _id,
                    Method = VerificationMethod.FlashCall,
                    FlashCall = new FlashCallDetails()
                    {
                        InterceptionTimeout = 50,
                        CliFilter = "cli-filter",
                        ReportTimeout = 5,
                        DenyCallAfter = 72,
                    },
                    Links = _links
                });
        }

        [Fact]
        public async Task StartPhoneCallVerification()
        {
            var response = await VerificationClient.Verification.Start(new VerificationStartRequest()
            {
                Identity = _identity,
                Method = VerificationMethod.Callout,
            });
            response.Should().BeOfType<PhoneCallVerificationStartResponse>().Which.Should().BeEquivalentTo(
                new PhoneCallVerificationStartResponse()
                {
                    Id = _id,
                    Method = VerificationMethod.Callout,
                    Links = _links
                });
        }
        
        [Fact]
        public async Task StartSeamlessVerification()
        {
            var response = await VerificationClient.Verification.Start(new VerificationStartRequest()
            {
                Identity = _identity,
                Method = VerificationMethod.Seamless,
            });
            response.Should().BeOfType<DataVerificationStartResponse>().Which.Should().BeEquivalentTo(
                new DataVerificationStartResponse()
                {
                    Id = _id,
                    Method = VerificationMethod.Seamless,
                    Links = _links,
                    Seamless = new Seamless()
                    {
                        TargetUri = "uri-target"
                    }
                });
        }
    }
}