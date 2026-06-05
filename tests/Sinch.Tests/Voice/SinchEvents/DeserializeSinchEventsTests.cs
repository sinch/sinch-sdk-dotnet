using System.Collections.Generic;
using Sinch.Voice.Destinations;
using System.Text.Json;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Calls;
using Sinch.Voice.Callouts.Callout;
using Sinch.Voice.SinchEvents;
using Xunit;

namespace Sinch.Tests.Voice.SinchEvents
{
    public class DeserializeSinchEventsTests
    {
        private readonly IVoiceSinchEvents _sinchEvents = new SinchClient(new SinchClientConfiguration
        {
            VoiceConfiguration = new SinchVoiceConfiguration
            {
                AppKey = "appkey",
                AppSecret = "appsecret"
            }
        }).Voice.SinchEvents;

        // ── ACE ──────────────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_ReturnsAnsweredCallEvent_WhenAllAnsweredCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/AnsweredCallEventAllFields.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<AnsweredCallEvent>().Which.Should().BeEquivalentTo(new AnsweredCallEvent
            {
                CallId = "a call id",
                ConferenceId = "a conference id",
                Timestamp = Helpers.ParseUtc("2024-01-19T12:49:53Z"),
                Version = 1,
                Custom = "my custom value",
                ApplicationKey = "my application key",
                Amd = new AnsweringMachineDetection
                {
                    Status = AnsweringMachineDetection.AnsweringMachineDetectionStatus.Human,
                    Reason = AnsweringMachineDetection.AnsweringMachineDetectionReason.LongGreeting,
                    Duration = 15
                }
            });
        }

        [Fact]
        public void ParseEvent_ReturnsAnsweredCallEvent_WhenPartialAnsweredCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/AnsweredCallEvent.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<AnsweredCallEvent>().Which.Should().BeEquivalentTo(new AnsweredCallEvent
            {
                CallId = "a call id",
                Version = 1
            });
        }

        // ── DICE ─────────────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_ReturnsDisconnectedCallEvent_WhenAllDisconnectedCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/DisconnectedCallEventAllFields.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<DisconnectedCallEvent>().Which.Should().BeEquivalentTo(new DisconnectedCallEvent
            {
                CallId = "a call id",
                ConferenceId = "a conference id",
                Timestamp = Helpers.ParseUtc("2024-01-19T12:49:53Z"),
                Reason = CallResultReason.ManagerHangUp,
                Result = CallResult.Answered,
                Version = 1,
                Custom = "my custom value",
                Debit = new Rate { CurrencyId = "EUR", Amount = 0.1758M },
                UserRate = new Rate { CurrencyId = "USD", Amount = 0.345M },
                To = new DestinationPstn { Endpoint = "123456789" },
                ApplicationKey = "an app key",
                Duration = 1,
                From = "private",
                CallHeaders = new List<CallHeader> { new() { Key = "the key", Value = "the value" } }
            });
        }

        [Fact]
        public void ParseEvent_ReturnsDisconnectedCallEvent_WhenPartialDisconnectedCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/DisconnectedCallEvent.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<DisconnectedCallEvent>().Which.Should().BeEquivalentTo(new DisconnectedCallEvent
            {
                CallId = "a call id",
                Version = 1
            });
        }

        // ── ICE ──────────────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_ReturnsIncomingCallEvent_WhenAllIncomingCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/IncomingCallEventAllFields.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<IncomingCallEvent>().Which.Should().BeEquivalentTo(new IncomingCallEvent
            {
                CallId = "a call id",
                ConferenceId = "a conference id",
                CallResourceUrl = "https://calling-euc1.api.sinch.com/calling/v1/calls/id/a-call-id",
                Timestamp = Helpers.ParseUtc("2024-01-16T16:46:36Z"),
                Version = 1,
                Custom = "my custom",
                UserRate = new Rate { CurrencyId = "USD", Amount = 0.0M },
                Cli = "cli number",
                To = new DestinationPstn { Endpoint = "+123456879" },
                Domain = Domain.Mxp,
                ApplicationKey = "an app key",
                OriginationType = Domain.Mxp,
                Rdnis = "rdnis value",
                CallHeaders = new List<CallHeader> { new() { Key = "the key", Value = "the value" } }
            });
        }

        [Fact]
        public void ParseEvent_ReturnsIncomingCallEvent_WhenPartialIncomingCallEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/IncomingCallEvent.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<IncomingCallEvent>().Which.Should().BeEquivalentTo(new IncomingCallEvent
            {
                CallId = "a call id",
                Version = 1
            });
        }

        // ── Notify ───────────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_ReturnsNotificationEvent_WhenAllNotificationEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/NotificationEventAllFields.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<NotificationEvent>().Which.Should().BeEquivalentTo(new NotificationEvent
            {
                CallId = "a call id",
                ConferenceId = "a conference id",
                Version = 1,
                Custom = "my custom value",
                Type = "recording_finished",
                Destination = "destination value",
                Amd = new AnsweringMachineDetection
                {
                    Status = AnsweringMachineDetection.AnsweringMachineDetectionStatus.Human,
                    Reason = AnsweringMachineDetection.AnsweringMachineDetectionReason.LongGreeting,
                    Duration = 15
                }
            });
        }

        [Fact]
        public void ParseEvent_ReturnsNotificationEvent_WhenPartialNotificationEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/NotificationEvent.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<NotificationEvent>().Which.Should().BeEquivalentTo(new NotificationEvent
            {
                CallId = "a call id",
                Version = 1
            });
        }

        // ── PIE ──────────────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_ReturnsPromptInputEvent_WhenAllPromptInputEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/PromptInputEventAllFields.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<PromptInputEvent>().Which.Should().BeEquivalentTo(new PromptInputEvent
            {
                CallId = "a call id",
                ConferenceId = "a conference id",
                Timestamp = Helpers.ParseUtc("2024-01-23T15:04:28Z"),
                Version = 1,
                Custom = "my custom value",
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

        [Fact]
        public void ParseEvent_ReturnsPromptInputEvent_WhenPartialPromptInputEventFieldsProvided()
        {
            var json = Helpers.LoadResources("Voice/PromptInputEvent.json");

            var result = _sinchEvents.ParseEvent(json);

            result.Should().BeOfType<PromptInputEvent>().Which.Should().BeEquivalentTo(new PromptInputEvent
            {
                CallId = "a call id",
                Version = 1
            });
        }

        // ── Error handling ────────────────────────────────────────────────────────

        [Fact]
        public void ParseEvent_Throws_WhenEventTypeUnknown()
        {
            const string json = """{ "event": "unknown_event_type" }""";

            System.Action act = () => _sinchEvents.ParseEvent(json);

            act.Should().Throw<JsonException>()
                .WithMessage("*Voice Sinch Event*");
        }

        // ── Enum case-insensitivity ───────────────────────────────────────────────

        [Theory]
        [InlineData("\"mxp\"")]
        [InlineData("\"MXP\"")]
        public void Deserialize_Domain_IsCaseInsensitive(string value)
        {
            JsonSerializer.Deserialize<Domain>(value).Should().BeEquivalentTo(Domain.Mxp);
        }

        [Theory]
        [InlineData("\"Number\"")]
        [InlineData("\"number\"")]
        public void Deserialize_DestinationType_IsCaseInsensitive(string value)
        {
            JsonSerializer.Deserialize<DestinationType>(value).Should().BeEquivalentTo(DestinationType.Number);
        }
    }
}
