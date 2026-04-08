using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation.Common;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Sinch.Conversation.Events.Send;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class EventsTests : ConversationTestBase
    {
        private const string EventId1 = "CONTACT_EVENT1";
        private const string EventId2 = "CONTACT_EVENT2";

        private readonly string _baseEventsUrl =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/events";

        #region Send Tests

        [Fact]
        public async Task Send_WithValidRequest_ReturnsSendEventResponse()
        {
            var expectedRequest = new
            {
                app_id = "01W4FFL35P4NC4K35CONVAPP001",
                recipient = new { contact_id = "01W4FFL35P4NC4K35CONTACT001" },
                @event = new { composing_event = new { } }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_baseEventsUrl}:send")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    event_id = EventId1,
                    accepted_time = "2024-06-06T12:42:42.528Z"
                }));

            var response = await Conversation.Events.Send(new SendEventRequest
            {
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Recipient = new ContactRecipient { ContactId = "01W4FFL35P4NC4K35CONTACT001" },
                Event = new AppEvent(new ComposingEvent())
            });

            response.Should().NotBeNull();
            response.EventId.Should().Be(EventId1);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidEventId_ReturnsConversationEvent()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseEventsUrl}/{EventId1}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = EventId1,
                    channel_identity = new { },
                    processing_mode = "CONVERSATION"
                }));

            var response = await Conversation.Events.Get(EventId1);

            response.Should().NotBeNull();
            response.Id.Should().Be(EventId1);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidEventId_DeletesEventSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{_baseEventsUrl}/{EventId1}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new { }));

            await Conversation.Events.Delete(EventId1);
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithPageSize_ReturnsListEventsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseEventsUrl}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    events = new[]
                    {
                        new { id = EventId1, channel_identity = new { }, processing_mode = "CONVERSATION" },
                        new { id = EventId2, channel_identity = new { }, processing_mode = "CONVERSATION" }
                    },
                    next_page_token = "next_token_123"
                }));

            var response = await Conversation.Events.List(new ListEventsRequest { PageSize = 2 });

            response.Should().NotBeNull();
            response.Events.Should().HaveCount(2);
            response.NextPageToken.Should().Be("next_token_123");
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllEvents()
        {
            const string eventId3 = "CONTACT_EVENT2";

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseEventsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    events = new[]
                    {
                        new { id = EventId1, channel_identity = new { }, processing_mode = "CONVERSATION" },
                        new { id = EventId2, channel_identity = new { }, processing_mode = "CONVERSATION" }
                    },
                    next_page_token = "next_token_123"
                }));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseEventsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    events = new[]
                    {
                        new { id = eventId3, channel_identity = new { }, processing_mode = "CONVERSATION" }
                    },
                    next_page_token = ""
                }));

            var results = new List<ConversationEvent>();
            await foreach (var ev in Conversation.Events.ListAuto(new ListEventsRequest { PageSize = 2 }))
                results.Add(ev);

            results.Should().HaveCount(3);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion
    }
}
