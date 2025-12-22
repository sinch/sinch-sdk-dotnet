using Sinch;
using Sinch.Verification;
using Sinch.Verification.Report.Request;
using System.Text.Json;

// The phone number being verified via Whatsapp.
var phoneNumber = "PHONE_NUMBER";

// The OTP is the code the user received via Whatsapp as part of the verification process.
var code = "OTP_CODE";

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = Environment.GetEnvironmentVariable("SINCH_APPLICATION_KEY") ?? "MY_APPLICATION_KEY",
        AppSecret = Environment.GetEnvironmentVariable("SINCH_APPLICATION_SECRET") ?? "MY_APPLICATION_SECRET"
    }
});

var verificationService = sinchClient.Verification;

Console.WriteLine($"Report Whatsapp verification code for phone number {phoneNumber}");

var request = new ReportWhatsAppVerificationRequest()
{
    WhatsApp = new WhatsApp()
    {
        Code = code
    }
};

var response = await verificationService.Verification.ReportWhatsAppByIdentity(phoneNumber, request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
