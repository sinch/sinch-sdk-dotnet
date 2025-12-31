using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation.Messages
{
    public class ChoiceMessageTests : ConversationTestBase
    {
        [Fact]
        public void UrlMessage_ShouldSerializeCorrectly()
        {
            var urlMessage = new UrlMessage
            {
                Title = "Click here",
                Url = "https://example.com"
            };

            var json = SerializeAsConversationClient(urlMessage);

            Helpers.AssertJsonEqual("""{"title":"Click here","url":"https://example.com"}""", json);
        }

        [Fact]
        public void CallMessage_ShouldSerializeCorrectly()
        {
            var callMessage = new CallMessage
            {
                PhoneNumber = "+1234567890",
                Title = "Call us"
            };

            var json = SerializeAsConversationClient(callMessage);

            Helpers.AssertJsonEqual("""{"phone_number":"+1234567890","title":"Call us"}""", json);
        }

        [Fact]
        public void UrlMessage_ShouldDeserializeCorrectly()
        {
            var json = """{"title":"Click here","url":"https://example.com"}""";

            var urlMessage = DeserializeAsConversationClient<UrlMessage>(json);

            urlMessage.Should().BeEquivalentTo(new UrlMessage
            {
                Title = "Click here",
                Url = "https://example.com"
            });
        }

        [Fact]
        public void CallMessage_ShouldDeserializeCorrectly()
        {
            var json = """{"phone_number":"+1234567890","title":"Call us"}""";

            var callMessage = DeserializeAsConversationClient<CallMessage>(json);

            callMessage.Should().BeEquivalentTo(new CallMessage
            {
                PhoneNumber = "+1234567890",
                Title = "Call us"
            });
        }

        [Fact]
        public void UrlMessage_RoundTrip_ShouldPreserveProperties()
        {
            var original = new UrlMessage
            {
                Title = "Visit our site",
                Url = "https://sinch.com"
            };

            var json = SerializeAsConversationClient(original);
            var deserialized = DeserializeAsConversationClient<UrlMessage>(json);

            deserialized.Should().BeEquivalentTo(original);
        }

        [Fact]
        public void CallMessage_RoundTrip_ShouldPreserveProperties()
        {
            var original = new CallMessage
            {
                PhoneNumber = "+15551234567",
                Title = "Contact Support"
            };

            var json = SerializeAsConversationClient(original);
            var deserialized = DeserializeAsConversationClient<CallMessage>(json);

            deserialized.Should().BeEquivalentTo(original);
        }

        [Fact]
        public void Choice_WithUrlMessage_ShouldSerializeCorrectly()
        {
            var choice = new Choice
            {
                UrlMessage = new UrlMessage
                {
                    Title = "Learn more",
                    Url = "https://docs.sinch.com"
                },
                PostbackData = "url_choice"
            };

            var json = SerializeAsConversationClient(choice);

            var expectedJson = """
            {
                "url_message": {
                    "title": "Learn more",
                    "url": "https://docs.sinch.com"
                },
                "postback_data": "url_choice"
            }
            """;
            Helpers.AssertJsonEqual(expectedJson, json);
        }

        [Fact]
        public void Choice_WithCallMessage_ShouldSerializeCorrectly()
        {
            var choice = new Choice
            {
                CallMessage = new CallMessage
                {
                    PhoneNumber = "+18005551234",
                    Title = "Call Support"
                },
                PostbackData = "call_choice"
            };

            var json = SerializeAsConversationClient(choice);

            var expectedJson = """
            {
                "call_message": {
                    "phone_number": "+18005551234",
                    "title": "Call Support"
                },
                "postback_data": "call_choice"
            }
            """;
            Helpers.AssertJsonEqual(expectedJson, json);
        }

        [Fact]
        public void ChoiceMessage_WithMultipleChoiceTypes_ShouldSerializeCorrectly()
        {
            var choiceMessage = new ChoiceMessage
            {
                TextMessage = new TextMessage("Select an option:"),
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        UrlMessage = new UrlMessage
                        {
                            Title = "Website",
                            Url = "https://sinch.com"
                        },
                        PostbackData = "website"
                    },
                    new Choice
                    {
                        CallMessage = new CallMessage
                        {
                            PhoneNumber = "+18005551234",
                            Title = "Call Us"
                        },
                        PostbackData = "call"
                    },
                    new Choice
                    {
                        TextMessage = new TextMessage("Option 3"),
                        PostbackData = "option3"
                    }
                }
            };

            var json = SerializeAsConversationClient(choiceMessage);
            var deserialized = DeserializeAsConversationClient<ChoiceMessage>(json);

            deserialized.Should().BeEquivalentTo(choiceMessage);
        }
    }
}
