using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.SMS.Batches.Send;
using Sinch.SMS.Hooks;
using DeliveryReport = Sinch.SMS.DeliveryReport;

namespace WebApiExamples.Controllers;

[ApiController]
[Route("reply")]
public class ReplyToInboundController : ControllerBase
{
    private readonly ISinchClient _sinch;
    private readonly ILogger _logger;

    public ReplyToInboundController(ISinchClient sinch, ILogger<ReplyToInboundController> logger)
    {
        _sinch = sinch;
        _logger = logger;
    }

    /// <summary>
    ///     If you want to handle either <see cref="IncomingTextSms"/> or <see cref="IncomingBinarySms"/>
    ///     use <see cref="IIncomingSms"/> as a parameter and match by type.
    /// </summary>
    /// <param name="incomingSms"></param>
    [HttpPost(Name = "receive")]
    public async Task Receive([FromBody] IncomingTextSms incomingSms)
    {
        _logger.LogInformation("Incoming {text} from {sender} to {recipient}",
            incomingSms.Body,
            incomingSms.From,
            incomingSms.To);
        var _ = await _sinch.Sms.Batches.Send(new SendTextBatchRequest()
        {
            Body = incomingSms.Body,
            DeliveryReport = DeliveryReport.None,
            To = new List<string>() { incomingSms.From },
            From = incomingSms.To
        });
        _logger.LogInformation("The reply was sent");
    }
}
