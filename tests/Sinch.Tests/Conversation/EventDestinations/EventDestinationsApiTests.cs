using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.EventDestinations;
using Xunit;

namespace Sinch.Tests.Conversation.EventDestinations
{
    public class EventDestinationsApiTests : ConversationTestBase
    {
        private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
        private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
        private const string EventDestinationId001 = "01W4FFL35P4NC4K35EVENTDESTINATION001";
        private const string EventDestinationId004 = "01W4FFL35P4NC4K35EVENTDESTINATION004";
        private const string EventDestinationsBaseUrl = $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/webhooks";

        [Fact]
        public async Task Create_WithValidRequest_ReturnsEventDestination()
        {
            var expectedRequest = new CreateEventDestinationRequest
            {
                AppId = AppId001,
                Target = "https://my-callback-server.com/capability",
                Triggers = [EventDestinationTrigger.Capability],
                Secret = "CactusKnight_SurfsWaves",
                TargetType = EventDestinationTargetType.Http
            };

            var expectedResponse = new EventDestination
            {
                Id = EventDestinationId004,
                AppId = AppId001,
                Target = "https://my-callback-server.com/capability",
                TargetType = EventDestinationTargetType.Http,
                Secret = "CactusKnight_SurfsWaves",
                Triggers = [EventDestinationTrigger.Capability]
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, EventDestinationsBaseUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.EventDestinations.Create(expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(EventDestinationId004);
        }

        [Fact]
        public async Task List_WithValidAppId_ReturnsEventDestinations()
        {
            var expectedResponse = new ListEventDestinationsResponse
            {
                EventDestinations =
                new List<EventDestination>
                {
                    new EventDestination
                    {
                        Id = EventDestinationId001,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/unsupported",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "VeganVampire_SipsTea",
                        Triggers = [EventDestinationTrigger.Unsupported],
                        ClientCredentials = new ClientCredentials
                        {
                            Endpoint = "https://my-auth-server.com/oauth2/token",
                            ClientId = "webhook-username",
                            ClientSecret = "webhook-password"
                        }
                    },
                    new EventDestination
                    {
                        Id = EventDestinationId004,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/capability",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "CactusKnight_SurfsWaves",
                        Triggers = [EventDestinationTrigger.Capability]
                    }
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/{AppId001}/webhooks")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.EventDestinations.List(AppId001);

            response.Should().NotBeNull();
            response.EventDestinations.Should().HaveCount(2);
            response.EventDestinations!.First().Id.Should().Be(EventDestinationId001);
        }

        [Fact]
        public async Task ListAuto_WithValidAppId_IteratesThroughAllEventDestinations()
        {
            var expectedResponse = new ListEventDestinationsResponse
            {
                EventDestinations =
                new List<EventDestination>
                {
                    new EventDestination
                    {
                        Id = EventDestinationId001,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/unsupported",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "VeganVampire_SipsTea",
                        Triggers = [EventDestinationTrigger.Unsupported]
                    },
                    new EventDestination
                    {
                        Id = EventDestinationId004,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/capability",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "CactusKnight_SurfsWaves",
                        Triggers = [EventDestinationTrigger.Capability]
                    }
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/{AppId001}/webhooks")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = new List<EventDestination>();
            await foreach (var eventDestination in Conversation.EventDestinations.ListAuto(AppId001))
            {
                response.Add(eventDestination);
            }

            response.Should().HaveCount(2);
            response[0].Id.Should().Be(EventDestinationId001);
        }

        [Fact]
        public async Task Get_WithValidEventDestinationId_ReturnsEventDestination()
        {
            var expectedResponse = new EventDestination
            {
                Id = EventDestinationId001,
                AppId = AppId001,
                Target = "https://my-callback-server.com/unsupported",
                TargetType = EventDestinationTargetType.Http,
                Secret = "VeganVampire_SipsTea",
                Triggers = [EventDestinationTrigger.Unsupported],
                ClientCredentials = new ClientCredentials
                {
                    Endpoint = "https://my-auth-server.com/oauth2/token",
                    ClientId = "webhook-username",
                    ClientSecret = "webhook-password"
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{EventDestinationsBaseUrl}/{EventDestinationId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.EventDestinations.Get(EventDestinationId001);

            response.Should().NotBeNull();
            response.Id.Should().Be(EventDestinationId001);
        }

        [Fact]
        public async Task Update_WithValidRequest_ReturnsUpdatedEventDestination()
        {
            var expectedRequest = new UpdateEventDestinationRequest
            {
                AppId = AppId002,
                Target = "https://my-callback-server.com/capability-optin-optout",
                Triggers = [EventDestinationTrigger.Capability, EventDestinationTrigger.OptIn, EventDestinationTrigger.OptOut],
                Secret = "SpacePanda_RidesUnicycle"
            };

            var expectedResponse = new EventDestination
            {
                Id = EventDestinationId004,
                AppId = AppId002,
                Target = "https://my-callback-server.com/capability-optin-optout",
                TargetType = EventDestinationTargetType.Http,
                Secret = "SpacePanda_RidesUnicycle",
                Triggers = [EventDestinationTrigger.Capability, EventDestinationTrigger.OptIn, EventDestinationTrigger.OptOut]
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"{EventDestinationsBaseUrl}/{EventDestinationId004}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.EventDestinations.Update(EventDestinationId004, expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(EventDestinationId004);
        }

        [Fact]
        public async Task Delete_WithValidEventDestinationId_DeletesEventDestinationSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{EventDestinationsBaseUrl}/{EventDestinationId004}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new { }));

            await Conversation.EventDestinations.Delete(EventDestinationId004);
        }

        [Fact]
        public async Task Get_WhenApiReturnsError_ThrowsSinchApiException()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{EventDestinationsBaseUrl}/{EventDestinationId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.NotFound, JsonContent.Create(new
                {
                    error = new
                    {
                        code = 404,
                        message = "EventDestination not found",
                        status = "NOT_FOUND"
                    }
                }));

            Func<Task> act = () => Conversation.EventDestinations.Get(EventDestinationId001);

            await act.Should().ThrowAsync<SinchApiException>();
        }
    }
}
