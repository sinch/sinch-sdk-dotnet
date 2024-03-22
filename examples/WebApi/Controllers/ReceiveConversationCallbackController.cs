using System.Text;
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
    public ActionResult Handle([FromBody] EventInboundAllOfEvent messageInboundEvent)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var rawJson = reader.ReadToEndAsync().Result;
        
        var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value);
        if (!_sinch.Conversation.Webhooks.ValidateAuthenticationHeader(headers, JsonNode.Parse(rawJson).AsObject(), Secret))
        {
            _logger?.LogError("Failed to authorize received callback.");
            return Unauthorized();
        }

        // do something with messageInboundEvent
        return Ok();
    }
}
