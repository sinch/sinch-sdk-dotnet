using System.Threading.Tasks;
using Sinch.SMS.Hooks;

namespace SmsWebhookTemplate.Sms;

public class ServerBusinessLogic
{
    private readonly ILogger<ServerBusinessLogic> _logger;

    public ServerBusinessLogic(ILogger<ServerBusinessLogic> logger)
    {
        _logger = logger;
    }

    public async Task HandleEvent(ISmsEvent smsEvent)
    {
        switch (smsEvent)
        {
            case TextMessage textSms:
                await HandleIncomingTextMessage(textSms);
                break;
            case BinaryMessage binarySms:
                await HandleIncomingBinaryMessage(binarySms);
                break;
            case MediaMessage mediaSms:
                await HandleIncomingMediaMessage(mediaSms);
                break;
            case DeliveryReport deliveryReport:
                await HandleBatchDeliveryReport(deliveryReport);
                break;
            case DeliveryReportMms deliveryReportMms:
                await HandleBatchDeliveryReportMms(deliveryReportMms);
                break;
            case RecipientDeliveryReport recipientDeliveryReport:
                await HandleRecipientDeliveryReport(recipientDeliveryReport);
                break;
            case RecipientDeliveryReportMms recipientDeliveryReportMms:
                await HandleRecipientDeliveryReportMms(recipientDeliveryReportMms);
                break;
            default:
                _logger.LogInformation("Unknown SMS event type: {Type}", smsEvent.GetType());
                break;
        }
    }

    private Task HandleIncomingTextMessage(TextMessage message)
    {
        _logger.LogInformation("Received text SMS from {From}: {Body}", message.From, message.Body);
        return Task.CompletedTask;
    }

    private Task HandleIncomingBinaryMessage(BinaryMessage message)
    {
        _logger.LogInformation("Received binary SMS from {From}", message.From);
        return Task.CompletedTask;
    }

    private Task HandleIncomingMediaMessage(MediaMessage message)
    {
        _logger.LogInformation("Received MMS from {From} with {Count} media items", message.From, message.Body.Media?.Count ?? 0);
        return Task.CompletedTask;
    }

    private Task HandleBatchDeliveryReport(DeliveryReport report)
    {
        _logger.LogInformation("Received batch report: {Type}", report.Type);
        return Task.CompletedTask;
    }
    
    private Task HandleBatchDeliveryReportMms(DeliveryReportMms report)
    {
        _logger.LogInformation("Received batch report: {Type}", report.Type);
        return Task.CompletedTask;
    }

    private Task HandleRecipientDeliveryReport(RecipientDeliveryReport report)
    {
        _logger.LogInformation("Received recipient report: {Recipient}", report.Recipient);
        return Task.CompletedTask;
    }
    
    private Task HandleRecipientDeliveryReportMms(RecipientDeliveryReportMms report)
    {
        _logger.LogInformation("Received recipient report: {Recipient}", report.Recipient);
        return Task.CompletedTask;
    }
}
