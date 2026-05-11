using Microsoft.AspNetCore.Mvc;
using Sinch.Verification.SinchEvents;

namespace SinchEvents.Template.Verification;

/// <summary>
/// Example controller for receiving Verification Sinch Events.
/// </summary>
[ApiController]
[VerificationSinchEvent(requireAuthentication: false)]
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

    [HttpPost("VerificationStartEvent")]
    [Consumes("application/json")]
    public IActionResult VerificationEvent()
    {
        var body = HttpContext.Items[SinchEventsConstants.BodyItemKey] as string;
        var verificationEvent = _verificationSinchEvents.ParseEvent(body!);

        var response = verificationEvent switch
        {
            VerificationStartEvent startEvent => _businessLogic.VerificationStartEvent(startEvent),
            VerificationResultEvent resultEvent => _businessLogic.VerificationResultEvent(resultEvent),
            _ => throw new InvalidOperationException($"Unexpected verification event type: {verificationEvent.GetType()}")
        };

        if (response is not null)
        {
            
            var serializedResponse = _verificationSinchEvents.SerializeResponse(response);
            _logger.LogInformation("JSON response: {SerializedResponse}", serializedResponse);
            return Ok(serializedResponse);
        }
        
        return Ok();
    }
}
