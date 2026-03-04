using Sinch;
using Sinch.Snippets.Shared;
using Sinch.Verification;
using Sinch.Verification.Common;
using System.Text.Json;

var applicationKey = ConfigurationHelper.GetApplicationKey() ?? "MY_APPLICATION_KEY";
var applicationSecret = ConfigurationHelper.GetApplicationSecret() ?? "MY_APPLICATION_SECRET";

// The phone number you are verifying, in E.164 format (e.g. +46701234567).
// This should be the same number you used when starting the verification.
var phoneNumber = "PHONE_NUMBER";

// The verification method you used when starting the verification.
var verificationMethod = VerificationMethod.Sms;

var client = new SinchClient(new SinchClientConfiguration()
{
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = applicationKey,
        AppSecret = applicationSecret
    }
});

var verificationStatus = client.Verification.VerificationStatus;

Console.WriteLine($"Verification status for phone number {phoneNumber}");

var response = await verificationStatus.GetByIdentity(phoneNumber, verificationMethod);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
