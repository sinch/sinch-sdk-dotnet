using Sinch.SMS.Hooks;

namespace Webhook.Template.Sms;

public class ServerBusinessLogic(ILogger<ServerBusinessLogic> logger)
{
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
            case BatchDeliveryReportSms deliveryReport:
                await HandleIncomingBatchDeliveryReport(deliveryReport);
                break;
            case BatchDeliveryReportMms deliveryReportMms:
                await HandleIncomingBatchDeliveryReportMms(deliveryReportMms);
                break;
            case RecipientDeliveryReportSms recipientDeliveryReport:
                await HandleIncomingRecipientDeliveryReport(recipientDeliveryReport);
                break;
            case RecipientDeliveryReportMms recipientDeliveryReportMms:
                await HandleIncomingRecipientDeliveryReportMms(recipientDeliveryReportMms);
                break;
            default:
                logger.LogWarning("Unknown SMS event type: {Type}", smsEvent.GetType());
                break;
        }
    }

    private Task HandleIncomingTextMessage(TextMessage message)
    {
        logger.LogInformation("Received text SMS from {From}: {Body}", message.From, message.Body);
        return Task.CompletedTask;
    }

    private Task HandleIncomingBinaryMessage(BinaryMessage message)
    {
        logger.LogInformation("Received binary SMS from {From}", message.From);
        return Task.CompletedTask;
    }

    private Task HandleIncomingMediaMessage(MediaMessage message)
    {
        logger.LogInformation("Received MMS from {From} with {Count} media items", message.From, message.MessageBody.Media?.Count ?? 0);
        return Task.CompletedTask;
    }

    private Task HandleIncomingBatchDeliveryReport(BatchDeliveryReportSms report)
    {
        logger.LogInformation("Received batch SMS report: {Type}", report.Type);
        return Task.CompletedTask;
    }

    private Task HandleIncomingBatchDeliveryReportMms(BatchDeliveryReportMms report)
    {
        logger.LogInformation("Received batch MMS report: {Type}", report.Type);
        return Task.CompletedTask;
    }

    private Task HandleIncomingRecipientDeliveryReport(RecipientDeliveryReportSms report)
    {
        logger.LogInformation("Received recipient SMS report: {Recipient}", report.Recipient);
        return Task.CompletedTask;
    }

    private Task HandleIncomingRecipientDeliveryReportMms(RecipientDeliveryReportMms report)
    {
        logger.LogInformation("Received recipient MMS report: {Recipient}", report.Recipient);
        return Task.CompletedTask;
    }
}
