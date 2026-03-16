using System;
using System.Text.Json.Nodes;
using FluentAssertions;
using Sinch.Conversation;
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
        
        [Fact]
        public void Validate_ThrowsWhenRegionNotSet()
        {
            var client = new SinchClient(new SinchClientConfiguration());
            var act = () => client.Conversation;
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Region*required*");
        }

        [Fact]
        public void Validate_DoesNotThrow_WhenRegionIsSet()
        {
            var client = new SinchClient(new SinchClientConfiguration
            {
                ConversationConfiguration = new SinchConversationConfiguration { Region = ConversationRegion.Us }
            });
            var act = () => client.Conversation;
            act.Should().NotThrow<InvalidOperationException>();
        }
    }
}
