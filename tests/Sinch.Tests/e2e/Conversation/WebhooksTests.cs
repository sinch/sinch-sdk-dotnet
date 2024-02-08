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
                AppId = "APPID",
                Secret = "secret",
                Target = "http://localhost:8080",
                TargetType = WebhookTargetType.Http,
                Triggers = new List<WebhookTrigger>()
                {
                    WebhookTrigger.Capability
                },
                ClientCredentials = new ClientCredentials()
                {
                    Endpoint = "a",
                    ClientId = "b",
                    ClientSecret = "c"
                },
            });
            response.Should().BeEquivalentTo(_webhookResponse);
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
            response.Should().BeEquivalentTo(_webhookResponse);
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
            response.Should().HaveCount(1);
        }

        [Fact]
        public async Task Update()
        {
            var response = await SinchClientMockServer.Conversation.Webhooks.Update(_webhookResponse);
            response.Should().BeEquivalentTo(_webhookResponse);
        }
    }
}
