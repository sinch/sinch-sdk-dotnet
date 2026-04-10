using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
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
            var expectedRequest = new SendEventRequest
            {
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Recipient = new ContactRecipient { ContactId = "01W4FFL35P4NC4K35CONTACT001" },
                Event = new AppEvent(new ComposingEvent())
            };

            var expectedResponse = new SendEventResponse { EventId = EventId1 };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_baseEventsUrl}:send")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Events.Send(expectedRequest);

            response.Should().NotBeNull();
            response.EventId.Should().Be(EventId1);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidEventId_ReturnsConversationEvent()
        {
            var expectedResponse = new ConversationEvent
            {
                Id = EventId1,
                ChannelIdentity = new ChannelIdentity(),
                ProcessingMode = ProcessingMode.Conversation
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseEventsUrl}/{EventId1}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

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
                .Respond(HttpStatusCode.OK, new StringContent(string.Empty));

            await Conversation.Events.Delete(EventId1);
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithPageSize_ReturnsListEventsResponse()
        {
            var expectedResponse = new ListEventsResponse
            {
                Events = new List<ConversationEvent>
                {
                    new() { Id = EventId1, ChannelIdentity = new ChannelIdentity(), ProcessingMode = ProcessingMode.Conversation },
                    new() { Id = EventId2, ChannelIdentity = new ChannelIdentity(), ProcessingMode = ProcessingMode.Conversation }
                },
                NextPageToken = "next_token_123"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_baseEventsUrl}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Events.List(new ListEventsRequest { PageSize = 2 });

            response.Should().NotBeNull();
            response.Events.Should().HaveCount(2);
            response.NextPageToken.Should().Be("next_token_123");
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllEvents()
        {
            const string eventId3 = "CONTACT_EVENT3";

            var firstPage = new ListEventsResponse
            {
                Events = new List<ConversationEvent>
                {
                    new() { Id = EventId1, ChannelIdentity = new ChannelIdentity(), ProcessingMode = ProcessingMode.Conversation },
                    new() { Id = EventId2, ChannelIdentity = new ChannelIdentity(), ProcessingMode = ProcessingMode.Conversation }
                },
                NextPageToken = "next_token_123"
            };

            var secondPage = new ListEventsResponse
            {
                Events = new List<ConversationEvent>
                {
                    new() { Id = eventId3, ChannelIdentity = new ChannelIdentity(), ProcessingMode = ProcessingMode.Conversation }
                },
                NextPageToken = string.Empty
            };

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseEventsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    firstPage,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, _baseEventsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    secondPage,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var results = new List<ConversationEvent>();
            await foreach (var ev in Conversation.Events.ListAuto(new ListEventsRequest { PageSize = 2 }))
                results.Add(ev);

            results.Should().HaveCount(3);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion
    }
}
