using System.Text.Json;
using Sinch;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;

var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

// the recipient's phone number of the verification
var destinationPhoneNumber = "PHONE_NUMBER";
// the received verification code
var code = "CODE";

var verificationService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).Verification;

var request = new ReportWhatsAppVerificationRequest()
{
    WhatsApp = new WhatsApp()
    {
        Code = code
    }
};
ReportWhatsAppVerificationResponse response = await verificationService.ReportWhatsAppByIdentity(destinationPhoneNumber, request);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
