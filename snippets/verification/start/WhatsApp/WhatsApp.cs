using System.Text.Json;
using Sinch;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;


var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

// the recipient's phone number of the verification
var destinationPhoneNumber = "PHONE_NUMBER";

var verificationService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).Verification;

var request = new StartWhatsAppVerificationRequest()
{
    Identity = Identity.Number(destinationPhoneNumber)
};
var response = await verificationService.StartWhatsApp(request);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
