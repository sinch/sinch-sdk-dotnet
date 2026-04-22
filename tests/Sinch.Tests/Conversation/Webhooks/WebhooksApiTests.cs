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

namespace Sinch.Tests.Conversation.Webhooks
{
    public class WebhooksApiTests : ConversationTestBase
    {
        private const string AppId001 = "01W4FFL35P4NC4K35CONVAPP001";
        private const string AppId002 = "01W4FFL35P4NC4K35CONVAPP002";
        private const string WebhookId001 = "01W4FFL35P4NC4K35WEBHOOK001";
        private const string WebhookId004 = "01W4FFL35P4NC4K35WEBHOOK004";

        private readonly string _webhooksBaseUrl =
            $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/webhooks";

        [Fact]
        public async Task Create_WithValidRequest_ReturnsWebhook()
        {
            var expectedRequest = new CreateWebhookRequest
            {
                AppId = AppId001,
                Target = "https://my-callback-server.com/capability",
                Triggers = [WebhookTrigger.Capability],
                Secret = "CactusKnight_SurfsWaves",
                TargetType = EventDestinationTargetType.Http
            };

            var expectedResponse = new EventDestination
            {
                Id = WebhookId004,
                AppId = AppId001,
                Target = "https://my-callback-server.com/capability",
                TargetType = EventDestinationTargetType.Http,
                Secret = "CactusKnight_SurfsWaves",
                Triggers = [WebhookTrigger.Capability]
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, _webhooksBaseUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Webhooks.Create(expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(WebhookId004);
        }

        [Fact]
        public async Task List_WithValidAppId_ReturnsWebhooks()
        {
            var expectedResponse = new ListWebhooksResponse
            {
                EventDestinations =
                new List<EventDestination>
                {
                    new EventDestination
                    {
                        Id = WebhookId001,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/unsupported",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "VeganVampire_SipsTea",
                        Triggers = [WebhookTrigger.Unsupported],
                        ClientCredentials = new ClientCredentials
                        {
                            Endpoint = "https://my-auth-server.com/oauth2/token",
                            ClientId = "webhook-username",
                            ClientSecret = "webhook-password"
                        }
                    },
                    new EventDestination
                    {
                        Id = WebhookId004,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/capability",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "CactusKnight_SurfsWaves",
                        Triggers = [WebhookTrigger.Capability]
                    }
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/apps/{AppId001}/webhooks")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Webhooks.List(AppId001);

            response.Should().NotBeNull();
            response.EventDestinations.Should().HaveCount(2);
            response.EventDestinations!.First().Id.Should().Be(WebhookId001);
        }

        [Fact]
        public async Task ListAuto_WithValidAppId_IteratesThroughAllWebhooks()
        {
            var expectedResponse = new ListWebhooksResponse
            {
                EventDestinations =
                new List<EventDestination>
                {
                    new EventDestination
                    {
                        Id = WebhookId001,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/unsupported",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "VeganVampire_SipsTea",
                        Triggers = [WebhookTrigger.Unsupported]
                    },
                    new EventDestination
                    {
                        Id = WebhookId004,
                        AppId = AppId001,
                        Target = "https://my-callback-server.com/capability",
                        TargetType = EventDestinationTargetType.Http,
                        Secret = "CactusKnight_SurfsWaves",
                        Triggers = [WebhookTrigger.Capability]
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
            await foreach (var webhook in Conversation.Webhooks.ListAuto(AppId001))
            {
                response.Add(webhook);
            }

            response.Should().HaveCount(2);
            response[0].Id.Should().Be(WebhookId001);
        }

        [Fact]
        public async Task Get_WithValidWebhookId_ReturnsWebhook()
        {
            var expectedResponse = new EventDestination
            {
                Id = WebhookId001,
                AppId = AppId001,
                Target = "https://my-callback-server.com/unsupported",
                TargetType = EventDestinationTargetType.Http,
                Secret = "VeganVampire_SipsTea",
                Triggers = [WebhookTrigger.Unsupported],
                ClientCredentials = new ClientCredentials
                {
                    Endpoint = "https://my-auth-server.com/oauth2/token",
                    ClientId = "webhook-username",
                    ClientSecret = "webhook-password"
                }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_webhooksBaseUrl}/{WebhookId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Webhooks.Get(WebhookId001);

            response.Should().NotBeNull();
            response.Id.Should().Be(WebhookId001);
        }

        [Fact]
        public async Task Update_WithValidRequest_ReturnsUpdatedWebhook()
        {
            var expectedRequest = new UpdateWebhookRequest
            {
                AppId = AppId002,
                Target = "https://my-callback-server.com/capability-optin-optout",
                Triggers = [WebhookTrigger.Capability, WebhookTrigger.OptIn, WebhookTrigger.OptOut],
                Secret = "SpacePanda_RidesUnicycle"
            };

            var expectedResponse = new EventDestination
            {
                Id = WebhookId004,
                AppId = AppId002,
                Target = "https://my-callback-server.com/capability-optin-optout",
                TargetType = EventDestinationTargetType.Http,
                Secret = "SpacePanda_RidesUnicycle",
                Triggers = [WebhookTrigger.Capability, WebhookTrigger.OptIn, WebhookTrigger.OptOut]
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"{_webhooksBaseUrl}/{WebhookId004}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonSerializer.Serialize(expectedRequest, SinchConversationClient.JsonSerializerOptionsInner))
                .Respond(HttpStatusCode.OK, JsonContent.Create(
                    expectedResponse,
                    options: SinchConversationClient.JsonSerializerOptionsInner));

            var response = await Conversation.Webhooks.Update(WebhookId004, expectedRequest);

            response.Should().NotBeNull();
            response.Id.Should().Be(WebhookId004);
        }

        [Fact]
        public async Task Delete_WithValidWebhookId_DeletesWebhookSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{_webhooksBaseUrl}/{WebhookId004}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new { }));

            await Conversation.Webhooks.Delete(WebhookId004);
        }

        [Fact]
        public async Task Get_WhenApiReturnsError_ThrowsSinchApiException()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_webhooksBaseUrl}/{WebhookId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.NotFound, JsonContent.Create(new
                {
                    error = new
                    {
                        code = 404,
                        message = "Webhook not found",
                        status = "NOT_FOUND"
                    }
                }));

            Func<Task> act = () => Conversation.Webhooks.Get(WebhookId001);

            await act.Should().ThrowAsync<SinchApiException>();
        }
    }
}
