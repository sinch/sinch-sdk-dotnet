/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Snippets.Shared;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Core;

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

Console.WriteLine($"Start a verification by WhatsApp onto phone number {phoneNumber}");

var request = new StartWhatsAppVerificationRequest()
{
    Identity = Identity.Number(phoneNumber)
};

var response = await sinchVerificationClient.Verification.StartWhatsApp(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
