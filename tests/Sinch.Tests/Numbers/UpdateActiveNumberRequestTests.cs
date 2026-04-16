using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Sinch.Numbers;
using Sinch.Numbers.Active.Update;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class UpdateActiveNumberRequestTests : NumberTestBase
    {
        private JsonSerializerOptions Options => HttpCamelCase.JsonSerializerOptions;

        [Fact]
        public void SerializeSmsConfigurationWithValue()
        {
            var request = new UpdateActiveNumberRequest
            {
                SmsConfiguration = new SmsConfiguration { ServicePlanId = "plan-123" }
            };

            var json = JsonSerializer.Serialize(request, Options);
            using var doc = JsonDocument.Parse(json);

            doc.RootElement.TryGetProperty("smsConfiguration", out var prop).Should().BeTrue();
            prop.ValueKind.Should().NotBe(JsonValueKind.Null);
            prop.GetProperty("servicePlanId").GetString().Should().Be("plan-123");
        }

        [Fact]
        public void SerializeSmsConfigurationWithNull()
        {
            var request = new UpdateActiveNumberRequest
            {
                SmsConfiguration = null
            };

            var json = JsonSerializer.Serialize(request, Options);
            using var doc = JsonDocument.Parse(json);

            doc.RootElement.TryGetProperty("smsConfiguration", out var prop).Should().BeTrue();
            prop.ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void SerializeSmsConfigurationWhenUnset()
        {
            var request = new UpdateActiveNumberRequest();

            var json = JsonSerializer.Serialize(request, Options);
            using var doc = JsonDocument.Parse(json);

            doc.RootElement.TryGetProperty("smsConfiguration", out _).Should().BeFalse();
        }

        /// <summary>
        ///     Verifies that [JsonPropertyName] changes the JSON key while SetTracker
        ///     continues to work correctly using the CLR member name internally.
        /// </summary>
        [Fact]
        public void SerializeWithJsonPropertyName_UsesCustomKeyInAllThreeScenarios()
        {
            var options = Options;

            // Scenario 1: value set => custom key present with value
            var withValue = new CustomKeyRequest { Data = "hello" };
            var json1 = JsonSerializer.Serialize(withValue, options);
            using var doc1 = JsonDocument.Parse(json1);
            doc1.RootElement.TryGetProperty("my_own_payload_name", out var p1).Should().BeTrue();
            p1.GetString().Should().Be("hello");

            // Scenario 2: explicitly set to null => custom key present as null
            var withNull = new CustomKeyRequest { Data = null };
            var json2 = JsonSerializer.Serialize(withNull, options);
            using var doc2 = JsonDocument.Parse(json2);
            doc2.RootElement.TryGetProperty("my_own_payload_name", out var p2).Should().BeTrue();
            p2.ValueKind.Should().Be(JsonValueKind.Null);

            // Scenario 3: never assigned => custom key absent entirely
            var unset = new CustomKeyRequest();
            var json3 = JsonSerializer.Serialize(unset, options);
            using var doc3 = JsonDocument.Parse(json3);
            doc3.RootElement.TryGetProperty("my_own_payload_name", out _).Should().BeFalse();
        }

        private sealed class CustomKeyRequest : IHasSetTracker
        {
            private readonly SetTracker _setTracker = new();
            SetTracker IHasSetTracker.SetTracker => _setTracker;

            private string? _data;

            [JsonPropertyName("my_own_payload_name")]
            public string? Data
            {
                get => _data;
                set { _setTracker.Track(); _data = value; }
            }
        }
    }
}
