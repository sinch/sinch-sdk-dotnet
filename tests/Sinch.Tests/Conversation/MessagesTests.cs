using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class MessagesTests : ConversationTestBase
    {
        [Fact]
        public async Task GetMessage()
        {
            var messageId = "123_abc";
            var responseObj = Message();
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages/{messageId}")
                .WithQueryString("messages_source", "CONVERSATION_SOURCE")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(responseObj));

            var response = await Conversation.Messages.Get(messageId, MessageSource.ConversationSource);

            response.Should().NotBeNull();
            response.AppMessage.Message.Should().BeOfType<ListMessage>()
                .Which.Should().BeEquivalentTo(new ListMessage
                {
                    Title = "title",
                    Sections = new List<ListSection>()
                    {
                        new ListSection()
                        {
                            Title = "sec1",
                            Items = new List<IListItem>()
                            {
                                new ListItemChoice()
                                {
                                    Title = "title",
                                    Description = "desc",
                                    Media = new MediaMessage()
                                    {
                                        Url = new Uri("http://localhost")
                                    },
                                    PostbackData = "postback"
                                }
                            }
                        },
                        new ListSection()
                        {
                            Title = "sec2",
                            Items = new List<IListItem>()
                            {
                                new ListItemProduct()
                                {
                                    Id = "id",
                                    Marketplace = "amazon"
                                }
                            }
                        }
                    }
                });
            response.Direction.Should().Be(ConversationDirection.UndefinedDirection);
            response.ContactMessage.ReplyTo.MessageId.Should().Be("string");
            response.ChannelIdentity.Should().BeEquivalentTo(new ChannelIdentity()
            {
                AppId = "string",
                Channel = ConversationChannel.WhatsApp,
                Identity = "string"
            });
        }

        // I'm sorry, but it's really that complex object...
        private static object Message()
        {
            var responseObj = new
            {
                accept_time = "2019-08-24T14:15:22Z",
                app_message = new
                {
                    message = new
                    {
                        title = "title",
                        sections = new dynamic[]
                        {
                            new
                            {
                                title = "sec1",
                                items = new[]
                                {
                                    new
                                    {
                                        title = "title",
                                        description = "desc",
                                        media = new
                                        {
                                            url = "http://localhost",
                                        },
                                        postback_data = "postback"
                                    }
                                }
                            },
                            new
                            {
                                title = "sec2",
                                items = new[]
                                {
                                    new
                                    {
                                        id = "id",
                                        marketplace = "amazon"
                                    }
                                }
                            }
                        }
                    },
                    explicit_channel_message = new { },
                    additionalProperties = new
                    {
                        contact_name = "string"
                    }
                },
                channel_identity = new
                {
                    app_id = "string",
                    channel = "WHATSAPP",
                    identity = "string"
                },
                contact_id = "string",
                contact_message = new
                {
                    choice_response_message = new
                    {
                        message_id = "string",
                        postback_data = "string"
                    },
                    fallback_message = new
                    {
                        raw_message = "string",
                        reason = new { }
                    },
                    location_message = new
                    {
                        coordinates = new { },
                        label = "string",
                        title = "string"
                    },
                    media_card_message = new
                    {
                        caption = "string",
                        url = "string"
                    },
                    media_message = new
                    {
                        thumbnail_url = "string",
                        url = "string"
                    },
                    reply_to = new
                    {
                        message_id = "string"
                    },
                    text_message = new
                    {
                        text = "string"
                    }
                },
                conversation_id = "string",
                direction = "UNDEFINED_DIRECTION",
                id = "string",
                metadata = "string",
                injected = true
            };
            return responseObj;
        }
    }
}
