using System.Text.Json;
using FluentAssertions;
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
    }
}
