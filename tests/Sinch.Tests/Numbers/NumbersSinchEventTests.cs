using System.Text.Json;
using FluentAssertions;
using Sinch.Numbers;
using Sinch.Numbers.SinchEvents;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class NumbersSinchEventTests
    {
        [Fact]
        public void DeserializeEvent()
        {
            var json = Helpers.LoadResources("Numbers/SinchEvents/NumberSinchEvent.json");

            var result = JsonSerializer.Deserialize<NumbersSinchEvent>(json)!;

            result.EventId.Should().Be("abcd1234efghijklmnop567890");
            result.ProjectId.Should().Be("abcd12ef-ab12-ab12-bc34-abcdef123456");
            result.ResourceId.Should().Be("+12345612345");
            result.ResourceType.Should().Be(ResourceType.ActiveNumber);
            result.EventType.Should().Be(EventType.ProvisioningToCampaign);
            result.Status.Should().Be(EventStatus.Failed);
            result.FailureCode.Should().Be(FailureCode.CampaignNotAvailable);
        }

        [Fact]
        public void DeserializeEventWithCustomEnum()
        {
            var json = Helpers.LoadResources("Numbers/SinchEvents/NumberSinchEventUnknownType.json");

            var result = JsonSerializer.Deserialize<NumbersSinchEvent>(json)!;

            result.Status.Should().Be(EventStatus.Failed);
            result.EventType.Should().Be(new EventType("UNEXPECTED_ENUM_TYPE"));
            result.FailureCode.Should().Be(FailureCode.CampaignNotAvailable);
        }
    }
}
