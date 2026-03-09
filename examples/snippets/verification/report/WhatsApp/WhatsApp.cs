using Sinch;
using Sinch.Snippets.Shared;
using Sinch.Verification;
using Sinch.Verification.Report.Request;
using System.Text.Json;

var applicationKey = ConfigurationHelper.GetApplicationKey() ?? "MY_APPLICATION_KEY";
var applicationSecret = ConfigurationHelper.GetApplicationSecret() ?? "MY_APPLICATION_SECRET";

// The phone number being verified via Whatsapp.
const string phoneNumber = "PHONE_NUMBER";

// The OTP is the code the user received via Whatsapp as part of the verification process.
const string code = "OTP_CODE";

var client = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = applicationKey,
        AppSecret = applicationSecret
    }
});

var verificationClient = client.Verification;

var request = new ReportWhatsAppVerificationRequest()
{
    WhatsApp = new WhatsApp()
    {
        Code = code
    }
};

Console.WriteLine($"Report Whatsapp verification code for phone number {phoneNumber}");

var response = await verificationClient.Verification.ReportWhatsAppByIdentity(phoneNumber, request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
