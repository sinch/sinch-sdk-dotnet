using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.AppEvents;
using Xunit;

namespace Sinch.Tests.Conversation.Events
{
    public class AppEventTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeAppEventWithCommentReplyEvent()
        {
            var json = Helpers.LoadResources("Conversation/Events/AppEventWithCommentReplyEvent.json");

            var result = JsonSerializer.Deserialize<AppEvent>(json, Conversation.JsonSerializerOptions);

            result.Should().BeEquivalentTo(new AppEvent(new CommentReplyEvent
            {
                Text = "This is a reply to a comment"
            }));
        }
    }
}
