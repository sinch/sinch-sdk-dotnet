using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Core;
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
            var url = UrlResolver.ResolveConversationUrl(region, apiUrlOverrides);
            var expectedUrl = string.IsNullOrEmpty(apiUrlOverrides?.ConversationUrl)
                ? new Uri($"https://{region.Value}.conversation.api.sinch.com/")
                : new Uri(apiUrlOverrides.ConversationUrl);
            url.Should().BeEquivalentTo(expectedUrl);
        }
    }
}
