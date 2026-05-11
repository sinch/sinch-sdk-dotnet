using Sinch.Core;
using Sinch.Verification.SinchEvents;

namespace SinchEvents.Template.Verification;

public class VerificationServerBusinessLogic(ILogger<VerificationServerBusinessLogic> logger)
{
    public VerificationStartEventResponseBase VerificationStartEvent(VerificationStartEvent verificationEvent)
    {
        logger.LogInformation("Handle start event: {Event}", verificationEvent.ToPrettyString());
        return new VerificationStartEventResponseSms { Action = Sinch.Verification.SinchEvents.Action.Allow };
    }

    public VerificationStartEventResponseBase? VerificationResultEvent(VerificationResultEvent verificationEvent)
    {
        logger.LogInformation("Handle result event: {Event}", verificationEvent.ToPrettyString());
        return null;
    }
}
