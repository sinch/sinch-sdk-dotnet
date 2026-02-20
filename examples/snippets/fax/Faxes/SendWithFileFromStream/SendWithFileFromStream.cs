/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates sending faxes with binary file content from a Stream.
/// This method uses a Stream (typically from reading a file) to send the fax content
/// along with optional contentUrl parameters.
/// Supports both single and multiple recipients.
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using System.Text.Json;
using Sinch;
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

const string singleRecipient = "+46123456789";
const string senderNumber = "+46123456789"; // Must be a number associated with your account
const string multipleRecipient1 = "+46123456789";
const string multipleRecipient2 = "+46987654321";
const string fileName = "document.pdf";

// Example 1: Send to a single recipient with file from stream
Console.WriteLine("Example 1: Sending a fax with file from stream to a single recipient");

var fileBytes = new byte[] { /* Your file bytes here */ };
await using var singleRecipientStream = new MemoryStream(fileBytes);
using var singleRecipientRequest = new SendFaxRequest(singleRecipientStream, fileName)
{
    To = new List<string> { singleRecipient },
    From = senderNumber
};

var singleRecipientResponse = await sinchClient.Fax.Faxes.Send(singleRecipientRequest);

Console.WriteLine($"Single recipient response: {JsonSerializer.Serialize(singleRecipientResponse, new JsonSerializerOptions { WriteIndented = true })}");

// Example 2: Send to multiple recipients with file from stream
Console.WriteLine("\nExample 2: Sending a fax with file from stream to multiple recipients");

// Create a new stream and request for the second example
fileBytes = new byte[] { /* Your file bytes here */ };
await using var multipleRecipientsStream = new MemoryStream(fileBytes);
using var multipleRecipientsRequest = new SendFaxRequest(multipleRecipientsStream, fileName)
{
    To = new List<string> { multipleRecipient1, multipleRecipient2 },
    From = senderNumber
};

var multipleRecipientsResponse = await sinchClient.Fax.Faxes.Send(multipleRecipientsRequest);

Console.WriteLine($"Multiple recipients response: {JsonSerializer.Serialize(multipleRecipientsResponse, new JsonSerializerOptions { WriteIndented = true })}");
