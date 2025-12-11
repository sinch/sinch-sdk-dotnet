# Sinch .NET SDK

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://github.com/sinch/sinch-sdk-dotnet/blob/main/LICENSE)

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

Here you'll find documentation related to the Sinch .NET SDK, including how to install it, initialize it, and start developing .NET code using Sinch services.

To use Sinch services, you'll need a Sinch account and access keys. You can sign up for an account and create access keys at [dashboard.sinch.com](https://dashboard.sinch.com).

For more information on the Sinch APIs on which this SDK is based, refer to the official [developer documentation portal](https://developers.sinch.com/).

- [Installation](#installation)
- [Getting started](#getting-started)
- [Client initialization](#client-initialization)
- [Migrating to 2.0](#migrating-to-20)
- [Supported Sinch Products](#supported-sinch-products)
- [Logging and additional options](#logging-and-additional-options)
- [Handling exceptions](#handling-exceptions)
- [Sample apps](#sample-apps)
- [License](#license)

## Installation

SinchSDK can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Sinch
```

## Getting started

Once the SDK is installed, you must start by initializing the main client class.

### Client initialization

To initialize communication with the Sinch servers, credentials obtained from the Sinch dashboard must be provided to the main client class of this SDK. It's highly recommended to not hardcode these credentials and to load them from environment variables instead or any key-secret storage (for example, [app-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)).

Console application:

```csharp
using Sinch;

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = "PROJECT_ID",
        KeyId = "KEY_ID",
        KeySecret = "KEY_SECRET"
    }
});
```

With ASP.NET dependency injection:

```csharp
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = "PROJECT_ID",
        KeyId = "KEY_ID",
        KeySecret = "KEY_SECRET"
    }
});
```

To configure Conversation or Sms hosting regions, and any other additional parameters, use the dedicated configuration classes:

```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SmsConfiguration = new SinchSmsConfiguration
    {
        Region = Sinch.SMS.SmsRegion.Eu
    },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        ConversationRegion = Sinch.Conversation.ConversationRegion.Eu
    }
});
```
## Migrating to 2.0

If you are upgrading from version 1, please refer to the [migration guide](MIGRATION_GUIDE.md) for detailed instructions. Version 2.0 introduces a new configuration-based initialization pattern using `SinchClientConfiguration`, changes to Voice and Verification client setup, and updates to how regions and options are configured.

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

## Logging and additional options

To configure a logger for console applications, use `SinchOptions` within the configuration:

```csharp
using Sinch;

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchOptions = new SinchOptions
    {
        LoggerFactory = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        })
    }
});
```

For ASP.NET Core applications, the SDK automatically uses `ILoggerFactory` from the DI container when using `AddSinchClient()`.

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



