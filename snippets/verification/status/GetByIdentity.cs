using System.Text.Json;
using Sinch;
using Sinch.Verification.Status;
using Sinch.Verification.Common;

var applicationKey = Environment.GetEnvironmentVariable("MY_APPLICATION_KEY");
var applicationSecret = Environment.GetEnvironmentVariable("MY_APPLICATION_SECRET");

// Phone number that received the Verification
var destinationPhoneNumber = "PHONE_NUMBER";
// Verification method used for the Verification
var verificationMethod = VerificationMethod.Sms;

var statusService = new SinchClient(null, null, null).Verification(applicationKey, applicationSecret).VerificationStatus;

IVerificationStatusResponse response = await statusService.GetByIdentity(destinationPhoneNumber, verificationMethod);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
