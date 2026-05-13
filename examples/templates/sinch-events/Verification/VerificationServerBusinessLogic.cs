using Sinch.Core;
using Sinch.Verification.SinchEvents;

namespace SinchEvents.Template.Verification;

public class VerificationServerBusinessLogic(ILogger<VerificationServerBusinessLogic> logger)
{
    public VerificationStartEventResponseBase HandleEvent(VerificationStartEvent verificationEvent)
    {
        logger.LogInformation("Handle start event: {Event}", verificationEvent.ToPrettyString());
        return new VerificationStartEventResponseSms { Action = Sinch.Verification.SinchEvents.Action.Allow };
    }

    public void HandleEvent(VerificationResultEvent verificationEvent)
    {
        logger.LogInformation("Handle result event: {Event}", verificationEvent.ToPrettyString());
    }
}
