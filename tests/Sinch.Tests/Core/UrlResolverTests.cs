using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Core;
using Sinch.Mailgun;
using Sinch.SMS;
using Sinch.Voice;
using Xunit;

namespace Sinch.Tests.Core
{
    public class UrlResolverTests
    {
        public static IEnumerable<object[]> ConversationUrlResolveData => new List<object[]>
        {
            new object[] { ConversationRegion.Eu, null },
            new object[] { ConversationRegion.Br, null },
            new object[] { ConversationRegion.Us, null },
            new object[]
            {
                ConversationRegion.Us, new ApiUrlOverrides()
                {
                    ConversationUrl = null
                }
            },
            new object[]
            {
                ConversationRegion.Us, new ApiUrlOverrides()
                {
                    ConversationUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(ConversationUrlResolveData))]
        public void ResolveConversationUrl(ConversationRegion region, ApiUrlOverrides apiUrlOverrides)
        {
            var conversationUrl = new UrlResolver(apiUrlOverrides).ResolveConversationUrl(region);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.ConversationUrl)
                ? new Uri($"https://{region.Value}.conversation.api.sinch.com/")
                : new Uri(apiUrlOverrides.ConversationUrl);
            conversationUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> AuthUrlResolveData => new List<object[]>
        {
            new object[]
            {
                null
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    AuthUrl = null
                }
            }
        };

        [Theory]
        [MemberData(nameof(AuthUrlResolveData))]
        public void ResolveAuthUrl(ApiUrlOverrides apiUrlOverrides)
        {
            var authUrl = new UrlResolver(apiUrlOverrides).ResolveAuth();
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.AuthUrl)
                ? new Uri("https://auth.sinch.com")
                : new Uri(apiUrlOverrides.AuthUrl);
            authUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> VoiceUrlResolveData => new List<object[]>
        {
            new object[]
            {
                VoiceRegion.Global, null
            },
            new object[]
            {
                VoiceRegion.Europe, null
            },
            new object[]
            {
                VoiceRegion.NorthAmerica, null
            },
            new object[]
            {
                VoiceRegion.SouthAmerica, null
            },
            new object[]
            {
                VoiceRegion.SouthEastAsia2, null
            },
            new object[]
            {
                VoiceRegion.SouthEastAsia1, null
            },
            new object[]
            {
                VoiceRegion.SouthEastAsia1,
                new ApiUrlOverrides()
                {
                    VoiceUrl = null
                }
            },
            new object[]
            {
                VoiceRegion.SouthEastAsia1,
                new ApiUrlOverrides()
                {
                    VoiceUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(VoiceUrlResolveData))]
        public void ResolveVoiceUrl(VoiceRegion voiceRegion, ApiUrlOverrides apiUrlOverrides)
        {
            var voiceUrl = new UrlResolver(apiUrlOverrides).ResolveVoiceUrl(voiceRegion);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.VoiceUrl)
                ? new Uri($"https://{voiceRegion.Value}.api.sinch.com/")
                : new Uri(apiUrlOverrides.VoiceUrl);
            voiceUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> VoiceApplicationManagementData => new List<object[]>
        {
            new object[]
            {
                null
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    VoiceApplicationManagementUrl = null
                }
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    VoiceApplicationManagementUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(VoiceApplicationManagementData))]
        public void ResolveVoiceApplicationManagementUrl(ApiUrlOverrides apiUrlOverrides)
        {
            var voiceUrl = new UrlResolver(apiUrlOverrides).ResolveVoiceApplicationManagementUrl();
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.VoiceApplicationManagementUrl)
                ? new Uri($"https://callingapi.sinch.com/")
                : new Uri(apiUrlOverrides.VoiceApplicationManagementUrl);
            voiceUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> VerificationUrlData => new List<object[]>
        {
            new object[]
            {
                null
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    VerificationUrl = null
                }
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    VerificationUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(VerificationUrlData))]
        public void ResolveVerificationUrl(ApiUrlOverrides apiUrlOverrides)
        {
            var verificationUrl = new UrlResolver(apiUrlOverrides).ResolveVerificationUrl();
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.VerificationUrl)
                ? new Uri($"https://verification.api.sinch.com/")
                : new Uri(apiUrlOverrides.VerificationUrl);
            verificationUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> NumbersUrlData => new List<object[]>
        {
            new object[]
            {
                null
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    NumbersUrl = null
                }
            },
            new object[]
            {
                new ApiUrlOverrides()
                {
                    NumbersUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(NumbersUrlData))]
        public void ResolveNumbersUrl(ApiUrlOverrides apiUrlOverrides)
        {
            var verificationUrl = new UrlResolver(apiUrlOverrides).ResolveNumbersUrl();
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.NumbersUrl)
                ? new Uri("https://numbers.api.sinch.com/")
                : new Uri(apiUrlOverrides.NumbersUrl);
            verificationUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> SmsUrlData => new List<object[]>
        {
            new object[]
            {
                SmsRegion.Eu,
                null,
            },
            new object[]
            {
                SmsRegion.Us,
                new ApiUrlOverrides()
                {
                    SmsUrl = null
                }
            },
            new object[]
            {
                SmsRegion.Eu,
                new ApiUrlOverrides()
                {
                    SmsUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(SmsUrlData))]
        public void ResolveSmsUrl(SmsRegion smsRegion, ApiUrlOverrides apiUrlOverrides)
        {
            var smsUrl = new UrlResolver(apiUrlOverrides).ResolveSmsUrl(smsRegion);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.SmsUrl)
                ? new Uri($"https://zt.{smsRegion.Value}.sms.api.sinch.com/")
                : new Uri(apiUrlOverrides.SmsUrl);
            smsUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public static IEnumerable<object[]> SmsServicePlanIdUrlData => new List<object[]>
        {
            new object[]
            {
                SmsServicePlanIdRegion.Us,
                null,
            },
            new object[]
            {
                SmsServicePlanIdRegion.Eu,
                null,
            },
            new object[]
            {
                SmsServicePlanIdRegion.Au,
                null,
            },
            new object[]
            {
                SmsServicePlanIdRegion.Br,
                null,
            },
            new object[]
            {
                SmsServicePlanIdRegion.Ca,
                null,
            },
            new object[]
            {
                SmsServicePlanIdRegion.Us,
                new ApiUrlOverrides()
                {
                    SmsUrl = null
                }
            },
            new object[]
            {
                SmsServicePlanIdRegion.Eu,
                new ApiUrlOverrides()
                {
                    SmsUrl = "https://hello.world"
                }
            }
        };

        [Theory]
        [MemberData(nameof(SmsServicePlanIdUrlData))]
        public void ResolveSmsServicePlanIdUrl(SmsServicePlanIdRegion smsServicePlanIdRegion,
            ApiUrlOverrides apiUrlOverrides)
        {
            var smsUrl = new UrlResolver(apiUrlOverrides).ResolveSmsServicePlanIdUrl(smsServicePlanIdRegion);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.SmsUrl)
                ? new Uri($"https://{smsServicePlanIdRegion.Value}.sms.api.sinch.com/")
                : new Uri(apiUrlOverrides.SmsUrl);
            smsUrl.Should().BeEquivalentTo(expectedUrl);
        }

        public record ResolveMailgunUrlTestData(
            string TestName,
            MailgunRegion Region,
            ApiUrlOverrides ApiUrlOverrides,
            string ExpectedUrl)
        {
            private static readonly ResolveMailgunUrlTestData[] TestCases =
            {
                new("Default US Mailgun address", MailgunRegion.Us, null, "https://api.mailgun.net"),
                new("Default EU Mailgun address", MailgunRegion.Eu, null, "https://api.eu.mailgun.net"),
                new("Default EU region even if override set but null", MailgunRegion.Eu, new ApiUrlOverrides()
                {
                    MailgunUrl = null
                }, "https://api.eu.mailgun.net"),
                new("Override url", MailgunRegion.Eu, new ApiUrlOverrides()
                {
                    MailgunUrl = "https://my-mailgun-proxy.net"
                }, "https://my-mailgun-proxy.net"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString()
            {
                return TestName;
            }
        }

        [Theory]
        [MemberData(nameof(ResolveMailgunUrlTestData.TestCasesData), MemberType = typeof(ResolveMailgunUrlTestData))]
        public void ResolveMailgunUrl(ResolveMailgunUrlTestData data)
        {
            var actual = new UrlResolver(data.ApiUrlOverrides).ResolveMailgunUrl(data.Region);

            actual.Should().BeEquivalentTo(new Uri(data.ExpectedUrl));
        }

        [Fact]
        public void ThrowIfRegionIsUnexpected()
        {
            var op = () => new UrlResolver(new ApiUrlOverrides()).ResolveMailgunUrl((MailgunRegion)(-1));
            op.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}
