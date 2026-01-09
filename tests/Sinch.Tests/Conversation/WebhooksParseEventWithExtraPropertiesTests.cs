using FluentAssertions;
using Sinch.Conversation.Hooks;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class WebhooksParseEventWithExtraPropertiesTests : ConversationTestBase
    {
        [Fact]
        public void ParseEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventSmsWithExtraProperties.json");
            
            var result = Conversation.Webhooks.ParseEvent(json);
            
            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }

        [Fact]
        public void ParseCapabilityEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/CapabilityEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<CapabilityEvent>();
        }

        [Fact]
        public void ParseChannelEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ChannelEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ChannelEvent>();
        }

        [Fact]
        public void ParseContactCreateEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ContactCreateEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ContactCreateEvent>();
        }

        [Fact]
        public void ParseContactDeleteEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ContactDeleteEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ContactDeleteEvent>();
        }

        [Fact]
        public void ParseContactIdentitiesDuplicationEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ContactIdentitiesDuplicationEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ContactIdentitiesDuplicationEvent>();
        }

        [Fact]
        public void ParseContactMergeEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ContactMergeEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ContactMergeEvent>();
        }

        [Fact]
        public void ParseContactUpdateEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ContactUpdateEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ContactUpdateEvent>();
        }

        [Fact]
        public void ParseConversationDeleteEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ConversationDeleteEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ConversationDeleteEvent>();
        }

        [Fact]
        public void ParseConversationStartEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ConversationStartEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ConversationStartEvent>();
        }

        [Fact]
        public void ParseConversationStopEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/ConversationStopEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<ConversationStopEvent>();
        }

        [Fact]
        public void ParseDeliveryEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/EventDeliveryReportEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<DeliveryEvent>();
        }

        [Fact]
        public void ParseMessageDeliveryReceiptEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageDeliveryReceiptEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessageDeliveryReceiptEvent>();
        }

        [Fact]
        public void ParseInboundEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/InboundContactEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<InboundEvent>();
        }

        [Fact]
        public void ParseMessageInboundEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageInboundEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundEvent>();
        }

        [Fact]
        public void ParseMessageSubmitEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageSubmitEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessageSubmitEvent>();
        }

        [Fact]
        public void ParseMessageInboundSmartConversationRedactionEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/MessageInboundSmartConversationRedactionEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessageInboundSmartConversationRedactionEvent>();
        }

        [Fact]
        public void ParseSmartConversationsEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/SmartConversationsEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<SmartConversationsEvent>();
        }

        [Fact]
        public void ParseUnsupportedCallbackEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/UnsupportedCallbackEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<UnsupportedCallbackEvent>();
        }

        [Fact]
        public void ParseOptInEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/OptInEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<OptInEvent>();
        }

        [Fact]
        public void ParseOptOutEventShouldSucceedWhenPayloadContainsExtraProperties()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/OptOutEventWithExtraProperties.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<OptOutEvent>();
        }
    }
}
