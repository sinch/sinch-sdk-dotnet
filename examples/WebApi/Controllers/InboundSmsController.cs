using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.SMS.Batches.Send;
using Sinch.SMS.Groups.Create;
using Sinch.SMS.Groups.Update;
using Sinch.SMS.Hooks;
using DeliveryReport = Sinch.SMS.DeliveryReport;

namespace WebApiExamples.Controllers;

[ApiController]
[Route("reply")]
public class InboundSmsController : ControllerBase
{
    private readonly ISinchClient _sinchClient;
    private readonly ILogger _logger;

    public InboundSmsController(ISinchClient sinchClient, ILogger<InboundSmsController> logger)
    {
        _sinchClient = sinchClient;
        _logger = logger;
    }

    [HttpPost]
    [Route("subscription")]
    public async Task Subscribe([FromBody] IncomingTextSms incomingSms)
    {
        var group = await _sinchClient.Sms.Groups.Create(new CreateGroupRequest() { Name = "Pirates of Sinch" });
        var fromNumber = incomingSms.From;
        var toNumber = incomingSms.To;
        string autoReply;
        var inboundMessage = incomingSms.Body;

        if (group.Id is null)
        {
            _logger.LogError("GroupId is null.");
            return;
        }

        var groupNumbers = await _sinchClient.Sms.Groups.ListMembers(group.Id);
        switch (groupNumbers.Contains(fromNumber), inboundMessage)
        {
            case (false, "SUBSCRIBE"):
                await _sinchClient.Sms.Groups.Update(new UpdateGroupRequest
                {
                    GroupId = group.Id,
                    Name = "group 1",
                    Add = new List<string>()
                        {
                            fromNumber,
                        }
                }
                );
                autoReply = $"Congratulations! You are now subscribed to {group.Name}. Text STOP to leave this group.";
                break;
            case (true, "STOP"):
                await _sinchClient.Sms.Groups.Update(new UpdateGroupRequest
                {
                    GroupId = group.Id,
                    Name = "group 1",
                    Remove = new List<string>()
                    {
                        fromNumber,
                    },
                });
                autoReply =
                    $"We're sorry to see you go. You can always rejoin {group.Name} by texting \"SUBSCRIBE\" to {toNumber}";
                break;
            default:
                autoReply =
                    $"Thanks for your interest. If you want to subscribe to this group, text \"SUBSCRIBE\"  to {toNumber}";
                break;
        }

        var response = await _sinchClient.Sms.Batches.Send(new SendTextBatchRequest()
        {
            Body = autoReply,
            DeliveryReport = DeliveryReport.None,
            To = new List<string>() { fromNumber },
            From = toNumber
        });
        _logger.LogInformation(JsonSerializer.Serialize(response, new JsonSerializerOptions()
        {
            WriteIndented = true
        }));
    }
}
