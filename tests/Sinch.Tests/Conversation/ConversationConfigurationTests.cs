using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Conversation;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class ConversationConfigurationTests
    {
        public record ConversationUrlResolveTests(
            string TestName,
            ConversationRegion Region,
            string UrlOverride,
            string Expected)
        {
            private static readonly ConversationUrlResolveTests[] TestCases =
            {
                new("Brazil region", ConversationRegion.Br, null, $"https://br.conversation.api.sinch.com/"),
                new("Europe region", ConversationRegion.Eu, null, $"https://eu.conversation.api.sinch.com/"),
                new("Us region", ConversationRegion.Us, null, $"https://us.conversation.api.sinch.com/"),
                new("Url override", ConversationRegion.Us, "https://hello.world", "https://hello.world/"),
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString()
            {
                return TestName;
            }
        }

        [Theory]
        [MemberData(nameof(ConversationUrlResolveTests.TestCasesData),
            MemberType = typeof(ConversationUrlResolveTests))]
        public void ResolveUrl(ConversationUrlResolveTests testCase)
        {
            var config = new SinchConversationConfiguration()
            {
                ConversationRegion = testCase.Region,
                ConversationUrlOverride = testCase.UrlOverride,
            };

            config.ResolveConversationUrl().ToString().Should().Be(testCase.Expected);
        }
    }
}
