/// <summary>
/// Sinch .NET SDK Snippet
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Microsoft.Extensions.Logging;
using Sinch;
using Sinch.Core;
using Sinch.Snippets.Shared;

var logger = LoggerHelper.Logger;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

var phoneNumber = ConfigurationHelper.GetPhoneNumber() ?? "MY_SINCH_PHONE_NUMBER";

logger.LogInformation("Get for: {PhoneNumber}", phoneNumber);

var response = await sinchClient.Numbers.Get(phoneNumber);

logger.LogInformation("Response: {Response}", response.ToPrettyString());
