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
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Events.Send;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class EventsTests : TestBase
    {
        private readonly ConversationEvent _conversationEvent = new ConversationEvent
        {
            Id = "123",
            Direction = ConversationDirection.ToContact,
            Event = new ConversationEventEvent(new ContactMessageEvent(new PaymentStatusUpdateEvent()
            {
                PaymentStatus = PaymentStatus.PaymentStatusCaptured,
                ReferenceId = "refid",
                PaymentTransactionId = "transid",
                PaymentTransactionStatus = PaymentTransactionStatus.PaymentStatusTransactionSuccess
            })),
            ConversationId = "conversation_id",
            ContactId = "contact_id",
            ChannelIdentity = new ChannelIdentity
            {
                Identity = "a",
                Channel = ConversationChannel.Sms
            },
            AcceptTime = DateTime.Parse("2018-11-13T20:20:39+00:00", styles: DateTimeStyles.AssumeUniversal),
            ProcessingMode = ProcessingMode.Conversation
        };

        [Theory(Skip = "Wait for doppelganger to merge updates OAS file")]
        [ClassData(typeof(AppEvents))]
        public async Task SendEvents(AppEvent @event)
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

        [Fact]
        public async Task SendEvent()
        {
            var response = await SinchClientMockServer.Conversation.Events.Send(new SendEventRequest
            {
                AppId = "app_id",
                Event = new AppEvent(new AgentJoinedEvent()),
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

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.Events.Get("123");
            response.Should().BeEquivalentTo(_conversationEvent);
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockServer.Conversation.Events.List(new ListEventsRequest()
            {
                ConversationId = "conv_id",
                ContactId = "contact_id",
                PageSize = 10,
                PageToken = "abc"
            });
            response.Should().BeEquivalentTo(new ListEventsResponse()
            {
                Events = new List<ConversationEvent>() { _conversationEvent, _conversationEvent },
                NextPageToken = "def"
            });
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.Events.Delete("123");
            await op.Should().NotThrowAsync();
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
