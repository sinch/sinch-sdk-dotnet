/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Core;
using Sinch.Fax;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var faxRegion = ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION";

// The phone number of the recipient you want to send a fax to
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";
const string faxContentUrl = "https://developers.sinch.com/fax/fax.pdf";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    FaxConfiguration = new SinchFaxConfiguration
    {
        Region = new FaxRegion(faxRegion)
    }
});

Console.WriteLine("Sending a fax with contentUrl to a recipient");

var response = await client.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = [recipientPhoneNumber],
        ContentUrl = [faxContentUrl]
    }
);

Console.WriteLine($"Response: {response.ToPrettyString()}");
