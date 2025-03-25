using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
using Xunit;

namespace Sinch.Tests.Conversation.Messages
{
    public class AppMessageTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeAppMessageCarouselMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageCarouselMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new CarouselMessage()
            {
                Cards = new List<CardMessage>()
                {
                    new CardMessage()
                    {
                        Title = "title value",
                        Description = "description value",
                        MediaMessage = new MediaProperties()
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
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageChoiceMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageListMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
                },
                Media = new MediaProperties
                {
                    ThumbnailUrl = "another url",
                    Url = "an url value",
                    FilenameOverride = "filename override value"
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
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageLocationMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageMediaMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageTemplateMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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


        [Fact]
        public void DeserializeAppMessageContactInfoMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageContactInfoMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
        public void DeserializeAppMessageCardMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageCardMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new CardMessage()
            {
                Title = "title value",
                Description = "description value",
                MediaMessage = new MediaProperties()
                {
                    Url = "url value",
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
        public void DeserializeAppMessageTextMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageTextMessage.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

            result.Should().BeEquivalentTo(new AppMessage(new TextMessage("This is a text message."))
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
        public void DeserializeAppMessageListMessageProduct()
        {
            var json = Helpers.LoadResources("Conversation/Messages/AppMessage/AppMessageListMessageProduct.json");

            var result = DeserializeAsConversationClient<AppMessage>(json);

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
                            new ProductItem()
                            {
                                Id = "product ID value",
                                Marketplace = "marketplace value",
                                Quantity = 4,
                                ItemPrice = 3.14159f,
                                Currency = "currency value"
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
            }));
        }


        [Fact]
        public void DeserializeOrderDetailsChannelSpecificMessage()
        {
            var json = Helpers.LoadResources(
                "Conversation/Messages/ChannelSpecific/OrderDetailsChannelSpecificMessage.json");

            var result = DeserializeAsConversationClient<IChannelSpecificMessage>(json);

            result.As<PaymentOrderDetailsMessage>().Should().BeEquivalentTo(
                new PaymentOrderDetailsMessage()
                {
                    Message = new PaymentOrderDetailsChannelSpecificMessage
                    {
                        Header = new WhatsAppInteractiveDocumentHeader()
                        {
                            Document = new WhatsAppInteractiveHeaderMedia()
                            {
                                Link = "a document URL link"
                            }
                        },
                        Body = new WhatsAppInteractiveBody
                        {
                            Text = "Flow message body"
                        },
                        Footer = new WhatsAppInteractiveFooter
                        {
                            Text = "Flow message footer"
                        },
                        Payment = new PaymentOrderDetailsChannelSpecificMessagePayment
                        {
                            Type = PaymentOrderDetailsChannelSpecificMessagePayment.TypeEnum.Br,
                            ReferenceId = "a reference ID",
                            TypeOfGoods = TypeOfGoods.DigitalGoods,
                            PaymentSettings = new PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettings()
                            {
                                DynamicPix =
                                    new PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettingsDynamicPix()
                                    {
                                        Code = "code value",
                                        MerchantName = "merchant name",
                                        Key = "key value",
                                        KeyType =
                                            PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettingsDynamicPix
                                                .KeyTypeEnum.Cnpj
                                    }
                            },
                            TotalAmountValue = 1200,
                            Order = new PaymentOrderDetailsChannelSpecificMessagePaymentOrder()
                            {
                                CatalogId = "catalog id",
                                ExpirationTime = "1741934627",
                                ExpirationDescription = "expiration description",
                                SubtotalValue = 6000,
                                TaxValue = 7000,
                                TaxDescription = "tex description",
                                ShippingValue = 5000,
                                ShippingDescription = "shipping description",
                                DiscountValue = 1000,
                                DiscountDescription = "discount description",
                                DiscountProgramName = "discount program name",
                                Items = new List<PaymentOrderDetailsChannelSpecificMessagePaymentOrderItems>
                                {
                                    new()
                                    {
                                        RetailerId = "item retailer id",
                                        Name = "item name",
                                        AmountValue = 2000,
                                        Quantity = 3000,
                                        SaleAmountValue = 4000
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }

        [Fact]
        public void DeserializeOrderStatusChannelSpecificMessage()
        {
            var json = Helpers.LoadResources(
                "Conversation/Messages/ChannelSpecific/OrderStatusChannelSpecificMessage.json");

            var result = DeserializeAsConversationClient<IChannelSpecificMessage>(json);

            result.As<PaymentOrderStatusMessage>().Should().BeEquivalentTo(
                new PaymentOrderStatusMessage()
                {
                    Message = new PaymentOrderStatusChannelSpecificMessage
                    {
                        Header = new WhatsAppInteractiveDocumentHeader
                        {
                            Document = new WhatsAppInteractiveHeaderMedia
                            {
                                Link = "a document URL link"
                            }
                        },
                        Body = new WhatsAppInteractiveBody
                        {
                            Text = "Flow message body"
                        },
                        Footer = new WhatsAppInteractiveFooter
                        {
                            Text = "Flow message footer"
                        },
                        Payment = new PaymentOrderStatusChannelSpecificMessagePayment
                        {
                            ReferenceId = "order status reference id",
                            Order = new PaymentOrderStatusChannelSpecificMessagePaymentOrder
                            {
                                Status = PaymentOrderStatusChannelSpecificMessagePaymentOrder.StatusEnum.Canceled,
                                Description = "Order cancelled"
                            }
                        }
                    }
                }
            );
        }

        [Fact]
        public void DeserializeOrderDetailsChannelSpecificMessagePlain()
        {
            var json = Helpers.LoadResources(
                "Conversation/Messages/ChannelSpecific/OrderDetails.json");

            var result = DeserializeAsConversationClient<PaymentOrderDetailsChannelSpecificMessage>(json);

            result.Should().BeEquivalentTo(new PaymentOrderDetailsChannelSpecificMessage
                {
                    Header = new WhatsAppInteractiveDocumentHeader
                    {
                        Document = new WhatsAppInteractiveHeaderMedia
                        {
                            Link = "a document URL link"
                        }
                    },
                    Body = new WhatsAppInteractiveBody
                    {
                        Text = "Flow message body"
                    },
                    Footer = new WhatsAppInteractiveFooter
                    {
                        Text = "Flow message footer"
                    },
                    Payment = new PaymentOrderDetailsChannelSpecificMessagePayment
                    {
                        Type = PaymentOrderDetailsChannelSpecificMessagePayment.TypeEnum.Br,
                        ReferenceId = "a reference ID",
                        TypeOfGoods = TypeOfGoods.DigitalGoods,
                        PaymentSettings = new PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettings
                        {
                            DynamicPix =
                                new PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettingsDynamicPix
                                {
                                    Code = "code value",
                                    MerchantName = "merchant name",
                                    Key = "key value",
                                    KeyType =
                                        PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettingsDynamicPix
                                            .KeyTypeEnum.Cnpj
                                }
                        },
                        TotalAmountValue = 1200,
                        Order = new PaymentOrderDetailsChannelSpecificMessagePaymentOrder
                        {
                            CatalogId = "catalog id",
                            ExpirationTime = "1741934627",
                            ExpirationDescription = "expiration description",
                            SubtotalValue = 6000,
                            TaxValue = 7000,
                            TaxDescription = "tex description",
                            ShippingValue = 5000,
                            ShippingDescription = "shipping description",
                            DiscountValue = 1000,
                            DiscountDescription = "discount description",
                            DiscountProgramName = "discount program name",
                            Items = new List<PaymentOrderDetailsChannelSpecificMessagePaymentOrderItems>
                            {
                                new PaymentOrderDetailsChannelSpecificMessagePaymentOrderItems
                                {
                                    RetailerId = "item retailer id",
                                    Name = "item name",
                                    AmountValue = 2000,
                                    Quantity = 3000,
                                    SaleAmountValue = 4000
                                }
                            }
                        }
                    }
                }
            );
        }
    }
}
