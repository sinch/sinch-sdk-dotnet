using System;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Core;
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
            var conversationUrl = UrlResolver.ResolveConversationUrl(region, apiUrlOverrides);
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
            var authUrl = UrlResolver.ResolveAuthApiUrl(apiUrlOverrides);
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
            var voiceUrl = UrlResolver.ResolveVoiceApiUrl(voiceRegion, apiUrlOverrides);
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
            var voiceUrl = UrlResolver.ResolveVoiceApiApplicationManagementUrl(apiUrlOverrides);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.VoiceApplicationManagementUrl)
                ? new Uri($"https://callingapi.sinch.com/")
                : new Uri(apiUrlOverrides.VoiceApplicationManagementUrl);
            voiceUrl.Should().BeEquivalentTo(expectedUrl);
        }
    }
}
