using FluentAssertions;
using Sinch.Conversation.Hooks;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class WebhooksParseEventWithExtraPropertiesTests : ConversationTestBase
    {
        [Fact]
        public void ParseEventShouldSucceedWhenPayloadContainsOnlyKnownProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventSms.json");
            
            var result = Conversation.Webhooks.ParseEvent(json);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }

        [Fact]
        public void ParseEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventSmsWithExtraProperties.json");
            
            var result = Conversation.Webhooks.ParseEvent(json);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }
    }
}
