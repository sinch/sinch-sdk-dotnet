using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Callouts.Callout;
using Sinch.Voice.Calls;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Hooks;
using Xunit;
using DestinationType = Sinch.Voice.Common.DestinationType;

namespace Sinch.Tests.Voice
{
    public class DeserializeHooksTests
    {
        private readonly ISinchVoiceClient _voiceClient = new SinchClient(null, null, null).Voice("k", "v");

        [Fact]
        public void DeserializeAce()
        {
            var json = Helpers.LoadResources("Voice/AnsweredCallEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            var eventWithClient = _voiceClient.ParseEvent(json);

            AssertEvent(@event);
            AssertEvent(eventWithClient);

            void AssertEvent(IVoiceEvent parsed)
            {
                parsed.As<AnsweredCallEvent>().Should().BeEquivalentTo(new AnsweredCallEvent
                {
                    Event = EventType.AnsweredCallEvent,
                    CallId = "a call id",
                    Timestamp = Helpers.ParseUtc("2024-01-19T12:49:53Z"),
                    Version = 1,
                    Custom = "my custom value",
                    Amd = new AnsweringMachineDetection
                    {
                        Status = AnsweringMachineDetection.AnsweringMachineDetectionStatus.Human,
                        Reason = AnsweringMachineDetection.AnsweringMachineDetectionReason.LongGreeting,
                        Duration = 15
                    }
                });
            }
        }

        [Fact]
        public void DeserializeNotificationEvent()
        {
            var json = Helpers.LoadResources("Voice/NotificationEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            var eventWithClient = _voiceClient.ParseEvent(json);

            AssertEvent(@event);
            AssertEvent(eventWithClient);

            void AssertEvent(IVoiceEvent parsed)
            {
                parsed.As<NotificationEvent>().Should().BeEquivalentTo(new NotificationEvent
                {
                    Event = EventType.NotificationEvent,
                    CallId = "a call id",
                    Type = "recording_finished",
                    Version = 1,
                    Custom = "my custom value",
                    ConferenceId = "conferenceId value",
                    Destination = "destination value",
                    Amd = new AnsweringMachineDetection
                    {
                        Status = AnsweringMachineDetection.AnsweringMachineDetectionStatus.Human,
                        Reason = AnsweringMachineDetection.AnsweringMachineDetectionReason.LongGreeting,
                        Duration = 15
                    }
                });
            }
        }

        [Fact]
        public void DeserializePromtInputEvent()
        {
            var json = Helpers.LoadResources("Voice/PromtInputEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            var eventWithClient = _voiceClient.ParseEvent(json);

            AssertEvent(@event);
            AssertEvent(eventWithClient);

            void AssertEvent(IVoiceEvent parsed)
            {
                parsed.As<PromptInputEvent>().Should().BeEquivalentTo(new PromptInputEvent
                {
                    Event = EventType.PromptInputEvent,
                    CallId = "a call id",
                    Timestamp = Helpers.ParseUtc("2024-01-23T15:04:28Z"),
                    Version = 1,
                    ApplicationKey = "my application key",
                    MenuResult = new MenuResult
                    {
                        MenuId = "confirm",
                        Type = MenuType.Sequence,
                        Value = "1452",
                        InputMethod = InputMethod.Dtmf
                    }
                });
            }
        }

        [Fact]
        public void DeserializeDisconnectedCallEvent()
        {
            var json = Helpers.LoadResources("Voice/DisconnectedCallEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            var eventWithClient = _voiceClient.ParseEvent(json);

            AssertEvent(@event);
            AssertEvent(eventWithClient);

            void AssertEvent(IVoiceEvent parsed)
            {
                parsed.As<DisconnectedCallEvent>().Should().BeEquivalentTo(new DisconnectedCallEvent
                {
                    Event = EventType.DisconnectedCallEvent,
                    CallId = "a call id",
                    Timestamp = Helpers.ParseUtc("2024-01-19T12:49:53Z"),
                    Reason = CallResultReason.ManagerHangUp,
                    Result = CallResult.Answered,
                    Version = 1,
                    Custom = "my custom value",
                    Debit = new Rate
                    {
                        CurrencyId = "EUR",
                        Amount = 0.1758M
                    },
                    UserRate = new Rate
                    {
                        CurrencyId = "USD",
                        Amount = 0.345M
                    },
                    To = new To
                    {
                        Type = DestinationType.Number,
                        Endpoint = "123456789"
                    },
                    ApplicationKey = "an app key",
                    Duration = 1,
                    From = "private",
                });
            }
        }

        [Fact]
        public void DeserializeIncomingCallEvent()
        {
            var json = Helpers.LoadResources("Voice/IncomingCallEvent.json");

            var @event = JsonSerializer.Deserialize<IVoiceEvent>(json);
            var eventWithClient = _voiceClient.ParseEvent(json);

            AssertEvent(@event);
            AssertEvent(eventWithClient);

            void AssertEvent(IVoiceEvent parsed)
            {
                parsed.As<IncomingCallEvent>().Should().BeEquivalentTo(new IncomingCallEvent
                {
                    Event = EventType.IncomingCallEvent,
                    CallId = "a call id",
                    CallResourceUrl = "https://calling-euc1.api.sinch.com/calling/v1/calls/id/a call id",
                    Timestamp = Helpers.ParseUtc("2024-01-16T16:46:36Z"),
                    Version = 1,
                    Custom = "my custom",
                    UserRate = new Rate
                    {
                        CurrencyId = "USD",
                        Amount = 0.0M
                    },
                    Cli = "cli number",
                    To = new To
                    {
                        Type = DestinationType.Number,
                        Endpoint = "+123456879"
                    },
                    Domain = Domain.Mxp,
                    ApplicationKey = "an app key",
                    OriginationType = Domain.Mxp,
                    Rdnis = "rdnis value",
                    CallHeaders = new List<CallHeader>
                    {
                        new CallHeader
                        {
                            Key = "the key",
                            Value = "the value"
                        }
                    }
                });
            }
        }

        [Theory]
        [InlineData("\"mxp\"")]
        [InlineData("\"MXP\"")]
        public void DeserializeDomainCaseInsensitive(string domainStr)
        {

            var enumValue = JsonSerializer.Deserialize<Domain>(domainStr);

            enumValue.Should().BeEquivalentTo(Domain.Mxp);
        }

        [Theory]
        [InlineData("\"Number\"")]
        [InlineData("\"number\"")]
        public void DeserializeDestinationTypeCaseInsensitive(string domainStr)
        {

            var enumValue = JsonSerializer.Deserialize<DestinationType>(domainStr);

            enumValue.Should().BeEquivalentTo(DestinationType.Number);
        }
    }
}
