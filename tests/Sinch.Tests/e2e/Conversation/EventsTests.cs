using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.AppEvents;
using Sinch.Conversation.Messages;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class EventsTests : TestBase
    {
        [Theory]
        [ClassData(typeof(AppEvents))]
        public async Task SendEvent(AppEvent @event)
        {
            var response = await SinchClientMockServer.Conversation.Events.Send(new SendEventRequest
            {
                AppId = "app_id",
                Event = @event,
                Recipient = new ContactRecipient()
                {
                    ContactId = "Hey yo"
                },
                Queue = MessageQueue.HighPriority,
                EventMetadata = "meta",
                CallbackUrl = "url",
                ChannelPriorityOrder = new List<ConversationChannel>()
                {
                    ConversationChannel.Line,
                    ConversationChannel.Telegram
                }
            });
            response.Should().BeEquivalentTo(new SendEventResponse()
            {
                AcceptedTime = DateTime.Parse("2018-11-13T20:20:39+00:00", styles: DateTimeStyles.AssumeUniversal),
                EventId = "some_string_value"
            });
        }

        private class AppEvents : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var agent = new Agent()
                {
                    Type = AgentType.Human,
                    DisplayName = "name",
                    PictureUrl = "url"
                };
                yield return new object[] { new AppEvent(new ComposingEvent()) };
                yield return new object[]
                {
                    new AppEvent(new ComposingEndEvent())
                };
                yield return new object[]
                {
                    new AppEvent(new AgentJoinedEvent()
                    {
                        Agent = agent
                    })
                };
                yield return new object[]
                {
                    new AppEvent(new AgentLeftEvent()
                    {
                        Agent = agent
                    })
                };
                yield return new object[]
                {
                    new AppEvent(new GenericEvent()
                    {
                        Payload = new JsonObject()
                        {
                            ["data"] = "jojo"
                        }
                    })
                };
                yield return new object[]
                {
                    new AppEvent(new CommentReplyEvent()
                    {
                        Text = "hi"
                    })
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
