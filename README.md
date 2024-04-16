# Sinch .NET SDK

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://github.com/sinch/sinch-sdk-dotnet/blob/main/LICENSE)

[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
[![.NET 7.0](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

Here you'll find documentation related to the Sinch .NET SDK, including how to install it, initialize it, and start developing .NET code using Sinch services.

To use Sinch services, you'll need a Sinch account and access keys. You can sign up for an account and create access keys at [dashboard.sinch.com](https://dashboard.sinch.com).

For more information on the Sinch APIs on which this SDK is based, refer to the official [developer documentation portal](https://developers.sinch.com/).

> [!WARNING]
> This SDK is currently available to selected developers for preview use only. It is being provided for the purpose of collecting feedback, and should not be used in production environments.

- [Installation](#installation)
- [Getting started](#getting-started)
  - [Client initialization](#client-initialization)
- [Supported Sinch products](#supported-sinch-products)
- [Logging, HttpClient and additional options](#logging-httpclient-and-additional-options)
- [Handling Exceptions](#handling-exceptions)

## Installation

SinchSDK can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Sinch --prerelease
```

## Getting started

Once the SDK is installed, you must start by initializing the main client class.

### Client initialization

To initialize communication with the Sinch servers, credentials obtained from the Sinch dashboard must be provided to the main client class of this SDK. It's highly recommended to not hardcode these credentials and to load them from environment variables instead or any key-secret storage (for example, [app-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)).

https://github.com/sinch/sinch-sdk-dotnet/blob/9f69eb2c5da48d5678d0f28ec4c039dd816f36d7/examples/Console/Program.cs#L8-L10

With ASP.NET dependency injection:

https://github.com/sinch/sinch-sdk-dotnet/blob/9f69eb2c5da48d5678d0f28ec4c039dd816f36d7/examples/WebApi/Program.cs#L17-L25

To configure Conversation or Sms hosting regions, and any other additional parameters, use [`SinchOptions`](https://github.com/sinch/sinch-sdk-dotnet/blob/main/src/Sinch/SinchOptions.cs):

https://github.com/sinch/sinch-sdk-dotnet/blob/4ca70fc3df975f213c822a66a0e6775d3ddee23d/examples/Console/UsingSinchOptions.cs#L9-L16

## Supported Sinch Products

Sinch client provides access to the following Sinch products:

- [SMS](https://developers.sinch.com/docs/sms/)
- [Conversation](https://developers.sinch.com/docs/conversation/)
- [Numbers](https://developers.sinch.com/docs/numbers/)
- [Verification](https://developers.sinch.com/docs/verification/)
- [Voice](https://developers.sinch.com/docs/voice/)
- additional products coming soon!

Usage example of the `numbers` product, assuming `sinch` is a type of `ISinchClient`:
```csharp
using Sinch.Numbers.Active.List;

ListActiveNumbersResponse response = await sinch.Numbers.Active.List(new ListActiveNumbersRequest
{
    RegionCode = "US",
    Type = Types.Mobile
});
```

## Logging, HttpClient, and additional options

To configure a logger, provide your own `HttpClient`, or any additional options utilize `SinchOptions` action within the constructor:

```csharp
using Sinch;
using Sinch.SMS;

var sinch = new SinchClient(
    configuration["Sinch:KeyId"],
    configuration["Sinch:KeySecret"],
    configuration["Sinch:ProjectId"],
    options =>
    {
        // provide any logger factory which satisfies Microsoft.Extensions.Logging.ILoggerFactory
        options.LoggerFactory = LoggerFactory.Create(config => {
            // add log output to console
            config.AddConsole();
        });
        // Provide your http client here
        options.HttpClient = new HttpClient();
        // Set a hosting region for Sms
        options.SmsHostingRegion = SmsHostingRegion.Eu;
    });
```

## Handling exceptions

For an unsuccessful API calls `SinchApiException` will be thrown:

```csharp
using Sinch;
using Sinch.SMS.Batches.Send;

try {
    var batch = await sinch.Sms.Batches.Send(new SendBatchRequest
    {
        Body = "Hello, World!",
        DeliveryReport = DeliveryReport.None,
        To = new List<string>()
        {
            123456789
        }
    });
}
catch(SinchApiException e)
{
    logger.LogError("Api Exception. Status: {status}. Detailed message: {message}", e.Status, e.DetailedMessage);
}
```

## Sample apps

For additional examples see [examples](https://github.com/sinch/sinch-sdk-dotnet/tree/main/examples)

## License

This project is licensed under the Apache License. See the [LICENSE](license) file for the license text.



