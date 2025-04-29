using System;
using System.Text.Json;
using FluentAssertions;
using Sinch.Voice.Hooks;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class DeserializeHooksTests
    {
        [Fact]
        public void DeserializeAce()
        {
            var json = Helpers.LoadResources("Voice/AnsweredCallEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            @event.As<AnsweredCallEvent>().Should().BeEquivalentTo(new AnsweredCallEvent
            {
                Event = EventType.AnsweredCallEvent,
                CallId = "a call id",
                Timestamp = Helpers.ParseUtc("2024-01-19T12:49:53Z"),
                Version = 1,
                Custom = "my custom value",
                Amd = new AmdObject
                {
                    Status = AmdObject.AmdStatus.Human,
                    Reason = AmdObject.AmdReason.LongGreeting,
                    Duration = 15
                }
            });
        }

        [Fact]
        public void DeserializeNotificationEvent()
        {
            var json = Helpers.LoadResources("Voice/NotificationEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            @event.As<NotificationEvent>().Should().BeEquivalentTo(new NotificationEvent()
            {
                Event = EventType.NotificationEvent,
                CallId = "a call id",
                Type = "recording_finished",
                Version = 1,
                Custom = "my custom value",
                ConferenceId = "conferenceId value",
                Destination = "destination value",
                Amd = new AmdObject
                {
                    Status = AmdObject.AmdStatus.Human,
                    Reason = AmdObject.AmdReason.LongGreeting,
                    Duration = 15
                }
            });
        }
    }
}
