using System.Text.Json;
using Sinch;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;

var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

var destinationPhoneNumber = "PHONE_NUMBER_TO_SEND_TO";

var verificationService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).Verification;

var request = new ReportWhatsAppVerificationRequest()
{
    WhatsApp = new WhatsApp()
    {
        Code = "A CODE"
    }
};
ReportWhatsAppVerificationResponse response = await verificationService.ReportWhatsAppByIdentity(destinationPhoneNumber, request);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
