# [![Sinch Logo](https://developers.sinch.com/static/logo-07afe977d6d9dcd21b066d1612978e5c.svg)](https://www.sinch.com)

# .NET SDK

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://github.com/sinch/sinch-sdk-dotnet/blob/main/LICENSE)

[![.NET 5.0](https://img.shields.io/badge/.NET-5.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
[![.NET 7.0](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

# Welcome to Sinch's .NET SDK.

Here you'll find documentation to start developing C# code using Sinch services.

To use this SDK you'll need a Sinch account and API keys. Please sign up at [sinch.com](https://sinch.com)

For more in depth version of the Sinch APIs, please refer to the official developer
portal - [developers.sinch.com](https://developers.sinch.com/)

>[!WARNING]
>This SDK is currently available as a technical preview. It is being provided for the purpose of
collecting feedback, and should not be used in production environments.

* [Installation](#installation)
* [Getting started](#getting-started)
* [Products](#products)
* [Handling Exceptions](#handling-exceptions)
* [Client customization options](#logging-httpclient-and-additional-options)

# Installation

SinchSDK can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Sinch
```

# Getting started

## Client initialization

To initialize communication with Sinch backed, credentials obtained from Sinch portal have to be provided to the main
client class of this SDK.

> [!NOTE] 
> Always store your credentials securely as an environment variables or with a [secret manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)

```csharp
using Sinch;

var sinch = new SinchClient(configuration["Sinch:KeyId"], configuration["Sinch:KeySecret"], configuration["Sinch:ProjectId"]);
```
To configure Conversation and SMS regions, use `options`:
```csharp
var sinch = new SinchClient(
    configuration["Sinch:KeyId"],
    configuration["Sinch:KeySecret"], 
    configuration["Sinch:ProjectId"],
    options =>
    {
        options.SmsRegion = Sinch.SMS.SmsRegion.Eu;
        options.ConversationRegion = Sinch.Conversation.ConversationRegion.Eu;
    });
```

With ASP.NET dependency injection:

```csharp
// SinchClient is thread safe so it's okay to add it as a singleton
builder.Services.AddSingleton<ISinch>(x => new SinchClient(
    builder.Configuration["Sinch:KeyId"],
    builder.Configuration["Sinch:KeySecret"],
    builder.Configuration["Sinch:ProjectId"]));
```


## Products

Sinch client provides access to the following Sinch products:

- Numbers
- SMS
- Work-in-Progress Conversation API

Usage example of the `numbers` product:

```csharp
Sinch.Numbers.Active.List.Response response = await sinch.Numbers.Active.List(new Sinch.Numbers.Active.List.Request
{
    RegionCode = "US",
    Type = Types.Mobile
});

```

## Handling exceptions

For an unsuccessful API calls `ApiException` will be thrown.

```csharp
using Sinch;

try {
    var batch = await sinch.Sms.Batches.Send(new Sinch.SMS.Batches.Send.Request
    {
        Body = "Hello, World!",
        DeliveryReport = DeliveryReport.None,
        To = new List<string>()
        {
            123456789
        }
    });
}
catch(ApiException e) 
{
    logger.LogError("Api Exception. Status: {status}. Detailed message: {message}", e.Status, e.DetailedMessage);
}
```

## Logging, HttpClient, and additional options

To configure logger, provide own `HttpClient`, and additional options utilize `SinchOptions` within constructor:

```csharp
var sinch = new SinchClient(
    configuration["Sinch:KeyId"],
    configuration["Sinch:KeySecret"], 
    configuration["Sinch:ProjectId"],
    options =>
    {
        // provde any logger factory which satisfies Microsoft.Extensions.Logging.ILoggerFactory
        options.LoggerFactory = LoggerFactory.Create(config => { 
            // add log output to console
            config.AddConsole();
        });
        // Provide your http client here
        options.HttpClient = new HttpClient();
        // Set a region for SMS product
        options.SmsRegion = Sinch.SMS.SmsRegion.Eu;
    });
```

