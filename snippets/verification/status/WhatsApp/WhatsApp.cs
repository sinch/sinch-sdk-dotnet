using System.Text.Json;
using Sinch;
using Sinch.Verification.Status;

var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

var destinationPhoneNumber = "PHONE_NUMBER_TO_SEND_TO";

var statusService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).VerificationStatus;

WhatsAppVerificationStatusResponse response = await statusService.GetWhatsAppByIdentity(destinationPhoneNumber);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
