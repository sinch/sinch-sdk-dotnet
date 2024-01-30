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
            var response = await VerificationClient.Verification.Start(new StartVerificationRequest()
            {
                Custom = "456",
                Reference = "123",
                Method = VerificationMethodEx.Sms,
                Identity = new Identity()
                {
                    Endpoint = "+49000000",
                    Type = IdentityType.Number
                },
            });

            response.Should().BeOfType<StartSmsVerificationResponse>().Which.Should().BeEquivalentTo(
                new StartSmsVerificationResponse()
                {
                    Id = "1234567890",
                    Method = VerificationMethodEx.Sms,
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
            var response = await VerificationClient.Verification.Start(new StartVerificationRequest()
            {
                Identity = _identity,
                Method = VerificationMethodEx.FlashCall,
                FlashCallOptions = new FlashCallOptions()
                {
                    DialTimeout = 12,
                }
            });
            response.Should().BeOfType<StartFlashCallVerificationResponse>().Which.Should().BeEquivalentTo(
                new StartFlashCallVerificationResponse()
                {
                    Id = _id,
                    Method = VerificationMethodEx.FlashCall,
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
            var response = await VerificationClient.Verification.Start(new StartVerificationRequest()
            {
                Identity = _identity,
                Method = VerificationMethodEx.Callout,
            });
            response.Should().BeOfType<StartPhoneCallVerificationResponse>().Which.Should().BeEquivalentTo(
                new StartPhoneCallVerificationResponse()
                {
                    Id = _id,
                    Method = VerificationMethodEx.Callout,
                    Links = _links
                });
        }

        [Fact]
        public async Task StartSeamlessVerification()
        {
            var response = await VerificationClient.Verification.Start(new StartVerificationRequest()
            {
                Identity = _identity,
                Method = VerificationMethodEx.Seamless,
            });
            response.Should().BeOfType<StartDataVerificationResponse>().Which.Should().BeEquivalentTo(
                new StartDataVerificationResponse()
                {
                    Id = _id,
                    Method = VerificationMethodEx.Seamless,
                    Links = _links,
                    Seamless = new Seamless()
                    {
                        TargetUri = "uri-target"
                    }
                });
        }
    }
}
