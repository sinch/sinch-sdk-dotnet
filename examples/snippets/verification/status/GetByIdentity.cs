using Sinch;
using Sinch.Verification;
using Sinch.Verification.Common;
using System.Text.Json;

// The phone number you are verifying, in E.164 format (e.g. +46701234567).
// This should be the same number you used when starting the verification.
var phoneNumber = "PHONE_NUMBER";

// The verification method you used when starting the verification.
var verificationMethod = VerificationMethod.Sms;

var sinch = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = Environment.GetEnvironmentVariable("SINCH_APPLICATION_KEY") ?? "MY_APPLICATION_KEY",
        AppSecret = Environment.GetEnvironmentVariable("SINCH_APPLICATION_SECRET") ?? "MY_APPLICATION_SECRET"
    }
});

Console.WriteLine($"Verification status for phone number {phoneNumber}");

var sinchVerificationClient = sinch.Verification;
var verificationStatus = sinchVerificationClient.VerificationStatus;

var response = await verificationStatus.GetByIdentity(phoneNumber, verificationMethod);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
