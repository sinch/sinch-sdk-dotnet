using System.Text.Json.Nodes;
using FluentAssertions;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class ConversationsTests
    {
        [Fact]
        public void UpdateMaskConversation()
        {
            var conversation = new Sinch.Conversation.Conversations.Conversation
            {
                ActiveChannel = null,
                Active = true,
                AppId = "null",
                ContactId = "id",
                Id = "1",
                Metadata = "n",
                MetadataJson = new JsonObject(),
                CorrelationId = string.Empty
            };

            conversation.GetPropertiesMask().Should().BeEquivalentTo(
                "active_channel,active,app_id,contact_id,metadata,metadata_json,correlation_id");
        }

        [Fact]
        public void UpdateMaskConversationOnlyOneField()
        {
            var conversation = new Sinch.Conversation.Conversations.Conversation
            {
                AppId = "AppId",
            };

            conversation.GetPropertiesMask().Should().BeEquivalentTo(
                "app_id");
        }
    }
}
