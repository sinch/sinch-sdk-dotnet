using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class TranscodingTests : TestBase
    {
        [Fact]
        public async Task SendTranscode()
        {
            var response = await SinchClientMockServer.Conversation.Transcoding.Transcode(new TranscodeRequest
            {
                AppId = "123",
                From = "hello",
                To = "world",
                Channels = new List<ConversationChannel>()
                {
                    ConversationChannel.Instagram, ConversationChannel.WhatsApp
                },
                AppMessage = new AppMessage(new TextMessage("aaa"))
                {
                    ExplicitChannelMessage = new Dictionary<ConversationChannel, string>()
                    {
                        { ConversationChannel.WhatsApp, "data" }
                    },
                    Agent = new Agent()
                    {
                        Type = AgentType.UnknownAgentType,
                        PictureUrl = "https://upload.wikimedia.org/wikipedia/en/b/b9/Elden_Ring_Box_art.jpg",
                        DisplayName = "Elden ring"
                    }
                }
            });
            response.Should().BeEquivalentTo(new TranscodeResponse()
            {
                TranscodedMessage = new Dictionary<ConversationChannel, string>()
                {
                    { ConversationChannel.Instagram, "data" },
                    { ConversationChannel.WhatsApp, "data" }
                }
            });
        }
    }
}
