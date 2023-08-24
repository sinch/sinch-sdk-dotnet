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
    }
}
