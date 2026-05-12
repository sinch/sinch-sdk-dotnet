using Sinch.Core;
using Sinch.Voice.SinchEvents;

namespace SinchEvents.Template.Voice;

public class VoiceServerBusinessLogic(ILogger<VoiceServerBusinessLogic> logger)
{
    public CallEventResponse? IncomingCallEvent(IncomingCallEvent incomingCallEvent)
    {
        logger.LogInformation("Handle ICE event: {Event}", incomingCallEvent.ToPrettyString());
        return null;
    }

    public CallEventResponse? AnsweredCallEvent(AnsweredCallEvent answeredCallEvent)
    {
        logger.LogInformation("Handle ACE event: {Event}", answeredCallEvent.ToPrettyString());
        return null;
    }

    public void DisconnectedCallEvent(DisconnectedCallEvent disconnectedCallEvent)
    {
        logger.LogInformation("Handle DiCE event: {Event}", disconnectedCallEvent.ToPrettyString());
    }

    public CallEventResponse? PromptInputEvent(PromptInputEvent promptInputEvent)
    {
        logger.LogInformation("Handle PIE event: {Event}", promptInputEvent.ToPrettyString());
        return null;
    }

    public void NotificationEvent(NotificationEvent notificationEvent)
    {
        logger.LogInformation("Handle notification event: {Event}", notificationEvent.ToPrettyString());
    }
}
