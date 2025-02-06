using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation.Messages
{
    public class ContactMessageTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeChannelSpecificNfmReplyMessage()
        {
            var json = Helpers.LoadResources(
                "Conversation/Messages/ContactMessage/ContactMessageChannelSpecificContactMessageNfmReply.json");

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
        public void DeserializeContactMessageProductResponseMessage()
        {
            var json = Helpers.LoadResources(
                "Conversation/Messages/ContactMessage/ContactMessageProductResponseMessage.json");

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
            var json = Helpers.LoadResources(
                "Conversation/Messages/ContactMessage/ContactMessageChoiceResponseMessage.json");

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
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessage/ContactMessageFallbackMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new FallbackMessage()
            {
                RawMessage = "raw message value",
                Reason = new Reason()
                {
                    Code = ReasonCode.RecipientNotOptedIn,
                    Description = "reason description",
                    SubCode = ReasonSubCode.UnspecifiedSubCode
                }
            })
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }

        [Fact]
        public void DeserializeContactMessageLocationMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessage/ContactMessageLocationMessage.json");

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
            var json = Helpers.LoadResources(
                "Conversation/Messages/ContactMessage/ContactMessageMediaCardMessage.json");

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
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessage/ContactMessageMediaMessage.json");

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
            var json = Helpers.LoadResources("Conversation/Messages/ContactMessage/ContactMessageTextMessage.json");

            var result = JsonSerializer.Deserialize<ContactMessage>(json);

            result.Should().BeEquivalentTo(new ContactMessage(new TextMessage("This is a text message."))
            {
                ReplyTo = new ReplyTo("message id value")
            });
        }
    }
}
