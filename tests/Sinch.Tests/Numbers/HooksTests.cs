using System.Text.Json;
using FluentAssertions;
using Sinch.Numbers.Hooks;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class HooksTests
    {
        [Fact]
        public void DeserializeEvent()
        {
            const string jsonInput = @"
            {
                ""eventId"": ""abcd1234efghijklmnop567890"",
                ""timestamp"": ""2023-06-06T07:45:27.785357"",
                ""projectId"": ""abcd12ef-ab12-ab12-bc34-abcdef123456"",
                ""resourceId"": ""+12345612345"",
                ""resourceType"": ""NUMBER"",
                ""eventType"": ""LINK_TO_10DLC_CAMPAIGN"",
                ""status"": ""FAILED"",
                ""failureCode"": ""CAMPAIGN_NOT_AVAILABLE""
            }";
            var @enum = JsonSerializer.Deserialize<Event>(jsonInput)!;
            @enum.Status.Should().Be(EventStatus.Failed);
            @enum.EventType.Should().Be(EventType.LinkTo10DlcCampaign);
            @enum.FailureCode.Should().Be(FailureCode.CampaignNotAvailable);
        }
        
        [Fact]
        public void DeserializeEventWithCustomEnum()
        {
            const string jsonInput = @"
            {
                ""eventId"": ""abcd1234efghijklmnop567890"",
                ""timestamp"": ""2023-06-06T07:45:27.785357"",
                ""projectId"": ""abcd12ef-ab12-ab12-bc34-abcdef123456"",
                ""resourceId"": ""+12345612345"",
                ""resourceType"": ""NUMBER"",
                ""eventType"": ""UNEXPECTED_ENUM_TYPE"",
                ""status"": ""FAILED"",
                ""failureCode"": ""CAMPAIGN_NOT_AVAILABLE""
            }";
            var @enum = JsonSerializer.Deserialize<Event>(jsonInput)!;
            @enum.Status.Should().Be(EventStatus.Failed);
            @enum.EventType.Should().Be(new EventType("UNEXPECTED_ENUM_TYPE"));
            @enum.FailureCode.Should().Be(FailureCode.CampaignNotAvailable);
        }
    }
}
