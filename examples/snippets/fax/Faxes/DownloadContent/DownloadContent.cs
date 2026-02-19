/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates downloading the PDF content of a fax.
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

Console.WriteLine("Downloading fax content as PDF");

const string faxId = "FAX_ID";

var contentResult = await sinchClient.Fax.Faxes.DownloadContent(faxId);

// Save the PDF to a file
var outputPath = Path.Combine(Directory.GetCurrentDirectory(), contentResult.FileName ?? "fax.pdf");
await using var fileStream = File.Create(outputPath);
await contentResult.Stream.CopyToAsync(fileStream);

Console.WriteLine($"Fax content saved to: {outputPath}");
