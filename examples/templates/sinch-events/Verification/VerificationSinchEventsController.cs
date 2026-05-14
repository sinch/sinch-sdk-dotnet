using Microsoft.AspNetCore.Mvc;
using Sinch.Verification.SinchEvents;

namespace SinchEvents.Template.Verification;

/// <summary>
/// Example controller for receiving Verification Sinch Events.
/// </summary>
[ApiController]
[VerificationSinchEvent(requireAuthentication: true)]
public class VerificationSinchEventsController : ControllerBase
{
    private readonly IVerificationSinchEvents _verificationSinchEvents;
    private readonly VerificationServerBusinessLogic _businessLogic;
    private readonly ILogger<VerificationSinchEventsController> _logger;
    
    public VerificationSinchEventsController(
        IVerificationSinchEvents verificationSinchEvents,
        VerificationServerBusinessLogic businessLogic,
        ILogger<VerificationSinchEventsController> logger)
    {
        _verificationSinchEvents = verificationSinchEvents;
        _businessLogic = businessLogic;
        _logger = logger;
    }

    [HttpPost("VerificationEvent")]
    [Consumes("application/json")]
    public IActionResult VerificationEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;
        var verificationEvent = _verificationSinchEvents.ParseEvent(body!);

        if (verificationEvent is VerificationStartEvent startEvent)
        {
            var response = _businessLogic.HandleEvent(startEvent);
            var serializedResponse = _verificationSinchEvents.SerializeResponse(response);
            _logger.LogInformation("JSON response: {SerializedResponse}", serializedResponse);
            return Ok(serializedResponse);
        }

        if (verificationEvent is VerificationResultEvent resultEvent)
        {
            _businessLogic.HandleEvent(resultEvent);
            return Ok();
        }

        if (verificationEvent is VerificationSmsDeliveredEvent smsDeliveredEvent)
        {
            _businessLogic.HandleEvent(smsDeliveredEvent);
            return Ok();
        }

        throw new InvalidOperationException($"Unexpected verification event type: {verificationEvent.GetType()}");
    }
}
