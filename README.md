# Sinch .NET SDK

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://github.com/sinch/sinch-sdk-dotnet/blob/main/LICENSE)

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

Here you'll find documentation related to the Sinch .NET SDK, including how to install it, initialize it, and start developing .NET code using Sinch services.

To use Sinch services, you'll need a Sinch account and access keys. You can sign up for an account and create access keys at [dashboard.sinch.com](https://dashboard.sinch.com).

For more information on the Sinch APIs on which this SDK is based, refer to the official [developer documentation portal](https://developers.sinch.com/).

- [Installation](#installation)
- [Getting started](#getting-started)
- [Migrating to 2.0](#)
- [Client initialization](#client-initialization)
- [Supported Sinch products](#supported-sinch-products)
- [Logging, HttpClient and additional options](#logging-httpclient-and-additional-options)
- [Handling Exceptions](#handling-exceptions)

## [Migrating to 2.0](docs/MigrationTo2.0.md)

## Installation

SinchSDK can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Sinch
```

## Getting started

Once the SDK is installed, you must start by initializing the main client class.

### Client initialization

To initialize communication with the Sinch servers, credentials obtained from the Sinch dashboard must be provided to the main client class of this SDK. It's highly recommended to not hardcode these credentials and to load them from environment variables instead or any key-secret storage (for example, [app-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)).

```csharp
using Sinch;

var sinch = new SinchClient(configuration["Sinch:ProjectId"], configuration["Sinch:KeyId"], configuration["Sinch:KeySecret"]);
```

With ASP.NET dependency injection:

```csharp
// SinchClient is thread safe so it's okay to add it as a singleton
builder.Services.AddSingleton<ISinch>(x => new SinchClient(
    builder.Configuration["Sinch:ProjectId"],
    builder.Configuration["Sinch:KeyId"],
    builder.Configuration["Sinch:KeySecret"]
));
```

To configure Conversation or Sms hosting regions, and any other additional parameters, use [`SinchOptions`](https://github.com/sinch/sinch-sdk-dotnet/blob/main/src/Sinch/SinchOptions.cs):

```csharp
var sinch = new SinchClient(
    configuration["Sinch:ProjectId"],
    configuration["Sinch:KeyId"],
    configuration["Sinch:KeySecret"],
    options =>
    {
        options.SmsRegion = Sinch.SMS.SmsRegion.Eu;
        options.ConversationRegion = Sinch.Conversation.ConversationRegion.Eu;
    });
```
## [Migrating to 2.0](docs/MigrationTo2.0.md)

If you are upgrading from version 1.*, please refer to the [migration guide](docs/MigrationTo2.0.md) for detailed instructions. Version 2.0 introduces a new configuration-based initialization pattern using `SinchClientConfiguration`, changes to Voice and Verification client setup, and updates to how regions and options are configured.

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
    configuration["Sinch:ProjectId"],
    configuration["Sinch:KeyId"],
    configuration["Sinch:KeySecret"],
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
        options.SmsRegion = SmsRegion.Eu;
    });
```

## Handling exceptions

For an unsuccessful API calls `SinchApiException` will be thrown:

```csharp
using Sinch;
using Sinch.SMS.Batches.Send;

try {
    var batch = await sinch.Sms.Batches.Send(new SendTextBatchRequest()
    {
        Body = "Hello, World!",
        To = new List<string>()
        {
            "+123456789"
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



