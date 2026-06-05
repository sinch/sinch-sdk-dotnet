using Microsoft.AspNetCore.Mvc;
using Sinch.Voice.SinchEvents;

namespace SinchEvents.Template.Voice;

/// <summary>
/// Example controller for receiving Voice Sinch Events.
/// </summary>
[ApiController]
[VoiceSinchEvent(requireAuthentication: false)]
public class VoiceSinchEventsController : ControllerBase
{
    private readonly IVoiceSinchEvents _voiceSinchEvents;
    private readonly VoiceServerBusinessLogic _businessLogic;
    private readonly ILogger<VoiceSinchEventsController> _logger;

    public VoiceSinchEventsController(
        IVoiceSinchEvents voiceSinchEvents,
        VoiceServerBusinessLogic businessLogic,
        ILogger<VoiceSinchEventsController> logger)
    {
        _voiceSinchEvents = voiceSinchEvents;
        _businessLogic = businessLogic;
        _logger = logger;
    }

    [HttpPost("VoiceEvent")]
    [Consumes("application/json")]
    public IActionResult VoiceEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;
        var voiceEvent = _voiceSinchEvents.ParseEvent(body!);

        SinchEventResponse? response = voiceEvent switch
        {
            IncomingCallEvent ice => _businessLogic.IncomingCallEvent(ice),
            AnsweredCallEvent ace => _businessLogic.AnsweredCallEvent(ace),
            DisconnectedCallEvent dice => HandleDisconnected(dice),
            PromptInputEvent pie => _businessLogic.PromptInputEvent(pie),
            NotificationEvent notify => HandleNotification(notify),
            _ => throw new InvalidOperationException($"Unexpected voice event type: {voiceEvent.GetType()}")
        };

        if (response is not null)
        {
            var serializedResponse = _voiceSinchEvents.SerializeResponse(response);
            _logger.LogInformation("JSON response: {SerializedResponse}", serializedResponse);
            return Ok(serializedResponse);
        }

        return Ok();
    }

    private SinchEventResponse? HandleDisconnected(DisconnectedCallEvent dice)
    {
        _businessLogic.DisconnectedCallEvent(dice);
        return null;
    }

    private SinchEventResponse? HandleNotification(NotificationEvent notify)
    {
        _businessLogic.NotificationEvent(notify);
        return null;
    }
}
