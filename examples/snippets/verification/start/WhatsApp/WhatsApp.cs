using Sinch;
using Sinch.Snippets.Shared;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using System.Text.Json;

var applicationKey = ConfigurationHelper.GetApplicationKey() ?? "MY_APPLICATION_KEY";
var applicationSecret = ConfigurationHelper.GetApplicationSecret() ?? "MY_APPLICATION_SECRET";

// The phone number you want to verify, in E.164 format (e.g. +46701234567).
const string phoneNumber = "PHONE_NUMBER";

var client = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = applicationKey,
        AppSecret = applicationSecret
    }
});

var sinchVerificationClient = client.Verification;

var request = new StartWhatsAppVerificationRequest()
{
    Identity = Identity.Number(phoneNumber)
};

Console.WriteLine($"Start a verification by WhatsApp onto phone number {phoneNumber}");

var response = await sinchVerificationClient.Verification.StartWhatsApp(request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
