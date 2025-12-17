using FluentAssertions;
using Sinch.Conversation.Hooks;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class WebhooksParseEventWithExtraPropertiesTests : ConversationTestBase
    {
        [Fact]
        public void ParseEvent_ShouldSucceed_WhenPayloadContainsOnlyKnownProperties()
        {
            // Arrange
            string json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventSms.json");

            // Act
            var result = Conversation.Webhooks.ParseEvent(json);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }

        [Fact]
        public void ParseEvent_ShouldSucceed_WhenPayloadContainsExtraProperties()
        {
            // Arrange
            string json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventWithExtraProperties.json");

            // Act
            var result = Conversation.Webhooks.ParseEvent(json);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }
    }
}
