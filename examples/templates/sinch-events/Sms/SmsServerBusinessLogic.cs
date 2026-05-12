using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Inbounds;
using Sinch.SMS.SinchEvents;

namespace SinchEvents.Template.Sms;

public class SmsServerBusinessLogic(ILogger<SmsServerBusinessLogic> logger)
{
    public async Task HandleEvent(ISmsSinchEvent smsEvent)
    {
        switch (smsEvent)
        {
            case SmsInbound textSms:
                await HandleIncomingTextMessage(textSms);
                break;
            case BinaryInbound binarySms:
                await HandleIncomingBinaryMessage(binarySms);
                break;
            case MediaInbound mediaSms:
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

    private Task HandleIncomingTextMessage(SmsInbound message)
    {
        logger.LogInformation("Received text SMS from {From}: {Body}", message.From, message.Body);
        return Task.CompletedTask;
    }

    private Task HandleIncomingBinaryMessage(BinaryInbound message)
    {
        logger.LogInformation("Received binary SMS from {From}", message.From);
        return Task.CompletedTask;
    }

    private Task HandleIncomingMediaMessage(MediaInbound message)
    {
        logger.LogInformation("Received MMS from {From} with {Count} media items", message.From, message.Body.Media?.Count ?? 0);
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
