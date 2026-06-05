using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Sinch.Numbers.SinchEvents;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class SinchEventsTests : NumberTestBase
    {
        private const string Body =
            "{\"eventId\":\"01hpa0mww4m79q8j2dwn3ggbgz\",\"timestamp\":\"2024-02-10T17:22:09.412722588\",\"projectId\":\"37b62a7b-0177-abcd-efgh-e10f848de123\",\"resourceId\":\"+17818510001\",\"resourceType\":\"ACTIVE_NUMBER\",\"eventType\":\"DEPROVISIONING_TO_VOICE_PLATFORM\",\"status\":\"SUCCEEDED\",\"failureCode\":null}";
        private const string HmacSecret = "event-secret";
        private const string ValidSignature = "c8933dc64fbe81f37516f95c2b587f2e1ea91dfa";

        [Fact]
        public void ParseEvent_ReturnsNumbersSinchEvent()
        {
            var result = Numbers.SinchEvents.ParseEvent(Body);

            result.Should().BeOfType<NumbersSinchEvent>();
            result.EventId.Should().Be("01hpa0mww4m79q8j2dwn3ggbgz");
            result.Status.Should().Be(EventStatus.Succeeded);
            result.ResourceId.Should().Be("+17818510001");
        }

        [Fact]
        public void ParseEvent_WorksWithDirectInstantiation()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var sinchEvents = new NumbersSinchEvents(options);

            var result = sinchEvents.ParseEvent(Body);

            result.Should().BeOfType<NumbersSinchEvent>();
            result.EventId.Should().Be("01hpa0mww4m79q8j2dwn3ggbgz");
            result.ResourceType.Should().Be(ResourceType.ActiveNumber);
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsTrue()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                ["x-sinch-signature"] = new[] { ValidSignature }
            };

            Numbers.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, Body)
                .Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsTrue_WhenHeaderKeyIsUpperCase()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                ["X-SINCH-SIGNATURE"] = new[] { ValidSignature }
            };

            Numbers.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, Body)
                .Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsTrue_WhenHeaderKeyIsMixedCase()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                ["X-Sinch-Signature"] = new[] { ValidSignature }
            };

            Numbers.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, Body)
                .Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsFalse_WhenSignatureWrong()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                ["x-sinch-signature"] = new[] { "0000000000000000000000000000000000000000" }
            };

            Numbers.SinchEvents.ValidateAuthenticationHeader(HmacSecret, headers, Body)
                .Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsFalse_WhenHeaderMissing()
        {
            Numbers.SinchEvents.ValidateAuthenticationHeader(
                HmacSecret, new Dictionary<string, IEnumerable<string>>(), Body)
                .Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_ReturnsFalse_WhenSecretWrong()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                ["x-sinch-signature"] = new[] { ValidSignature }
            };

            Numbers.SinchEvents.ValidateAuthenticationHeader("wrong-secret", headers, Body)
                .Should().BeFalse();
        }

        [Fact]
        public void ParseEvent_DoesNotRequireCredentials()
        {
            var sinch = new SinchClient();
            var json = Helpers.LoadResources("Numbers/SinchEvents/NumberSinchEvent.json");

            var sinchEvent = sinch.Numbers.SinchEvents.ParseEvent(json);

            sinchEvent.Should().NotBeNull();
            sinchEvent.EventId.Should().Be("abcd1234efghijklmnop567890");
        }
    }
}
