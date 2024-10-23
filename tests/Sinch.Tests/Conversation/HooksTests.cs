using System;
using Sinch.Conversation.Hooks;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Hooks.Models;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class HooksTests
    {
        [Fact]
        public void DeserializeContactEvent()
        {
            var messageData = new
            {
                app_id = "01EB37HMH1M6SV18ABNS3G135H",
                accepted_time = "2020-11-16T08:17:44.993024Z",
                event_time = "2020-11-16T08:17:42.814Z",
                project_id = "c36f3d3d-1523-4edd-ae42-11995557ff61",
                message = new
                {
                    id = "01EQ8235TD19N21XQTH12B145D",
                    direction = "TO_APP",
                    contact_message = new
                    {
                        text_message = new
                        {
                            text = "Hi!"
                        }
                    },
                    channel_identity = new
                    {
                        channel = "MESSENGER",
                        identity = "2742085512340733",
                        app_id = "01EB37HMH1M6SV18ABNS3G135H"
                    },
                    conversation_id = "01EQ8172WMDB8008EFT4M30481",
                    contact_id = "01EQ4174TGGY5B1VPTPGHW19R0",
                    metadata = "",
                    accept_time = "2020-11-16T08:17:43.915829Z",
                    sender_id = "12039414555",
                    processing_mode = "CONVERSATION",
                    injected = false
                },
                message_metadata = new JsonObject()
                {
                    ["arbitrary"] = "json object stringify as metadata"
                },
                correlation_id = "correlation-id-1"
            };
            var json = JsonSerializer.Serialize(messageData);

            var result = JsonSerializer.Deserialize<ICallbackEvent>(json);
            result.Should().BeEquivalentTo(new MessageInboundEvent()
            {
                AppId = "01EB37HMH1M6SV18ABNS3G135H",
                AcceptedTime = DateTime.Parse("2020-11-16T08:17:44.993024Z").ToUniversalTime(),
                EventTime = DateTime.Parse("2020-11-16T08:17:42.814Z").ToUniversalTime(),
                ProjectId = "c36f3d3d-1523-4edd-ae42-11995557ff61",
                CorrelationId = "correlation-id-1",
                Message = new MessageInboundEventItem()
                {
                    Id = "01EQ8235TD19N21XQTH12B145D",
                    Direction = ConversationDirection.ToApp,
                    ConversationId = "01EQ8172WMDB8008EFT4M30481",
                    ContactId = "01EQ4174TGGY5B1VPTPGHW19R0",
                    Metadata = "",
                    AcceptTime = DateTime.Parse("2020-11-16T08:17:43.915829Z").ToUniversalTime(),
                    SenderId = "12039414555",
                    ProcessingMode = ProcessingMode.Conversation,
                    Injected = false,
                    ChannelIdentity = new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Messenger,
                        Identity = "2742085512340733",
                        AppId = "01EB37HMH1M6SV18ABNS3G135H"
                    },
                    ContactMessage = new ContactMessage(new TextMessage("Hi!")),
                }
            }, options => options.Excluding(x => x.MessageMetadata));
            result.As<MessageInboundEvent>().MessageMetadata!["arbitrary"]!.GetValue<string>().Should()
                .BeEquivalentTo("json object stringify as metadata");
        }
    }
}
