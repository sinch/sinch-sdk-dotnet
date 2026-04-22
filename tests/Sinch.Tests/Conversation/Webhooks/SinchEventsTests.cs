using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Hooks;
using Xunit;

namespace Sinch.Tests.Conversation.Webhooks
{
    public class SinchEventsTests : ConversationTestBase
    {
        private const string CallbackSecret = "foo_secret1234";
        private const string TimestampHeader = "x-sinch-webhook-signature-timestamp";
        private const string NonceHeader = "x-sinch-webhook-signature-nonce";
        private const string AlgorithmHeader = "x-sinch-webhook-signature-algorithm";
        private const string SignatureHeader = "x-sinch-webhook-signature";

        [Fact]
        public void ValidateAuthenticationHeader_WithHeadersAndValidSignature_ReturnsTrue()
        {
            var payload = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var headers = new Dictionary<string, IEnumerable<string>>
            {
                { NonceHeader, ["01FJA8B4A7BM43YGWSG9GBV067"] },
                { TimestampHeader, ["1634579353"] },
                { AlgorithmHeader, ["HmacSHA256"] },
                { SignatureHeader, ["wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo="] },
            };

            var result = Conversation.Webhooks.ValidateAuthenticationHeader(headers, payload, CallbackSecret);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithHeadersAndInvalidSignature_ReturnsFalse()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, string>()
            {
                { NonceHeader, "01FJA8B4A7BM43YGWSG9GBV067" },
                { TimestampHeader, "1634579353" },
                { AlgorithmHeader, "HmacSHA256" },
                { SignatureHeader, "wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo=" },
            }, json, "wrong_secret");

            isValid.Should().BeFalse();
        }


        [Fact]
        public void ValidateAuthenticationHeader_WithUnsupportedAlgorithm_ThrowsNotSupportedException()
        {
            var payload = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var headers = new Dictionary<string, IEnumerable<string>>
            {
                { NonceHeader, ["01FJA8B4A7BM43YGWSG9GBV067"] },
                { TimestampHeader, ["1634579353"] },
                { AlgorithmHeader, ["HmacSHA512"] },
                { SignatureHeader, ["wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo="] },
            };

            var act = () => Conversation.Webhooks.ValidateAuthenticationHeader(headers, payload, CallbackSecret);

            act.Should().Throw<NotSupportedException>()
                .WithMessage("Unsupported HMAC algorithm: HmacSHA512");
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithSingleValueHeaders_ReturnsTrue()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, string>()
            {
                { NonceHeader, "01FJA8B4A7BM43YGWSG9GBV067" },
                { TimestampHeader, "1634579353" },
                { AlgorithmHeader, "HmacSHA256" },
                { SignatureHeader, "wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo=" },
            }, json, "foo_secret1234");

            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithMultiValueHeaders_ReturnsTrue()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, IEnumerable<string>>()
            {
                { NonceHeader, ["01FJA8B4A7BM43YGWSG9GBV067"] },
                { TimestampHeader, ["1634579353"] },
                { AlgorithmHeader, ["HmacSHA256"] },
                { SignatureHeader, ["wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo="] },
            }, json, "foo_secret1234");

            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithSingleLinePayload_ReturnsTrue()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidationSingleLine.json");

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, string>()
            {
                { NonceHeader, "01FJA8B4A7BM43YGWSG9GBV067" },
                { TimestampHeader, "1634579353" },
                { AlgorithmHeader, "HmacSHA256" },
                { SignatureHeader, "6bpJoRmFoXVjfJIVglMoJzYXxnoxRujzR4k2GOXewOE=" },
            }, json, "foo_secret1234");

            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithMultiLinePayload_ReturnsTrue()
        {
            var json = Helpers.LoadResources("Conversation/Hooks/WebhooksAuthValidation.json");

            var isValid = Conversation.Webhooks.ValidateAuthenticationHeader(new Dictionary<string, string>()
            {
                { NonceHeader, "01FJA8B4A7BM43YGWSG9GBV067" },
                { TimestampHeader, "1634579353" },
                { AlgorithmHeader, "HmacSHA256" },
                { SignatureHeader, "wKmZBGo4Cf+y9cZoPHhiVw6ziKeubLGqN4OdG8jlaPo=" },
            }, json, "foo_secret1234");

            isValid.Should().BeTrue();
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
