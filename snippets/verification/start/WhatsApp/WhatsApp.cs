using System.Text.Json;
using Sinch;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;


var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

var destinationPhoneNumber = "PHONE_NUMBER_TO_SEND_TO";

var verificationService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).Verification;

var request = new StartWhatsAppVerificationRequest
{
    Identity = Identity.Number(destinationPhoneNumber)
};
StartWhatsAppVerificationResponse response = await verificationService.StartWhatsApp(request);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
