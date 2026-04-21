using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Hooks;
using Xunit;

namespace Sinch.Tests.Conversation.SinchEvents
{
    public class SinchEventsTests : ConversationTestBase
    {
        private const string CallbackSecret = "foo_secret1234";

        [Fact]
        public void ValidateAuthenticationHeader_WithHeadersAndValidSignature_ReturnsTrue()
        {
            var payload = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var headers = new Dictionary<string, IEnumerable<string>>
            {
                { "x-sinch-webhook-signature-nonce", new[] { "01FJA8B4A7BM43YGWSG9GBV067" } },
                { "x-sinch-webhook-signature-timestamp", new[] { "1634579353" } },
                { "x-sinch-webhook-signature", new[] { "wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo=" } },
            };

            var result = Conversation.Webhooks.ValidateAuthenticationHeader(headers, payload, CallbackSecret);

            result.Should().BeTrue();
        }

        [Fact]
        public void ParseEvent_WithJsonString_ReturnsCapabilityEvent()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/CapabilityEvent.json");

            var result = Conversation.Webhooks.ParseEvent(json);

            result.Should().BeOfType<CapabilityEvent>();
            result.As<CapabilityEvent>().CapabilityNotification.Should().NotBeNull();
        }

        [Fact]
        public async Task ParseEventAsync_WithJsonStream_ReturnsCapabilityEvent()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/CapabilityEvent.json");
            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var result = await Conversation.Webhooks.ParseEventAsync(stream);

            result.Should().BeOfType<CapabilityEvent>();
            result.As<CapabilityEvent>().CapabilityNotification.Should().NotBeNull();
        }
    }
}
