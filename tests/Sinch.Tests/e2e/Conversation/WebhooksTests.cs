using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Webhooks;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class WebhooksTests : TestBase
    {
        private const string SomeStringValue = "some_string_value";

        private readonly Webhook _webhookResponse = new Webhook()
        {
            Id = SomeStringValue,
            AppId = SomeStringValue,
            Secret = SomeStringValue,
            TargetType = WebhookTargetType.Http,
            Target = SomeStringValue,
            Triggers = new List<WebhookTrigger>()
            {
                WebhookTrigger.MessageDelivery
            },
            ClientCredentials = new ClientCredentials()
            {
                Endpoint = SomeStringValue,
                ClientId = SomeStringValue,
                ClientSecret = SomeStringValue
            }
        };

        [Fact]
        public async Task Create()
        {
            var response = await SinchClientMockServer.Conversation.Webhooks.Create(new Webhook()
            {
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Secret = "CactusKnight_SurfsWaves",
                Target = "https://my-callback-server.com/capability",
                TargetType = WebhookTargetType.Http,
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Capability
                }
            });
            response.Should().BeEquivalentTo(new Webhook()
            {
                Id = "01W4FFL35P4NC4K35WEBHOOK004",
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Target = "https://my-callback-server.com/capability",
                TargetType = WebhookTargetType.Http,
                Secret = "CactusKnight_SurfsWaves",
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Capability
                },
                ClientCredentials = null
            });
        }

        [Fact]
        public async Task CreateInvalidValue()
        {
            var op = () => SinchClientMockServer.Conversation.Webhooks.Create(new Webhook()
            {
                AppId = "APPID",
                Target = "http://localhost:8080",
                TargetType = new WebhookTargetType("CUSTOM_UNKNOWN"),
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Capability
                },
            });
            // mock server should not match this request, ideally it should throw the exception with detailed message
            // - for this case - target_type enum have an invalid value
            // but I'm testing matching request from open-api-spec file
            await op.Should().ThrowAsync<SinchApiException>().Where(x => x.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockServer.Conversation.Webhooks.Get("123");
            response.Should().BeEquivalentTo(new Webhook()
            {
                Id = "01W4FFL35P4NC4K35WEBHOOK001",
                AppId = "01W4FFL35P4NC4K35CONVAPP001",
                Target = "https://my-callback-server.com/unsupported",
                TargetType = WebhookTargetType.Http,
                Secret = "VeganVampire_SipsTea",
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Unsupported
                },
                ClientCredentials = new ClientCredentials()
                {
                    Endpoint = "https://my-auth-server.com/oauth2/token",
                    ClientId = "webhook-username",
                    ClientSecret = "webhook-password"
                }
            });
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.Webhooks.Delete("123");
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockServer.Conversation.Webhooks.List("appid");

            var expected = new List<Webhook>
            {
                new Webhook
                {
                    Id = "01W4FFL35P4NC4K35WEBHOOK001",
                    AppId = "01W4FFL35P4NC4K35CONVAPP001",
                    Target = "https://my-callback-server.com/unsupported",
                    TargetType = WebhookTargetType.Http,
                    Secret = "VeganVampire_SipsTea",
                    Triggers = new List<WebhookTrigger> { WebhookTrigger.Unsupported },
                    ClientCredentials = new ClientCredentials
                    {
                        Endpoint = "https://my-auth-server.com/oauth2/token",
                        ClientId = "webhook-username",
                        ClientSecret = "webhook-password"
                    }
                },
                new Webhook
                {
                    Id = "01W4FFL35P4NC4K35WEBHOOK002",
                    AppId = "01W4FFL35P4NC4K35CONVAPP001",
                    Target = "https://my-callback-server.com/contact",
                    TargetType = WebhookTargetType.Http,
                    Secret = "DiscoDragon_BuildsLego",
                    Triggers = new List<WebhookTrigger>
                    {
                        WebhookTrigger.ContactCreate,
                        WebhookTrigger.ContactDelete,
                        WebhookTrigger.ContactIdentitiesDuplication,
                        WebhookTrigger.ContactMerge,
                        WebhookTrigger.ContactUpdate
                    },
                    ClientCredentials = null
                },
                new Webhook
                {
                    Id = "01W4FFL35P4NC4K35WEBHOOK003",
                    AppId = "01W4FFL35P4NC4K35CONVAPP001",
                    Target = "https://my-callback-server.com/conversation",
                    TargetType = WebhookTargetType.Http,
                    Secret = "PunkRockPenguin_GoesFishing",
                    Triggers = new List<WebhookTrigger>
                    {
                        WebhookTrigger.ConversationDelete,
                        WebhookTrigger.ConversationStart,
                        WebhookTrigger.ConversationStop,
                        WebhookTrigger.EventDelivery,
                        WebhookTrigger.EventInbound,
                        WebhookTrigger.MessageDelivery,
                        WebhookTrigger.MessageInbound,
                        WebhookTrigger.MessageSubmit
                    },
                    ClientCredentials = null
                },
                new Webhook
                {
                    Id = "01W4FFL35P4NC4K35WEBHOOK004",
                    AppId = "01W4FFL35P4NC4K35CONVAPP001",
                    Target = "https://my-callback-server.com/capability",
                    TargetType = WebhookTargetType.Http,
                    Secret = "CactusKnight_SurfsWaves",
                    Triggers = new List<WebhookTrigger> { WebhookTrigger.Capability },
                    ClientCredentials = null
                }
            };
            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Update()
        {
            var response = await SinchClientMockServer.Conversation.Webhooks.Update(new Webhook()
            {
                Id = "ID",
                AppId = "01W4FFL35P4NC4K35CONVAPP002",
                Target = "https://my-callback-server.com/capability-optin-optout",
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Capability,
                    WebhookTrigger.OptIn,
                    WebhookTrigger.OptOut
                },
                Secret = "SpacePanda_RidesUnicycle"
            });
            response.Should().BeEquivalentTo(new Webhook
            {
                Id = "01W4FFL35P4NC4K35WEBHOOK004",
                AppId = "01W4FFL35P4NC4K35CONVAPP002",
                Target = "https://my-callback-server.com/capability-optin-optout",
                TargetType = WebhookTargetType.Http,
                Secret = "SpacePanda_RidesUnicycle",
                Triggers = new List<WebhookTrigger>
                {
                    WebhookTrigger.Capability,
                    WebhookTrigger.OptIn,
                    WebhookTrigger.OptOut
                },
                ClientCredentials = null
            });
        }
    }
}
