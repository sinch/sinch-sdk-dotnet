using System;
using FluentAssertions;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.Conversation.Messages
{
    public class ChoiceTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeChoiceCalendarMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/Choice/ChoiceCalendarMessage.json");

            var result = DeserializeAsConversationClient<Choice>(json);

            result.Should().BeEquivalentTo(new Choice
            {
                CalendarMessage = new CalendarMessage
                {
                    Title = "Add to calendar",
                    EventStart = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
                    EventEnd = new DateTime(2024, 1, 15, 11, 0, 0, DateTimeKind.Utc),
                    EventTitle = "Team Meeting",
                    EventDescription = "Weekly team sync",
                    FallbackUrl = "https://calendar.google.com/event"
                },
                PostbackData = "postback calendar_message data value"
            });
        }

        [Fact]
        public void DeserializeChoiceShareLocationMessage()
        {
            var json = Helpers.LoadResources("Conversation/Messages/Choice/ChoiceShareLocationMessage.json");

            var result = DeserializeAsConversationClient<Choice>(json);

            result.Should().BeEquivalentTo(new Choice
            {
                ShareLocationMessage = new ShareLocationMessage
                {
                    Title = "Share your location",
                    FallbackUrl = "https://maps.google.com"
                },
                PostbackData = "postback share_location_message data value"
            });
        }

        [Fact]
        public void SerializeChoiceCalendarMessage()
        {
            var choice = new Choice
            {
                CalendarMessage = new CalendarMessage
                {
                    Title = "Add to calendar",
                    EventStart = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
                    EventEnd = new DateTime(2024, 1, 15, 11, 0, 0, DateTimeKind.Utc),
                    EventTitle = "Team Meeting",
                    EventDescription = "Weekly team sync",
                    FallbackUrl = "https://calendar.google.com/event"
                },
                PostbackData = "postback calendar_message data value"
            };

            var actual = SerializeAsConversationClient(choice);

            Helpers.AssertJsonEqual(
                Helpers.LoadResources("Conversation/Messages/Choice/ChoiceCalendarMessage.json"), actual);
        }

        [Fact]
        public void SerializeChoiceShareLocationMessage()
        {
            var choice = new Choice
            {
                ShareLocationMessage = new ShareLocationMessage
                {
                    Title = "Share your location",
                    FallbackUrl = "https://maps.google.com"
                },
                PostbackData = "postback share_location_message data value"
            };

            var actual = SerializeAsConversationClient(choice);

            Helpers.AssertJsonEqual(
                Helpers.LoadResources("Conversation/Messages/Choice/ChoiceShareLocationMessage.json"), actual);
        }
    }
}
