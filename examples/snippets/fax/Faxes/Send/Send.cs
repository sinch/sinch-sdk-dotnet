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

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

// Recipient phone numbers
const string recipient1PhoneNumber = "RECIPIENT1_PHONE_NUMBER";
const string recipient2PhoneNumber = "RECIPIENT2_PHONE_NUMBER";

Console.WriteLine("Sending a fax with contentUrl");

var response = await sinchClient.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = [recipient1PhoneNumber, recipient2PhoneNumber],
        ContentUrl = ["https://developers.sinch.com/fax/fax.pdf"]
    }
);

foreach (var fax in response)
{
    Console.WriteLine($"Response: {fax.ToPrettyString()}");
}

