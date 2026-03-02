using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation.Conversations.InjectEvent;
using Sinch.Conversation.Events;
using Sinch.Conversation.Events.EventTypes;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class ConversationInjectEventTests : ConversationTestBase
    {
        private const string ConversationId = "CONV001";
        private readonly string _url =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/conversations/{ConversationId}:inject-event";

        [Fact]
        public async Task InjectEvent_WithComposingEvent_ReturnsEventIdAndAcceptedTime()
        {
            var acceptTime = new DateTime(2024, 6, 6, 15, 15, 15, DateTimeKind.Utc);

            var expectedRequest = new
            {
                app_event = new
                {
                    composing_event = new { }
                },
                accept_time = acceptTime
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, _url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    event_id = "01W4FFL35P4NC4K35CONVEVENT1",
                    accepted_time = "2025-06-06T15:15:15.000Z"
                }));

            var injectEventRequest = new InjectEventRequest(
                new AppEvent(new ComposingEvent()))
                {
                    ConversationId = ConversationId,
                    AcceptTime = acceptTime
                };
            
            var response = await Conversation.Conversations.InjectEvent(injectEventRequest);

            response.Should().NotBeNull();
            response.EventId.Should().Be("01W4FFL35P4NC4K35CONVEVENT1");
            response.AcceptedTime.Should().Be(new DateTime(2025, 6, 6, 15, 15, 15, DateTimeKind.Utc));
        }
    }
}
