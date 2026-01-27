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
                await HandleTextMessage(textSms);
                break;
            case BinaryMessage binarySms:
                await HandleBinaryMessage(binarySms);
                break;
            case MediaMessage mediaSms:
                await HandleMediaMessage(mediaSms);
                break;
            case BatchDeliveryReportSms deliveryReport:
                await HandleBatchDeliveryReport(deliveryReport);
                break;
            case BatchDeliveryReportMms deliveryReportMms:
                await HandleBatchDeliveryReportMms(deliveryReportMms);
                break;
            case RecipientDeliveryReportSms recipientDeliveryReport:
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

    private Task HandleTextMessage(TextMessage message)
    {
        _logger.LogInformation("Received text SMS from {From}: {Body}", message.From, message.Body);
        return Task.CompletedTask;
    }

    private Task HandleBinaryMessage(BinaryMessage message)
    {
        _logger.LogInformation("Received binary SMS from {From}", message.From);
        return Task.CompletedTask;
    }

    private Task HandleMediaMessage(MediaMessage message)
    {
        _logger.LogInformation("Received MMS from {From} with {Count} media items", message.From, message.MessageBody.Media?.Count ?? 0);
        return Task.CompletedTask;
    }

    private Task HandleBatchDeliveryReport(BatchDeliveryReportSms reportSms)
    {
        _logger.LogInformation("Received batch report: {Type}", reportSms.Type);
        return Task.CompletedTask;
    }

    private Task HandleBatchDeliveryReportMms(BatchDeliveryReportMms report)
    {
        _logger.LogInformation("Received batch report: {Type}", report.Type);
        return Task.CompletedTask;
    }

    private Task HandleRecipientDeliveryReport(RecipientDeliveryReportSms reportSms)
    {
        _logger.LogInformation("Received recipient report: {Recipient}", reportSms.Recipient);
        return Task.CompletedTask;
    }

    private Task HandleRecipientDeliveryReportMms(RecipientDeliveryReportMms report)
    {
        _logger.LogInformation("Received recipient report: {Recipient}", report.Recipient);
        return Task.CompletedTask;
    }
}
