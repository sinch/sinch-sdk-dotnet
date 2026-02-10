/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
/// 
using Sinch;
using Sinch.Core;
using Sinch.Fax.Services;
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

const string serviceId = "my-service-id";

Console.WriteLine($"Updating fax service: {serviceId}");

var request = new UpdateServiceRequest
{
    Id = serviceId,
    Name = "Updated Fax Service Name"
};

var response = await sinchClient.Fax.Services.Update(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
