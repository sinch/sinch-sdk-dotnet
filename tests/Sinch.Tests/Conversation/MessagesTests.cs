using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Send;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class MessagesTests : ConversationTestBase
    {
        private readonly dynamic _baseMessageExpected = new ExpandoObject();

        private readonly Request _baseRequest = new Request
        {
            AppId = "123",
            Message = new AppMessage(new TextMessage("I'm a texter"))
            {
                ExplicitChannelMessage = null,
                AdditionalProperties = null
            },
            Recipient = new Contact()
            {
                ContactId = "ContactEasy"
            }
        };

        public MessagesTests()
        {
            _baseMessageExpected.app_id = "123";
            _baseMessageExpected.recipient = new
            {
                contact_id = "ContactEasy"
            };
            _baseMessageExpected.message = new ExpandoObject();
        }


        private readonly string _sendUrl =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/messages:send";

        private readonly JsonContent _baseSendResponse = JsonContent.Create(new
        {
            accepted_time = "2019-08-24T14:15:22Z",
            message_id = "string"
        });

        [Fact]
        public async Task SendText()
        {
            _baseMessageExpected.message.text_message = new
            {
                text = "I'm a texter"
            };
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.MessageId.Should().Be("string");
            response.AcceptedTime.Should().HaveMinute(15);
        }

        [Fact]
        public async Task SendLocation()
        {
            _baseMessageExpected.message.location_message = new
            {
                label = "label",
                title = "title",
                coordinates = new
                {
                    latitude = 3.18f,
                    longitude = 4.20f,
                }
            };
            _baseRequest.Message = new AppMessage(new LocationMessage
            {
                Coordinates = new Coordinates(3.18f, 4.20f),
                Label = "label",
                Title = "title"
            });

            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendCarousel()
        {
            _baseMessageExpected.message.carousel_message = new
            {
                cards = new[]
                {
                    new
                    {
                        description = "card description",
                        title = "Title Card",
                        height = "TALL",
                        media_message = new
                        {
                            caption = "cap",
                            url = "https://localmob"
                        },
                        choices = new[]
                        {
                            new
                            {
                                call_message = new
                                {
                                    phone_number = "123",
                                    title = "Jhon"
                                }
                            }
                        }
                    }
                },
                choices = new[]
                {
                    new
                    {
                        text_message = new
                        {
                            text = "123",
                        }
                    }
                }
            };
            _baseRequest.Message = new AppMessage(new CarouselMessage()
            {
                Cards = new List<CardMessage>()
                {
                    new()
                    {
                        Description = "card description",
                        Title = "Title Card",
                        Height = CardHeight.Tall,
                        MediaMessage = new MediaCarouselMessage()
                        {
                            Caption = "cap",
                            Url = new Uri("https://localmob"),
                        },
                        Choices = new List<Choice>
                        {
                            new Choice
                            {
                                CallMessage = new("123", "Jhon"),
                            }
                        }
                    }
                },
                Choices = new List<Choice>()
                {
                    new Choice()
                    {
                        TextMessage = new TextMessage("123")
                    }
                }
            });
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendChoice()
        {
            _baseMessageExpected.message.choice_message = new
            {
                choices = new[]
                {
                    new
                    {
                        text_message = new
                        {
                            text = "123",
                        },
                        postback_data = "postback",
                    }
                },
                text_message = new
                {
                    text = "123",
                }
            };
            _baseRequest.Message = new AppMessage(new ChoiceMessage()
            {
                Choices = new List<Choice>()
                {
                    new Choice()
                    {
                        TextMessage = new TextMessage("123"),
                        PostbackData = "postback"
                    }
                },
                TextMessage = new TextMessage("123")
            });
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendMedia()
        {
            _baseMessageExpected.message.media_message = new
            {
                url = "http://yup/ls",
                thumbnail_url = "https://img.c",
            };
            _baseRequest.Message = new AppMessage(new MediaMessage
            {
                Url = new Uri("http://yup/ls"),
                ThumbnailUrl = new Uri("https://img.c")
            });
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendTemplate()
        {
            _baseMessageExpected.message.template_message = new
            {
                omni_template = new
                {
                    language_code = "es",
                    template_id = "tempid",
                    version = "1.0",
                    parameters = new
                    {
                        key = "val"
                    }
                },
                channel_template = new
                {
                    test = new
                    {
                        template_id = "abc",
                        version = "305",
                        parameters = new
                        {
                            tarnished = "order"
                        },
                        language_code = "de",
                    }
                }
            };
            _baseRequest.Message = new AppMessage(new TemplateMessage()
            {
                OmniTemplate = new TemplateReference
                {
                    LanguageCode = "es",
                    Parameters = new Dictionary<string, string>()
                    {
                        { "key", "val" }
                    },
                    TemplateId = "tempid",
                    Version = "1.0"
                },
                ChannelTemplate = new Dictionary<string, TemplateReference>()
                {
                    {
                        "test", new TemplateReference
                        {
                            TemplateId = "abc",
                            Version = "305",
                            Parameters = new Dictionary<string, string>()
                            {
                                { "tarnished", "order" }
                            },
                            LanguageCode = "de"
                        }
                    }
                }
            });
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendList()
        {
            _baseMessageExpected.message.list_message = new
            {
                title = "list_title",
                description = "description",
                message_properties = new
                {
                    menu = "omenu",
                    catalog_id = "id1"
                },
                sections = new[]
                {
                    new
                    {
                        title = "item1",
                        items = new dynamic[]
                        {
                            new
                            {
                                title = "listitemchoice",
                                postback_data = "postno",
                                description = "desc",
                                media = new
                                {
                                    url = "https://nolocalhost",
                                    thumbnail_url = "https://knowyourmeme.com/photos/377946"
                                }
                            },
                            new
                            {
                                id = "prod_id",
                                marketplace = "amazon",
                                currency = "eur",
                                quantity = 20,
                                item_price = 12.18f,
                            }
                        }
                    }
                }
            };
            _baseRequest.Message = new AppMessage(new ListMessage
            {
                Title = "list_title",
                Description = "description",
                Sections = new List<ListSection>()
                {
                    new ListSection()
                    {
                        Title = "item1",
                        Items = new List<IListItem>()
                        {
                            new ListItemChoice()
                            {
                                Title = "listitemchoice",
                                PostbackData = "postno",
                                Description = "desc",
                                Media = new MediaMessage()
                                {
                                    Url = new Uri("https://nolocalhost"),
                                    ThumbnailUrl = new Uri("https://knowyourmeme.com/photos/377946")
                                }
                            },
                            new ListItemProduct
                            {
                                Id = "prod_id",
                                Marketplace = "amazon",
                                Currency = "eur",
                                Quantity = 20,
                                ItemPrice = 12.18f,
                            }
                        }
                    }
                },
                MessageProperties = new ListMessageMessageProperties()
                {
                    Menu = "omenu",
                    CatalogId = "id1"
                }
            });
            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task SendAllParams()
        {
            _baseMessageExpected.message.text_message = new
            {
                text = "I'm a texter"
            };
            _baseMessageExpected.recipient = new
            {
                identified_by = new
                {
                    channel_identities = new[]
                    {
                        new
                        {
                            identity = "identity",
                            channel = "MMS",
                        }
                    }
                }
            };
            _baseMessageExpected.callback_url = "http://callback";
            _baseMessageExpected.channel_priority_order = new[] { "INSTAGRAM", "TELEGRAM" };
            _baseMessageExpected.correlation_id = "cor_id";
            _baseMessageExpected.processing_strategy = "DISPATCH_ONLY";
            _baseMessageExpected.ttl = "1800s";
            _baseMessageExpected.queue = "HIGH_PRIORITY";
            _baseMessageExpected.message_metadata = "meta";

            _baseRequest.Message = new AppMessage(new TextMessage("I'm a texter"));
            _baseRequest.Recipient = new Identified
            {
                IdentifiedBy = new IdentifiedBy()
                {
                    ChannelIdentities = new List<ChannelIdentity>()
                    {
                        new ChannelIdentity
                        {
                            Identity = "identity",
                            Channel = ConversationChannel.Mms
                        }
                    }
                }
            };
            _baseRequest.CallbackUrl = new Uri("http://callback");
            _baseRequest.ChannelPriorityOrder = new List<ConversationChannel>()
            {
                ConversationChannel.Instagram, ConversationChannel.Telegram
            };
            _baseRequest.CorrelationId = "cor_id";
            _baseRequest.ProcessingStrategy = ProcessingStrategy.DispatchOnly;
            _baseRequest.Ttl = "1800s";
            _baseRequest.Queue = MessageQueue.HighPriority;
            _baseRequest.MessageMetadata = "meta";

            HttpMessageHandlerMock
                .When(HttpMethod.Post,
                    _sendUrl)
                .WithJson(JsonConvert.SerializeObject(_baseMessageExpected as object))
                .Respond(HttpStatusCode.OK, _baseSendResponse);

            var response = await Conversation.Messages.Send(_baseRequest);

            response.Should().NotBeNull();
        }
    }
}
