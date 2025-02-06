using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.Conversation.Hooks;

namespace WebApiExamples.Controllers;

[ApiController]
[Route("event-listener")]
public class ReceiveConversationCallbackController : ControllerBase
{
    private readonly ISinchClient _sinch;
    private readonly ILogger _logger;
    private const string Secret = "1234_my_secret";

    public ReceiveConversationCallbackController(ISinchClient sinch, ILogger<ReplyToInboundController> logger)
    {
        _sinch = sinch;
        _logger = logger;
    }

    [HttpPost(Name = "handle")]
    public ActionResult Handle([FromBody] JsonObject json)
    {
        var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value);
        if (!_sinch.Conversation.Webhooks.ValidateAuthenticationHeader(headers, json,
                Secret))
        {
            _logger.LogError("Failed to authorize received callback.");
            return Unauthorized();
        }
        
        var callbackEvent = _sinch.Conversation.Webhooks.ParseEvent(json);
        
        // do something with specific event
        switch (callbackEvent)
        {
            case CapabilityEvent capabilityEvent:
                break;
            case ChannelEvent channelEvent:
                break;
            case ContactCreateEvent contactCreateEvent:
                break;
            case ContactDeleteEvent contactDeleteEvent:
                break;
            case ContactIdentitiesDuplicationEvent contactIdentitiesDuplicationEvent:
                break;
            case ContactMergeEvent contactMergeEvent:
                break;
            case ContactUpdateEvent contactUpdateEvent:
                break;
            case ConversationDeleteEvent conversationDeleteEvent:
                break;
            case ConversationStartEvent conversationStartEvent:
                break;
            case ConversationStopEvent conversationStopEvent:
                break;
            case DeliveryEvent deliveryEvent:
                break;
            case InboundEvent inboundEvent:
                break;
            case MessageDeliveryReceiptEvent messageDeliveryReceiptEvent:
                break;
            case MessageInboundEvent messageInboundEvent:
                break;
            case MessageInboundSmartConversationRedactionEvent messageInboundSmartConversationRedactionEvent:
                break;
            case MessageSubmitEvent messageSubmitEvent:
                break;
            case OptInEvent optInEvent:
                break;
            case OptOutEvent optOutEvent:
                break;
            case SmartConversationsEvent smartConversationsEvent:
                break;
            case UnsupportedCallbackEvent unsupportedCallbackEvent:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(callbackEvent));
        }

        return Ok();
    }
}
