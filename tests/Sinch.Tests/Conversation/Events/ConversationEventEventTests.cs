using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Xunit;

namespace Sinch.Tests.Conversation.Events
{
    public sealed class ConversationEventEventTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeConversationEventEventWithAppEvent()
        {
            var json = Helpers.LoadResources("Conversation/Events/ConversationEventEventWithAppEvent.json");

            var result = JsonSerializer.Deserialize<ConversationEventEvent>(json, Conversation.JsonSerializerOptions);

            result.Should().BeEquivalentTo(new ConversationEventEvent(
                new AppEvent(new ComposingEvent())));
        }
    }
}

