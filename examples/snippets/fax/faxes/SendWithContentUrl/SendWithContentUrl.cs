/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Core;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

const string recipient = "RECIPIENT_PHONE_NUMBER";
const string faxContentUrl = "https://developers.sinch.com/fax/fax.pdf";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    }
});

Console.WriteLine("Sending a fax with contentUrl to a recipient");

var response = await client.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = [recipient],
        ContentUrl = [faxContentUrl]
    }
);

Console.WriteLine($"Response: {response.ToPrettyString()}");
