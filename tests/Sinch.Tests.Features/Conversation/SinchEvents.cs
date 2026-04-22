using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation.EventDestinations;
using Sinch.Conversation.Hooks;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Tests.Features.Conversation;

[Binding]
[Scope(Feature = "[Conversation][Webhooks events]")]
public class SinchEvents
{
    private const string CallbackSecret = "CactusKnight_SurfsWaves";
    private const string BaseEventDestinationsUrl = "http://localhost:3014/webhooks/conversation";

    private readonly HttpClient _httpClient = new();
    private ISinchConversationEventDestinations _eventDestinations;
    private HttpResponseMessage _eventResponse;
    private string _rawEvent;
    private ICallbackEvent _parsedEvent;

    [Given(@"the Conversation Webhooks handler is available")]
    public void GivenTheConversationEventDestinationsHandlerIsAvailable()
    {
        _eventDestinations = Utils.SinchConversationClient().EventDestinations;
    }

    [When(@"I send a request to trigger a ""(.*)"" event")]
    public Task WhenISendARequestToTriggerAnEvent(string eventType)
        => TriggerEvent(GetRouteForEvent(eventType));

    [When(@"I send a request to trigger a ""(.*)"" event with a ""(.*)"" status")]
    public Task WhenISendARequestToTriggerAnEventWithAStatus(string eventType, string status)
        => TriggerEvent(GetRouteForStatus(eventType, status));

    [When(@"I send a request to trigger a ""(.*)"" event for a ""(.*)"" message")]
    public Task WhenISendARequestToTriggerAnEventForAMessage(string eventType, string messageType)
        => TriggerEvent(GetRouteForMessageType(eventType, messageType));

    [Then(@"the header of the Conversation event ""(.*)"" contains a valid signature")]
    public void ThenTheHeaderOfTheConversationEventContainsAValidSignature(string eventType)
    {
        ValidateAuthenticationHeader();
    }

    [Then(@"the header of the Conversation event ""(.*)"" with a ""(.*)"" status contains a valid signature")]
    public void ThenTheHeaderOfTheConversationEventWithAStatusContainsAValidSignature(string eventType, string status)
    {
        ValidateAuthenticationHeader();
    }

    [Then(@"the header of the Conversation event ""(.*)"" for a ""(.*)"" message contains a valid signature")]
    public void ThenTheHeaderOfTheConversationEventForAMessageContainsAValidSignature(string eventType, string messageType)
    {
        ValidateAuthenticationHeader();
    }

    [Then(@"the Conversation event describes a ""(.*)"" event type")]
    public void ThenTheConversationEventDescribesAnEventType(string eventType)
    {
        AssertEventType(eventType);
    }

    [Then(@"the Conversation event describes a ""(.*)"" event type for a ""(.*)"" message")]
    public void ThenTheConversationEventDescribesAnEventTypeForAMessage(string eventType, string messageType)
    {
        AssertEventType(eventType);

        switch (eventType)
        {
            case "MESSAGE_SUBMIT":
                {
                    var messageSubmitEvent = _parsedEvent.Should().BeOfType<MessageSubmitEvent>().Subject;
                    messageSubmitEvent.MessageSubmitNotification.Should().NotBeNull();
                    messageSubmitEvent.MessageSubmitNotification!.SubmittedMessage.Should().NotBeNull();
                    if (string.Equals(messageType, "media", StringComparison.OrdinalIgnoreCase))
                    {
                        messageSubmitEvent.MessageSubmitNotification.SubmittedMessage!.MediaMessage.Should().NotBeNull();
                    }
                    else if (string.Equals(messageType, "text", StringComparison.OrdinalIgnoreCase))
                    {
                        messageSubmitEvent.MessageSubmitNotification.SubmittedMessage!.TextMessage.Should().NotBeNull();
                    }
                    break;
                }
            case "SMART_CONVERSATIONS":
                {
                    var smartConversationsEvent = _parsedEvent.Should().BeOfType<SmartConversationsEvent>().Subject;
                    smartConversationsEvent.SmartConversationNotification.Should().NotBeNull();
                    smartConversationsEvent.SmartConversationNotification!.AnalysisResults.Should().NotBeNull();
                    if (string.Equals(messageType, "media", StringComparison.OrdinalIgnoreCase))
                    {
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults!.MlImageRecognitionResult
                            .Should().NotBeNull();
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults.MlOffensiveAnalysisResult
                            .Should().NotBeNull();
                    }
                    else if (string.Equals(messageType, "text", StringComparison.OrdinalIgnoreCase))
                    {
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults!.MlSentimentResult
                            .Should().NotBeNull();
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults.MlNluResult
                            .Should().NotBeNull();
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults.MlPiiResult
                            .Should().NotBeNull();
                        smartConversationsEvent.SmartConversationNotification.AnalysisResults.MlOffensiveAnalysisResult
                            .Should().NotBeNull();
                    }
                    break;
                }
        }
    }

    [Then(@"the Conversation event describes a FAILED event delivery status and its reason")]
    public void ThenTheConversationEventDescribesAFailedEventDeliveryStatusAndItsReason()
    {
        var deliveryEvent = _parsedEvent.Should().BeOfType<DeliveryEvent>().Subject;
        deliveryEvent.EventDeliveryReport.Should().NotBeNull();
        deliveryEvent.EventDeliveryReport.Status.Should().Be(DeliveryStatus.Failed);
        deliveryEvent.EventDeliveryReport.Reason.Should().NotBeNull();
    }

    [Then(@"the Conversation event describes a FAILED message delivery status and its reason")]
    public void ThenTheConversationEventDescribesAFailedMessageDeliveryStatusAndItsReason()
    {
        var messageDeliveryEvent = _parsedEvent.Should().BeOfType<MessageDeliveryReceiptEvent>().Subject;
        messageDeliveryEvent.MessageDeliveryReport.Should().NotBeNull();
        messageDeliveryEvent.MessageDeliveryReport!.Status.Should().Be(DeliveryStatus.Failed);
        messageDeliveryEvent.MessageDeliveryReport.Reason.Should().NotBeNull();
    }

    private async Task TriggerEvent(string route)
    {
        _eventResponse = await _httpClient.GetAsync($"{BaseEventDestinationsUrl}/{route}");
        _eventResponse.EnsureSuccessStatusCode();

        _rawEvent = await _eventResponse.Content.ReadAsStringAsync();
        _parsedEvent = _eventDestinations.ParseEvent(_rawEvent);
    }

    private void ValidateAuthenticationHeader()
    {
        _eventDestinations.ValidateAuthenticationHeader(_eventResponse.GetAllHeaders(), _rawEvent, CallbackSecret)
            .Should()
            .BeTrue();
    }

    private static string GetRouteForEvent(string eventType)
        => eventType switch
        {
            "CAPABILITY" => "capability-lookup",
            "CONTACT_CREATE" => "contact-create",
            "CONTACT_DELETE" => "contact-delete",
            "CONTACT_MERGE" => "contact-merge",
            "CONTACT_UPDATE" => "contact-update",
            "CONVERSATION_DELETE" => "conversation-delete",
            "CONVERSATION_START" => "conversation-start",
            "CONVERSATION_STOP" => "conversation-stop",
            "EVENT_INBOUND" => "event-inbound",
            "MESSAGE_INBOUND" => "message-inbound",
            "MESSAGE_INBOUND_SMART_CONVERSATION_REDACTION" => "message-inbound/smart-conversation-redaction",
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, "Unsupported event type")
        };

    private static string GetRouteForStatus(string eventType, string status)
    {
        return (eventType, status) switch
        {
            ("EVENT_DELIVERY", "FAILED") => "event-delivery-report/failed",
            ("EVENT_DELIVERY", "DELIVERED") => "event-delivery-report/succeeded",
            ("MESSAGE_DELIVERY", "FAILED") => "message-delivery-report/failed",
            ("MESSAGE_DELIVERY", "QUEUED_ON_CHANNEL") => "message-delivery-report/succeeded",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unsupported event status")
        };
    }

    private static string GetRouteForMessageType(string eventType, string messageType)
    {
        return (eventType, messageType) switch
        {
            ("MESSAGE_SUBMIT", "media") => "message-submit/media",
            ("MESSAGE_SUBMIT", "text") => "message-submit/text",
            ("SMART_CONVERSATIONS", "media") => "smart-conversations/media",
            ("SMART_CONVERSATIONS", "text") => "smart-conversations/text",
            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType,
                "Unsupported event message type")
        };
    }

    private void AssertEventType(string eventType)
    {
        switch (eventType)
        {
            case "CAPABILITY":
                _parsedEvent.Should().BeOfType<CapabilityEvent>().Which.CapabilityNotification.Should().NotBeNull();
                break;
            case "CONTACT_CREATE":
                _parsedEvent.Should().BeOfType<ContactCreateEvent>().Which.ContactCreateNotification.Should().NotBeNull();
                break;
            case "CONTACT_DELETE":
                _parsedEvent.Should().BeOfType<ContactDeleteEvent>().Which.ContactDeleteNotification.Should().NotBeNull();
                break;
            case "CONTACT_MERGE":
                _parsedEvent.Should().BeOfType<ContactMergeEvent>().Which.ContactMergeNotification.Should().NotBeNull();
                break;
            case "CONTACT_UPDATE":
                _parsedEvent.Should().BeOfType<ContactUpdateEvent>().Which.ContactUpdateNotification.Should().NotBeNull();
                break;
            case "CONVERSATION_DELETE":
                _parsedEvent.Should().BeOfType<ConversationDeleteEvent>().Which.ConversationDeleteNotification.Should().NotBeNull();
                break;
            case "CONVERSATION_START":
                _parsedEvent.Should().BeOfType<ConversationStartEvent>().Which.ConversationStartNotification.Should().NotBeNull();
                break;
            case "CONVERSATION_STOP":
                _parsedEvent.Should().BeOfType<ConversationStopEvent>().Which.ConversationStopNotification.Should().NotBeNull();
                break;
            case "EVENT_DELIVERY":
                _parsedEvent.Should().BeOfType<DeliveryEvent>().Which.EventDeliveryReport.Should().NotBeNull();
                break;
            case "EVENT_INBOUND":
                _parsedEvent.Should().BeOfType<InboundEvent>().Which.Event.Should().NotBeNull();
                break;
            case "MESSAGE_DELIVERY":
                _parsedEvent.Should().BeOfType<MessageDeliveryReceiptEvent>().Which.MessageDeliveryReport.Should().NotBeNull();
                break;
            case "MESSAGE_INBOUND":
                _parsedEvent.Should().BeOfType<MessageInboundEvent>().Which.Message.Should().NotBeNull();
                break;
            case "MESSAGE_INBOUND_SMART_CONVERSATION_REDACTION":
                _parsedEvent.Should().BeOfType<MessageInboundSmartConversationRedactionEvent>().Which.MessageRedaction
                    .Should().NotBeNull();
                break;
            case "MESSAGE_SUBMIT":
                _parsedEvent.Should().BeOfType<MessageSubmitEvent>().Which.MessageSubmitNotification.Should().NotBeNull();
                break;
            case "SMART_CONVERSATIONS":
                _parsedEvent.Should().BeOfType<SmartConversationsEvent>().Which.SmartConversationNotification
                    .Should().NotBeNull();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, "Unsupported event type");
        }
    }
}
