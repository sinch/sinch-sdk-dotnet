using Sinch;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using System.Text.Json;

// The phone number you want to verify, in E.164 format (e.g. +46701234567).
var phoneNumber = "PHONE_NUMBER";

var sinch = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = Environment.GetEnvironmentVariable("SINCH_APPLICATION_KEY") ?? "MY_APPLICATION_KEY",
        AppSecret = Environment.GetEnvironmentVariable("SINCH_APPLICATION_SECRET") ?? "MY_APPLICATION_SECRET"
    }
});

var sinchVerificationClient = sinch.Verification;

Console.WriteLine($"Start a verification by WhatsApp onto phone number {phoneNumber}");

var request = new StartWhatsAppVerificationRequest()
{
    Identity = Identity.Number(phoneNumber)
};

var response = await sinchVerificationClient.Verification.StartWhatsApp(request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
