using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation.Events
{
    public sealed class ConversationEventTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeConversationEventWithAppEvent()
        {
            var json = Helpers.LoadResources("Conversation/Events/ConversationEventWithAppEvent.json");

            var result = JsonSerializer.Deserialize<ConversationEvent>(json, Conversation.JsonSerializerOptions);

            result.Should().NotBeNull();
            result!.Id.Should().Be("01W4FFL35P4NC4K35CONVEVENT1");
            result.Direction.Should().Be(ConversationDirection.ToContact);
            result.AppEvent.Should().BeEquivalentTo(new AppEvent(new ComposingEvent()));
            result.ContactEvent.Should().BeNull();
            result.ContactMessageEvent.Should().BeNull();
            result.ChannelIdentity.Should().BeEquivalentTo(new ChannelIdentity
            {
                Channel = ConversationChannel.Messenger,
                Identity = "7968425018576406",
                AppId = "01W4FFL35P4NC4K35CONVAPP001"
            });
            result.ConversationId.Should().Be("01W4FFL35P4NC4K35CONVERSATI");
            result.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
            result.ProcessingMode.Should().Be(ProcessingMode.Conversation);
        }

        [Fact]
        public void DeserializeConversationEventWithContactEvent()
        {
            var json = Helpers.LoadResources("Conversation/Events/ConversationEventWithContactEvent.json");

            var result = JsonSerializer.Deserialize<ConversationEvent>(json, Conversation.JsonSerializerOptions);

            var expectedContactEvent = new ContactEvent(new ComposingEndEvent());

            result.ContactEvent.Should().BeEquivalentTo(expectedContactEvent);
            result.AppEvent.Should().BeNull();
            result.ContactMessageEvent.Should().BeNull();
        }

        [Fact]
        public void DeserializeConversationEventWithContactMessageEvent()
        {
            var json = Helpers.LoadResources("Conversation/Events/ConversationEventWithContactMessageEvent.json");

            var result = JsonSerializer.Deserialize<ConversationEvent>(json, Conversation.JsonSerializerOptions);

            var expectedContactEvent = new ContactMessageEvent(
                new PaymentStatusUpdateEvent
                {
                    ReferenceId = "ref_id",
                    PaymentStatus = PaymentStatus.PaymentStatusFailed,
                    PaymentTransactionStatus = PaymentTransactionStatus.PaymentStatusTransactionFailed,
                    PaymentTransactionId = "transaction_id"
                });

            result.Should().NotBeNull();
            result!.Id.Should().Be("01W4FFL35P4NC4K35CONVEVENT3");
            result.ContactMessageEvent.Should().BeEquivalentTo(expectedContactEvent);
            result.AppEvent.Should().BeNull();
            result.ContactEvent.Should().BeNull();
        }
    }
}

