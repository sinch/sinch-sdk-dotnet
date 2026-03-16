using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Sinch.Core;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sinch.Tests.Conversation
{
    public class MessagesTests : ConversationTestBase
    {
        [Fact]
        public async Task GetMessage()
        {
            const string messageId = "123_abc";
            var responseObj = Message();
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages/{messageId}")
                .WithQueryString("messages_source", "CONVERSATION_SOURCE")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(responseObj));

            var response = await Conversation.Messages.Get(messageId, MessageSource.ConversationSource);

            response.Should().NotBeNull();
            response.AppMessage!.ListMessage.Should().BeEquivalentTo(new ListMessage
            {
                Title = "title",
                Sections = new List<ListSection>()
                {
                    new ListSection()
                    {
                        Title = "sec1",
                        Items = new List<IListItem>()
                        {
                            new ChoiceItem()
                            {
                                Title = "title",
                                Description = "desc",
                                Media = new MediaMessage()
                                {
                                    Url = "http://localhost"
                                },
                                PostbackData = "postback"
                            }
                        }
                    },
                    new ListSection()
                    {
                        Title = "sec2",
                        Items = new List<IListItem>
                        {
                            new ProductItem()
                            {
                                Id = "id",
                                Marketplace = "amazon"
                            }
                        }
                    }
                }
            });
            response.Direction.Should().Be(ConversationDirection.ToApp);
            response.ChannelIdentity.Should().BeEquivalentTo(new ChannelIdentity()
            {
                AppId = "string",
                Channel = ConversationChannel.WhatsApp,
                Identity = "string"
            });
        }

        [Fact]
        public async Task ListMessages()
        {
            const string conversationId = "conversationId";
            const string nextPageToken = "hola!";
            const string contactId = "contact_d";
            const string appId = "appId";
            const string channelId = "channel_id";
            const string time = "2022-07-12T00:00:00.0000000";

            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages")
                .WithQueryString("conversation_id", conversationId)
                .WithQueryString("contact_id", contactId)
                .WithQueryString("app_id", appId)
                .WithQueryString("channel_identity", channelId)
                .WithQueryString("start_time", time)
                .WithQueryString("end_time", time)
                .WithQueryString("page_size", "2")
                .WithQueryString("page_token", "3")
                .WithQueryString("view", "WITH_METADATA")
                .WithQueryString("messages_source", "DISPATCH_SOURCE")
                .WithQueryString("only_recipient_originated", "true")
                .WithQueryString("direction", "TO_CONTACT")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    next_page_token = nextPageToken,
                    messages = new[]
                    {
                        Message(),
                        Message()
                    }
                }));

            var dateTime = new DateTime(2022, 7, 12);
            var response = await Conversation.Messages.List(new ListMessagesRequest
            {
                ConversationId = conversationId,
                ContactId = contactId,
                AppId = appId,
                ChannelIdentity = channelId,
                StartTime = dateTime,
                EndTime = dateTime,
                PageSize = 2,
                PageToken = "3",
                View = View.WithMetadata,
                MessagesSource = MessageSource.DispatchSource,
                OnlyRecipientOriginated = true,
                Direction = ConversationDirection.ToContact
            });

            response.Should().NotBeNull();
            response.NextPageToken.Should().Be(nextPageToken);
            response.Messages.Should().HaveCount(2);
        }

        [Fact]
        public async Task Delete()
        {
            var messageId = "123_abc";
            HttpMessageHandlerMock
                .When(HttpMethod.Delete,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages/{messageId}")
                .WithQueryString("messages_source", "CONVERSATION_SOURCE")
                .Respond(HttpStatusCode.OK);

            await Conversation.Messages.Delete(messageId, MessageSource.ConversationSource);
        }

        [Fact]
        public async Task Exception()
        {
            var messageId = "123_abc";
            HttpMessageHandlerMock
                .When(HttpMethod.Delete,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages/{messageId}")
                .WithQueryString("messages_source", "CONVERSATION_SOURCE")
                .Respond(HttpStatusCode.BadRequest, JsonContent.Create(new
                {
                    error = new
                    {
                        code = 400,
                        error = "malformed",
                        message = "Invalid argument",
                        status = "INVALID_ARGUMENT",
                        details = new[]
                        {
                            new
                            {
                                type = "type.googleapis.com/google.rpc.BadRequest",
                                field_violations = new[]
                                {
                                    new
                                    {
                                        field = "message_id",
                                        description = "Field is mandatory"
                                    }
                                }
                            }
                        }
                    }
                }));

            Func<Task> request = () => Conversation.Messages.Delete(messageId, MessageSource.ConversationSource);
            await request.Should().ThrowAsync<SinchApiException>().WithMessage("Bad Request:Invalid argument")
                .Where(x => x.DetailedMessage == "Invalid argument");
        }

        // I'm sorry, but it's really that complex object...
        private static object Message()
        {
            var responseObj = new
            {
                accept_time = "2019-08-24T14:15:22Z",
                app_message = new
                {
                    list_message = new
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
                                        choice = new
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
                                }
                            },
                            new
                            {
                                title = "sec2",
                                items = new[]
                                {
                                    new
                                    {
                                        product = new
                                        {
                                            id = "id",
                                            marketplace = "amazon"
                                        }
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
                conversation_id = "string",
                direction = "TO_APP",
                id = "string",
                metadata = "string",
                injected = true
            };
            return responseObj;
        }

        [Fact]
        public async Task ListMessagesByChannelIdentity()
        {
            const string nextPageToken = "next_token_123";
            const string time = "2026-01-01T08:30:00.0000000";

            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:fetch-last-message")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    channel_identities = new[] { "447700900000", "447700900001" },
                    app_id = "app_id_1",
                    messages_source = "DISPATCH_SOURCE",
                    page_size = 5,
                    page_token = "prev_token",
                    view = "WITHOUT_METADATA",
                    start_time = time,
                    end_time = time,
                    channel = "WHATSAPP",
                    direction = "TO_CONTACT"
                }))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    next_page_token = nextPageToken,
                    messages = new[] { Message() }
                }));

            var dateTime = new DateTime(2026, 1, 1, 8, 30, 0);
            var response = await Conversation.Messages.ListMessagesByChannelIdentity(new ListMessagesByChannelIdentityRequest
            {
                ChannelIdentities = new List<string> { "447700900000", "447700900001" },
                AppId = "app_id_1",
                MessagesSource = MessageSource.DispatchSource,
                PageSize = 5,
                PageToken = "prev_token",
                View = View.WithoutMetadata,
                StartTime = dateTime,
                EndTime = dateTime,
                Channel = ConversationChannel.WhatsApp,
                Direction = ConversationDirection.ToContact
            });

            response.Should().NotBeNull();
            response.NextPageToken.Should().Be(nextPageToken);
            response.Messages.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListMessagesByContactIds()
        {
            const string nextPageToken = "next_page_456";

            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:fetch-last-message")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    contact_ids = new[] { "01H5XXXXXXXXXXXXXXXXXXX1", "01H5XXXXXXXXXXXXXXXXXXX2" },
                    messages_source = "CONVERSATION_SOURCE"
                }))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    next_page_token = nextPageToken,
                    messages = new[] { Message() }
                }));

            var response = await Conversation.Messages.ListMessagesByChannelIdentity(new ListMessagesByChannelIdentityRequest
            {
                ContactIds = new List<string> { "01H5XXXXXXXXXXXXXXXXXXX1", "01H5XXXXXXXXXXXXXXXXXXX2" },
                MessagesSource = MessageSource.ConversationSource
            });

            response.Should().NotBeNull();
            response.NextPageToken.Should().Be(nextPageToken);
            response.Messages.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListMessagesByChannelIdentityAuto_IteratesThroughAllPages()
        {
            const string nextPageToken = "next_token_page2";

            HttpMessageHandlerMock
                .Expect(HttpMethod.Post,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:fetch-last-message")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(new
                {
                    channel_identities = new[] { "447700900000" },
                    app_id = "app_id_1",
                    messages_source = "DISPATCH_SOURCE"
                }))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    next_page_token = nextPageToken,
                    messages = new[] { Message() }
                }));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Post,
                    $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:fetch-last-message")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    channel_identities = new[] { "447700900000" },
                    app_id = "app_id_1",
                    messages_source = "DISPATCH_SOURCE",
                    page_token = nextPageToken
                }))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    next_page_token = (string?)null,
                    messages = new[] { Message() }
                }));

            var messages = new List<ConversationMessage>();
            await foreach (var message in Conversation.Messages.ListMessagesByChannelIdentityAuto(
                new ListMessagesByChannelIdentityRequest
                {
                    ChannelIdentities = new List<string> { "447700900000" },
                    AppId = "app_id_1",
                    MessagesSource = MessageSource.DispatchSource
                }))
            {
                messages.Add(message);
            }

            messages.Should().HaveCount(2);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public void SerializeBirthDate()
        {
            // the birthday format is YYYY-MM-DD
            var t = @"{ ""birthday"": ""2000-03-12"", ""name"": { ""full_name"": ""AAA""}, ""phone_numbers"":[] }";

            var contact = DeserializeAsConversationClient<ContactInfoMessage>(t);

            contact.Birthday.Should().BeSameDateAs(new DateTime(2000, 03, 12));
        }

        private FlowMessage _flowMessage = new FlowMessage()
        {
            Message = new FlowChannelSpecificMessage()
            {
                FlowId = "g",
                Body = new WhatsAppInteractiveBody()
                {
                    Text = "body_text"
                },
                Footer = new WhatsAppInteractiveFooter()
                {
                    Text = "footer_text",
                },
                Header = new WhatsAppInteractiveVideoHeader()
                {
                    Video = new WhatsAppInteractiveHeaderMedia()
                    {
                        Link = "url_video"
                    }
                },
                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                FlowCta = "flow_cta",
                FlowMode = FlowChannelSpecificMessage.FlowModeType.Published,
                FlowToken = "flow_token",
                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                {
                    Data = null,
                    Screen = "a",
                }
            }
        };

        private const string FlowsRawJson =
            "{\"message_type\":\"FLOWS\",\"message\":{\"flow_mode\":\"published\",\"flow_action\":\"navigate\",\"header\":{\"type\":\"video\",\"video\":{\"link\":\"url_video\"}},\"body\":{\"text\":\"body_text\"},\"footer\":{\"text\":\"footer_text\"},\"flow_id\":\"g\",\"flow_token\":\"flow_token\",\"flow_cta\":\"flow_cta\",\"flow_action_payload\":{\"screen\":\"a\",\"data\":null}}}";

        [Fact]
        public void SerializeFlowChannelSpecificMessage()
        {
            var val = JsonSerializer.Serialize(_flowMessage);
            var expectedJson = JObject.Parse(FlowsRawJson);
            var actualJson = JObject.Parse(val);

            actualJson.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void DeserializeFlowMessage()
        {
            var json = $"{{\"WHATSAPP\":{FlowsRawJson}}}";
            var dict = DeserializeAsConversationClient<Dictionary<ConversationChannel, IChannelSpecificMessage>>(json);
            dict[ConversationChannel.WhatsApp].Should().BeEquivalentTo(_flowMessage);
        }


        [Theory]
        [ClassData(typeof(OmniMessageTestData))]
        public void DeserializeOmniMessageOverride(string json, object dataToCheck)
        {
            var dict = JsonSerializer
                .Deserialize<Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>>(json,
                    options: new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
                    });
            dict.Should().ContainKey(ChannelSpecificTemplate.WhatsApp).WhoseValue.Should()
                .BeEquivalentTo(dataToCheck);
        }


        [Fact]
        public void DeserializeWhatsAppInteractiveHeader()
        {
            var json = Helpers.LoadResources("Conversation/Messages/WhatsAppInteractiveHeader.json");

            var result = DeserializeAsConversationClient<WhatsAppInteractiveImageHeader>(json);

            result.Should().BeEquivalentTo(new WhatsAppInteractiveImageHeader()
            {
                Image = new WhatsAppInteractiveHeaderMedia()
                {
                    Link = "an image URL link"
                }
            });
        }

        [Fact]
        public void DeserializeWhatsAppInteractiveDocument()
        {
            var json = Helpers.LoadResources("Conversation/Messages/WhatsAppInteractiveDocument.json");

            var result = DeserializeAsConversationClient<WhatsAppInteractiveDocumentHeader>(json);

            result.Should().BeEquivalentTo(new WhatsAppInteractiveDocumentHeader()
            {
                Document = new WhatsAppInteractiveHeaderMedia()
                {
                    Link = "a document URL link"
                }
            });
        }

        [Fact]
        public void DeserializeWhatsAppInteractiveVideoHeader()
        {
            var json = Helpers.LoadResources("Conversation/Messages/WhatsAppInteractiveVideoHeader.json");

            var result = DeserializeAsConversationClient<WhatsAppInteractiveVideoHeader>(json);

            result.Should().BeEquivalentTo(new WhatsAppInteractiveVideoHeader()
            {
                Video = new WhatsAppInteractiveHeaderMedia()
                {
                    Link = "a video URL link"
                }
            });
        }

        [Fact]
        public void DeserializeWhatsAppInteractiveTextHeader()
        {
            var json = Helpers.LoadResources("Conversation/Messages/WhatsAppInteractiveTextHeader.json");

            var result = DeserializeAsConversationClient<WhatsAppInteractiveTextHeader>(json);

            result.Should().BeEquivalentTo(new WhatsAppInteractiveTextHeader()
            {
                Text = "text header value"
            });
        }
    }


    public class OmniMessageTestData : IEnumerable<object[]>
    {
        private static readonly string Text = @"
            {
              ""WHATSAPP"": {
                  ""text_message"": {
                    ""text"": ""hello""
                  }
              }
            }";

        private static readonly string Media = @"
            {
              ""WHATSAPP"": {
                  ""media_message"": {
                    ""url"": ""https://hello.net""
                  }
              }
            }";

        private static readonly string Template = @"
            {
              ""WHATSAPP"": {
                  ""template_reference"": {
                    ""template_id"": ""id"",
                    ""version"": ""3""
                  }
              }
            }";

        private readonly List<object[]> _data = new()
        {
            new object[] { Text, new TextMessage("hello") },
            new object[]
            {
                Media, new MediaMessage()
                {
                    Url = "https://hello.net"
                }
            },
            new object[]
            {
                Template, new TemplateReference()
                {
                    TemplateId = "id",
                    Version = "3"
                }
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
