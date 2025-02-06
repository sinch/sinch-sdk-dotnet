using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Sinch.Core;
using Sinch.Numbers;
using Xunit;

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
            response.Direction.Should().Be(ConversationDirection.UndefinedDirection);
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
                .WithQueryString("message_source", "DISPATCH_SOURCE")
                .WithQueryString("only_recipient_originated", "true")
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
                MessageSource = MessageSource.DispatchSource,
                OnlyRecipientOriginated = true
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
                direction = "UNDEFINED_DIRECTION",
                id = "string",
                metadata = "string",
                injected = true
            };
            return responseObj;
        }

        [Fact]
        public void SerializeBirthDate()
        {
            // the birthday format is YYYY-MM-DD
            var t = @"{ ""birthday"": ""2000-03-12"", ""name"": { ""full_name"": ""AAA""}, ""phone_numbers"":[] }";

            var contact = JsonSerializer.Deserialize<ContactInfoMessage>(t);

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
            val.Should().BeValidJson().And.BeEquivalentTo(FlowsRawJson);
        }

        [Fact]
        public void DeserializeFlowMessage()
        {
            var json = $"{{\"WHATSAPP\":{FlowsRawJson}}}";
            var dict = JsonSerializer.Deserialize<Dictionary<ConversationChannel, IChannelSpecificMessage>>(json);
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
        public void DeserializeChannelSpecificNfmReplyMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ChannelSpecificContactMessageNfmReply.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new ChannelSpecificContactMessage()
            {
                MessageType = ChannelSpecificMessageType.NfmReply,
                Message = new ChannelSpecificMessageContent()
                {
                    Type = ChannelSpecificMessageType.NfmReply,
                    NfmReply = new WhatsAppInteractiveNfmReply()
                    {
                        Name = WhatsAppInteractiveNfmReply.NameEnum.AddressMessage,
                        Body = "nfm reply body value",
                        ResponseJson = "{\"key\": \"value\"}"
                    }
                }
            }));
        }

        [Fact]
        public void DeserializeAppMessageContactInfoMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactInfoMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new ContactInfoMessage()
            {
                Name = new NameInfo()
                {
                    FullName = "full_name value",
                    FirstName = "first_name value",
                    LastName = "last_name value",
                    MiddleName = "middle_name value",
                    Prefix = "prefix value",
                    Suffix = "suffix value"
                },
                PhoneNumbers = new List<PhoneNumberInfo>()
                {
                    new PhoneNumberInfo()
                    {
                        PhoneNumber = "phone_number value",
                        Type = "type value"
                    }
                },
                Addresses = new List<AddressInfo>()
                {
                    new AddressInfo()
                    {
                        City = "city value",
                        Country = "country value",
                        State = "state va@lue",
                        Zip = "zip value",
                        CountryCode = "country_code value"
                    }
                },
                EmailAddresses = new List<EmailInfo>()
                {
                    new EmailInfo()
                    {
                        EmailAddress = "email_address value",
                        Type = "type value"
                    }
                },
                Organization = new OrganizationInfo()
                {
                    Company = "company value",
                    Department = "department value",
                    Title = "title value"
                },
                Urls = new List<UrlInfo>()
                {
                    new UrlInfo()
                    {
                        Url = "url value",
                        Type = "type value"
                    }
                },
                Birthday = new DateTime(1968, 07, 07)
            }));
        }

        [Fact]
        public void DeserializeWhatsAppInteractiveHeader()
        {
            var json = Helpers.LoadResources("Conversation/Messages/WhatsAppInteractiveHeader.json");

            var result = JsonSerializer.Deserialize<WhatsAppInteractiveImageHeader>(json);

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

            var result = JsonSerializer.Deserialize<WhatsAppInteractiveDocumentHeader>(json);

            result.Should().BeEquivalentTo(new WhatsAppInteractiveDocumentHeader()
            {
                Document = new WhatsAppInteractiveHeaderMedia()
                {
                    Link = "a document URL link"
                }
            });
        }

        [Fact]
        public void DeserializeContactMessageProductResponseMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageProductResponseMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new ProductResponseMessage()
            {
                Products = new List<ProductItem>()
                {
                    new ProductItem()
                    {
                        Id = "product ID value",
                        Marketplace = "marketplace value",
                        Quantity = 4,
                        ItemPrice = 3.14159f,
                        Currency = "currency value"
                    }
                },
                Title = "a product response message title value",
                CatalogId = "catalog id value"
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageChoiceMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageChoiceResponseMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new ChoiceResponseMessage()
            {
                MessageId = "message id value",
                PostbackData = "postback data value"
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageFallbackMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageFallbackMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new FallbackMessage()
            {
                RawMessage = "raw message value",
                Reason = new Reason()
                {
                    Code = "RECIPIENT_NOT_OPTED_IN",
                    Description = "reason description",
                    SubCode = "UNSPECIFIED_SUB_CODE"
                }
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageLocationMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageLocationMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new LocationMessage()
            {
                Label = "label value",
                Title = "title value",
                Coordinates = new Coordinates(47.6279809f, -2.8229159f)
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageMediaCardMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageMediaCardMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new MediaCardMessage()
            {
                Caption = "caption value",
                Url = "an url value",
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageMediaMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageMediaMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new MediaMessage()
            {
                ThumbnailUrl = "another url",
                FilenameOverride = "filename override value",
                Url = "an url value",
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageTextMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessageTextMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new TextMessage("This is a text message.")
            {
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeAppMessageCarouselMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageCarouselMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new CarouselMessage()
            {
                Cards = new List<CardMessage>()
                {
                    new CardMessage()
                    {
                        Title = "title value",
                        Description = "description value",
                        MediaMessage = new MediaMessage()
                        {
                            Url = "url value"
                        },
                        Height = CardHeight.Medium,
                        Choices = new List<Choice>()
                        {
                            new Choice()
                            {
                                TextMessage = new TextMessage("This is a text message."),
                                PostbackData = "postback_data text"
                            },
                            new Choice()
                            {
                                CallMessage = new CallMessage()
                                {
                                    Title = "title value",
                                    PhoneNumber = "phone number value"
                                },
                                PostbackData = "postback_data call"
                            },
                            new Choice()
                            {
                                LocationMessage = new LocationMessage()
                                {
                                    Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                    Title = "title value",
                                    Label = "label value"
                                },
                                PostbackData = "postback_data location"
                            },
                            new Choice()
                            {
                                UrlMessage = new UrlMessage()
                                {
                                    Title = "title value",
                                    Url = "an url value"
                                },
                                PostbackData = "postback_data url"
                            }
                        }
                    }
                },
                Choices = new List<Choice>()
                {
                    new Choice()
                    {
                        CallMessage = new CallMessage()
                        {
                            Title = "title value",
                            PhoneNumber = "phone number value"
                        },
                        PostbackData = "postback call_message data value"
                    },
                    new Choice()
                    {
                        LocationMessage = new LocationMessage()
                        {
                            Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                            Title = "title value",
                            Label = "label value"
                        },
                        PostbackData = "postback location_message data value"
                    },
                    new Choice()
                    {
                        TextMessage = new TextMessage("This is a text message."),
                        PostbackData = "postback text_message data value"
                    },
                    new Choice()
                    {
                        UrlMessage = new UrlMessage()
                        {
                            Title = "title value",
                            Url = "an url value"
                        },
                        PostbackData = "postback url_message data value"
                    }
                }
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk, new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                        Title = "title value",
                                        Label = "label value"
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
            });
        }

        [Fact]
        public void DeserializeAppMessageChoiceMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageChoiceMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new ChoiceMessage()
            {
                TextMessage = new TextMessage("This is a text message."),
                Choices = new List<Choice>()
                {
                    new Choice()
                    {
                        CallMessage = new CallMessage()
                        {
                            Title = "title value",
                            PhoneNumber = "phone number value"
                        },
                        PostbackData = "postback call_message data value"
                    },
                    new Choice()
                    {
                        LocationMessage = new LocationMessage()
                        {
                            Title = "title value",
                            Label = "label value",
                            Coordinates = new Coordinates(47.6279809f, -2.8229159f)
                        },
                        PostbackData = "postback location_message data value"
                    },
                    new Choice()
                    {
                        TextMessage = new TextMessage("This is a text message."),
                        PostbackData = "postback text_message data value"
                    },
                    new Choice()
                    {
                        UrlMessage = new UrlMessage()
                        {
                            Title = "title value",
                            Url = "an url value"
                        },
                        PostbackData = "postback url_message data value"
                    }
                }
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk,
                        new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Title = "title value",
                                        Label = "label value",
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f)
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
            });
        }

        [Fact]
        public void DeserializeAppMessageListMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageListMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new ListMessage()
            {
                Title = "a list message title value",
                Sections = new List<ListSection>()
                {
                    new ListSection()
                    {
                        Title = "a list section title value",
                        Items = new List<IListItem>()
                        {
                            new ChoiceItem()
                            {
                                Title = "choice title",
                                Description = "description value",
                                Media = new MediaMessage()
                                {
                                    Url = "an url value",
                                    ThumbnailUrl = "another url",
                                    FilenameOverride = "filename override value"
                                },
                                PostbackData = "postback value"
                            }
                        }
                    }
                },
                Description = "description value",
                MessageProperties = new ListMessageMessageProperties()
                {
                    CatalogId = "catalog ID value",
                    Menu = "menu value"
                }
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk,
                        new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Title = "title value",
                                        Label = "label value",
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f)
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
            });
        }

        [Fact]
        public void DeserializeAppMessageLocationMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageLocationMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new LocationMessage()
            {
                Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                Title = "title value",
                Label = "label value"
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk,
                        new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                        Title = "title value",
                                        Label = "label value"
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
            });
        }

        [Fact]
        public void DeserializeAppMessageMediaMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageMediaMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new MediaMessage()
            {
                Url = "an url value",
                ThumbnailUrl = "another url",
                FilenameOverride = "filename override value"
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk,
                        new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                        Title = "title value",
                                        Label = "label value"
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
            });
        }

        [Fact]
        public void DeserializeAppMessageTemplateMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessageTemplateMessage.json");

            var result = JsonSerializer.Deserialize<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new TemplateMessage()
            {
                ChannelTemplate = new Dictionary<ConversationChannel, TemplateReference>
                {
                    {
                        ConversationChannel.KakaoTalk, new TemplateReference()
                        {
                            TemplateId = "my template ID value",
                            Version = "a version",
                            LanguageCode = "en-US"
                        }
                    }
                },
                OmniTemplate = new TemplateReference()
                {
                    TemplateId = "another template ID",
                    Version = "another version",
                    LanguageCode = "another language",
                    Parameters = new Dictionary<string, string>()
                    {
                        {
                            "name",
                            "Value for the name parameter used in the version 1 and language \"en-US\" of the template"
                        }
                    }
                }
            })
            {
                ExplicitChannelMessage = new Dictionary<ConversationChannel, string>
                {
                    { ConversationChannel.KakaoTalk, "foo value" }
                },
                ExplicitChannelOmniMessage = new Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>
                {
                    {
                        ChannelSpecificTemplate.KakaoTalk,
                        new ChoiceMessage()
                        {
                            TextMessage = new TextMessage("This is a text message."),
                            Choices = new List<Choice>()
                            {
                                new Choice()
                                {
                                    CallMessage = new CallMessage()
                                    {
                                        Title = "title value",
                                        PhoneNumber = "phone number value"
                                    },
                                    PostbackData = "postback call_message data value"
                                },
                                new Choice()
                                {
                                    LocationMessage = new LocationMessage()
                                    {
                                        Coordinates = new Coordinates(47.6279809f, -2.8229159f),
                                        Title = "title value",
                                        Label = "label value"
                                    },
                                    PostbackData = "postback location_message data value"
                                },
                                new Choice()
                                {
                                    TextMessage = new TextMessage("This is a text message."),
                                    PostbackData = "postback text_message data value"
                                },
                                new Choice()
                                {
                                    UrlMessage = new UrlMessage()
                                    {
                                        Title = "title value",
                                        Url = "an url value"
                                    },
                                    PostbackData = "postback url_message data value"
                                }
                            }
                        }
                    }
                },
                ChannelSpecificMessage = new Dictionary<ConversationChannel, IChannelSpecificMessage>()
                {
                    {
                        ConversationChannel.Messenger, new FlowMessage()
                        {
                            Message = new FlowChannelSpecificMessage()
                            {
                                FlowId = "1",
                                FlowCta = "Book!",
                                Header = new WhatsAppInteractiveTextHeader()
                                {
                                    Text = "text header value"
                                },
                                Body = new WhatsAppInteractiveBody()
                                {
                                    Text = "Flow message body"
                                },
                                Footer = new WhatsAppInteractiveFooter()
                                {
                                    Text = "Flow message footer"
                                },
                                FlowToken = "AQAAAAACS5FpgQ_cAAAAAD0QI3s.",
                                FlowMode = FlowChannelSpecificMessage.FlowModeType.Draft,
                                FlowAction = FlowChannelSpecificMessage.FlowActionType.Navigate,
                                FlowActionPayload = new FlowChannelSpecificMessageFlowActionPayload()
                                {
                                    Screen = "<SCREEN_NAME>",
                                    Data = new JsonObject()
                                    {
                                        ["product_name"] = "name",
                                        ["product_description"] = "description",
                                        ["product_price"] = 100
                                    }
                                }
                            }
                        }
                    }
                },
                Agent = new Agent()
                {
                    DisplayName = "display_name value",
                    Type = AgentType.Bot,
                    PictureUrl = "picture_url value"
                }
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
