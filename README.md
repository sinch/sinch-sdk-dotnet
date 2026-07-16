# Sinch .NET SDK

[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
[![.NET 7.0](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![NuGet](https://img.shields.io/nuget/v/Sinch.svg)](https://www.nuget.org/packages/Sinch)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://github.com/sinch/sinch-sdk-dotnet/blob/main/LICENSE)

Here you'll find documentation related to the Sinch .NET SDK, including how to install it, initialize it, and start developing .NET code using Sinch services.

To use Sinch services, you'll need a Sinch account and access keys. You can sign up for an account and create access keys at [dashboard.sinch.com](https://dashboard.sinch.com).

For more information on the SDK, refer to the dedicated [.NET SDK documentation section](https://developers.sinch.com/docs/sdks/dotnet), and for the Sinch APIs on which this SDK is based, refer to the official [developer documentation portal](https://developers.sinch.com/).

## Table of contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Supported APIs](#supported-apis)
- [Getting started](#getting-started)
- [Logging](#logging)
- [Handling exceptions](#handling-exceptions)
- [Custom HTTP client implementation](#custom-http-client-implementation)
- [Third-party dependencies](#third-party-dependencies)
- [Examples](#examples)
- [Changelog & Migration](#changelog--migration)
- [License](#license)
- [Contact](#contact)

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), [.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0), or [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [NuGet](https://www.nuget.org/) or the `dotnet` CLI
- [Sinch account](https://dashboard.sinch.com/)

> **Warning**: This SDK is intended for server-side (backend) use only. Do not use it in front-end or client-side applications (web, mobile, or desktop), regardless of language or framework. Doing so can expose your Sinch credentials to end-users.

## Installation

Run the following command to install the SDK:

```bash
dotnet add package Sinch
```

## Supported APIs

| API Category    | API Name                                                            |
| --------------- | ------------------------------------------------------------------- |
| Messaging       | [Conversation API](https://developers.sinch.com/docs/conversation/) |
|                 | [SMS API](https://developers.sinch.com/docs/sms/)                     |
| Voice and Video | [Voice API](https://developers.sinch.com/docs/voice/)               |
| Numbers         | [Numbers API](https://developers.sinch.com/docs/numbers/)           |
| Verification    | [Verification API](https://developers.sinch.com/docs/verification/) |
| Fax             | [Fax API](https://developers.sinch.com/docs/fax/)                   |

> **Note:** The SMS API is end-of-sale. New integrations should use the [Conversation API](https://developers.sinch.com/docs/conversation/) instead, which supports SMS and many other channels.

## Getting started

### Client initialization

To start using the SDK, initialize the main client class. This client gives you access to all the SDK services:

```csharp
using Sinch;

// Warning: not all APIs support project authentication. Check the section for each API before using this snippet.

var sinch = new SinchClient(
    "SINCH_PROJECT_ID",
    "SINCH_KEY_ID",
    "SINCH_KEY_SECRET");
```

Get `SINCH_PROJECT_ID`, `SINCH_KEY_ID` and `SINCH_KEY_SECRET` from the [Access keys](https://dashboard.sinch.com/settings/access-keys) page in your Sinch dashboard (`SINCH_KEY_SECRET` is shown only once, at creation time). It's highly recommended to not hardcode these credentials: load them from environment variables for local development, and from a secret manager in production.

This snippet is the common starting point for project-based APIs. Some APIs need a different initialization or extra parameters (for example, a region or application credentials), see the section for each API below.

With ASP.NET dependency injection:

```csharp
// SinchClient is thread safe so it's okay to add it as a singleton
builder.Services.AddSingleton<ISinchClient>(_ => new SinchClient(
    builder.Configuration["Sinch:ProjectId"],
    builder.Configuration["Sinch:KeyId"],
    builder.Configuration["Sinch:KeySecret"]));
```

### Conversation API

The Conversation API is regionalized. To use this API, the `conversation_region` parameter is required:

```csharp
using Sinch;
using Sinch.Conversation;

var sinch = new SinchClient(
    "SINCH_PROJECT_ID",
    "SINCH_KEY_ID",
    "SINCH_KEY_SECRET",
    options =>
    {
        options.ConversationRegion = ConversationRegion.Eu;
    });
```

#### Sinch Events

The Conversation API delivers asynchronous Sinch Events to the Event Destination URL you configure for your app in the [Conversation dashboard](https://dashboard.sinch.com/convapi/apps). `ValidateAuthenticationHeader` confirms a request comes from Sinch and `ParseEvent` turns its payload into a typed event object; `headers` and `body` are the incoming request's headers and raw body:

```csharp
using System.Text.Json.Nodes;
using Sinch.Conversation.Hooks;

var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value);
var body = /* raw JSON body as JsonNode */;

bool validAuth = sinch.Conversation.Webhooks.ValidateAuthenticationHeader(headers, body, sinchEventsSecret);

ICallbackEvent callbackEvent = sinch.Conversation.Webhooks.ParseEvent(body);
```

`sinchEventsSecret` is set per app in the [Conversation dashboard](https://dashboard.sinch.com/convapi/apps). `ParseEvent` works without validating the request, but then its origin can't be verified, so validating is recommended in production.

You can find a complete example in [examples/WebApi/Controllers/ReceiveConversationCallbackController.cs](examples/WebApi/Controllers/ReceiveConversationCallbackController.cs).

### SMS API

> **Warning:** the SMS API is end-of-sale. For new integrations, prefer the [Conversation API](#conversation-api).

The SMS API is regionalized: set `sms_region` to the region where your SMS account is hosted. The accepted values depend on which credentials you use:

- **Project access keys**: available only in the `us` and `eu` regions. Use the same `project_id`, `key_id` and `key_secret` as the common client, plus `sms_region`:

```csharp
using Sinch;
using Sinch.SMS;

var sinch = new SinchClient(
    "SINCH_PROJECT_ID",
    "SINCH_KEY_ID",
    "SINCH_KEY_SECRET",
    options =>
    {
        options.SmsRegion = SmsRegion.Us;
    });
```

> **SMS authentication for new projects**
>
> Projects created after the SMS API end-of-sale (`15/04/26`) cannot use project access keys. The SMS API requests return `401 Unauthorized`.
>
> If you encounter this issue, consider the following options:
>
> 1. Use service plan credentials (`service_plan_id` + `sms_api_token`)
> 2. Use the Conversation API, which works with project access keys.
> 3. Contact your account manager

- **Service plan**: available in all regions (`us`, `eu`, `au`, `br`, `ca`). Use a `service_plan_id` and `sms_api_token`, both available on the [Service APIs dashboard](https://dashboard.sinch.com/sms/api/services):

```csharp
using Sinch;
using Sinch.SMS;

var sinch = new SinchClient(default, default, default,
    options =>
    {
        options.UseServicePlanIdWithSms(
            "SINCH_SERVICE_PLAN_ID",
            "SINCH_SMS_API_TOKEN",
            SmsServicePlanIdRegion.Us);
    });
```

> **Note:** if you use both the SMS and the [Conversation API](#conversation-api) from the same client, set `sms_region` and `conversation_region` to the same region. Mismatched regions cause delivery failures.

#### Sinch Events

The SMS API delivers asynchronous Sinch Events to an Event Destination, whose URL is set per batch with the `callback_url` parameter on the send, update and replace operations. The SDK provides typed models for deserializing inbound callbacks, such as `IncomingTextSms` and `RecipientDeliveryReport`:

```csharp
using Sinch.SMS.Hooks;

// In an ASP.NET controller, model binding deserializes the payload automatically:
[HttpPost]
public async Task HandleInbound([FromBody] IncomingTextSms incomingSms)
{
    // handle inbound SMS event
}
```

Signature authentication for SMS events must be enabled for your account by your account manager. Until it is activated, signature headers will not be present. See the [SMS events documentation](https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/#tag/Webhooks/section/Callbacks).

You can find a complete example in [examples/WebApi/Controllers/InboundSmsController.cs](examples/WebApi/Controllers/InboundSmsController.cs).

### Voice API

The Voice API uses application credentials. Set `application_key` and `application_secret`, both available on the [Apps dashboard](https://dashboard.sinch.com/voice/apps); `voice_region` is optional and defaults to a global region:

```csharp
using Sinch;
using Sinch.Voice;

var voiceClient = sinch.Voice(
    "SINCH_APPLICATION_KEY",
    "SINCH_APPLICATION_SECRET",
    VoiceRegion.Global);
```

#### Sinch Events

The Voice API delivers synchronous Sinch Events to the Event Destination URL configured for your app. Requests are signed with your application credentials, so validation requires the HTTP verb and URI of the controller handling the request, in addition to the `headers` and raw `body`:

```csharp
using Sinch.Voice.Hooks;

bool validAuth = voiceClient.ValidateAuthenticationHeader(
    HttpMethod.Post,
    "/webhooks/voice",
    Request.Headers.ToDictionary(h => h.Key, h => h.Value.AsEnumerable()),
    rawBody);

IVoiceEvent voiceEvent = voiceClient.ParseEvent(rawBody);
```

Some events (for example an incoming call) expect a SVAML response: build it from the business layer and return it from your controller.

You can find a complete example in [examples/WebApi/Controllers/HandleIncomingIceEventController.cs](examples/WebApi/Controllers/HandleIncomingIceEventController.cs).

### Verification API

The Verification API uses application credentials. Set `application_key` and `application_secret`, both available on the [Apps dashboard](https://dashboard.sinch.com/verification/apps):

```csharp
using Sinch;

var verificationClient = sinch.Verification(
    "SINCH_APPLICATION_KEY",
    "SINCH_APPLICATION_SECRET");
```

#### Sinch Events

The Verification API delivers synchronous Sinch Events to the Event Destination URL configured for your app. Requests are signed with your application credentials, so validation requires the HTTP verb and URI of the controller handling the request, in addition to the `headers` and raw `body`:

```csharp
using System.Text.Json;
using Sinch.Verification.Hooks;

bool validAuth = verificationClient.ValidateAuthenticationHeader(
    HttpMethod.Post,
    "/webhooks/verification",
    Request.Headers.ToDictionary(h => h.Key, h => h.Value.AsEnumerable()),
    rawBody);

var verificationEvent = JsonSerializer.Deserialize<VerificationRequestEvent>(rawBody);
```

Some events expect a response: build it from the business layer and return it to Sinch.

### Numbers API

The Numbers API needs no extra parameters, use the [common client](#client-initialization) shown above.

#### Sinch Events

The Numbers API delivers asynchronous Sinch Events to the Event Destination you configure through `sinch.Numbers.Callbacks`. `ValidateAuthenticationHeader` confirms a request comes from Sinch; `headers` and `body` are the incoming request's headers and raw body:

```csharp
using System.Text.Json;
using Sinch.Numbers.Hooks;

bool validAuth = sinch.Numbers.ValidateAuthenticationHeader(sinchEventsSecret, rawBody, Request.Headers);

var numbersEvent = JsonSerializer.Deserialize<Event>(rawBody);
```

`sinchEventsSecret` is the `HmacSecret` value configured on the Event Destination. Deserializing the payload works without validating the request, but then its origin can't be verified, so validating is recommended in production.

### Fax API

The Fax API needs no extra parameters beyond the [common client](#client-initialization) shown above. Optionally set `fax_region` in `SinchOptions` to select the regional endpoint.

#### Sinch Events

The Fax API delivers asynchronous Sinch Events to the incoming webhook URL you configure per service in the Fax dashboard. The SDK provides typed event models such as `IncomingFaxEvent` and `CompletedFaxEvent` for deserializing the payload:

```csharp
using Sinch.Fax.Hooks;

[HttpPost]
public IActionResult HandleFaxEvent([FromBody] IFaxEvent faxEvent)
{
    // handle fax event
}
```

No request signature validation is implemented for the Fax API. You can find a complete example in [examples/WebApi/Controllers/HandleFaxEventController.cs](examples/WebApi/Controllers/HandleFaxEventController.cs).

### Your first request

Once your client is configured, you can send your first message. The example below uses the Conversation API to send a simple text message over SMS. Replace `CONVERSATION_APP_ID` with your app ID, `SINCH_VIRTUAL_PHONE_NUMBER` with your Sinch number and `RECIPIENT_PHONE_NUMBER` with the recipient's phone number:

```csharp
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Send;

var response = await sinch.Conversation.Messages.Send(new SendMessageRequest
{
    AppId = "CONVERSATION_APP_ID",
    Recipient = new Identified
    {
        IdentifiedBy = new IdentifiedBy
        {
            ChannelIdentities = new List<ChannelIdentity>
            {
                new()
                {
                    Channel = ConversationChannel.Sms,
                    Identity = "RECIPIENT_PHONE_NUMBER"
                }
            }
        }
    },
    Message = new AppMessage(new TextMessage("Hello from the Sinch .NET SDK!")),
    ChannelProperties = new Dictionary<string, string>
    {
        ["SMS_SENDER"] = "SINCH_VIRTUAL_PHONE_NUMBER"
    }
});
```

## Logging

The SDK uses [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging). Provide an `ILoggerFactory` through `SinchOptions` when initializing the client:

```csharp
using Sinch;

var sinch = new SinchClient(
    "SINCH_PROJECT_ID",
    "SINCH_KEY_ID",
    "SINCH_KEY_SECRET",
    options =>
    {
        options.LoggerFactory = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        });
    });
```

If no logger factory is provided, the SDK does not emit log output.

## Handling exceptions

For an unsuccessful API call, `SinchApiException` will be thrown. It exposes the HTTP status code, a status string, a detailed message, and any additional error details:

```csharp
using Sinch;
using Sinch.SMS.Batches.Send;

try
{
    var batch = await sinch.Sms.Batches.Send(new SendTextBatchRequest
    {
        Body = "Hello, World!",
        To = new List<string> { "+123456789" }
    });
}
catch (SinchApiException e)
{
    logger.LogError("API exception. Status: {status}. Detailed message: {message}", e.Status, e.DetailedMessage);
}
```

Authentication failures throw `SinchAuthException` from the `Sinch.Auth` namespace.

## Custom HTTP client implementation

By default, the SDK creates and manages its own `HttpClient`. To provide your own instance (for example, to configure a proxy or reuse a shared client), pass it through `SinchOptions`:

```csharp
using Sinch;

var sinch = new SinchClient(
    "SINCH_PROJECT_ID",
    "SINCH_KEY_ID",
    "SINCH_KEY_SECRET",
    options =>
    {
        options.HttpClient = new HttpClient();
    });
```

For additional configuration options such as API URL overrides, see [`SinchOptions`](src/Sinch/SinchOptions.cs).

## Third-party dependencies

The SDK relies on the following third-party dependencies:

- [Microsoft.Extensions.Http](https://www.nuget.org/packages/Microsoft.Extensions.Http): HTTP client factory integration.
- [Microsoft.Extensions.Primitives](https://www.nuget.org/packages/Microsoft.Extensions.Primitives): Primitive types used across the SDK.
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json): JSON serialization and deserialization.

## Examples

You can find:

- a C# example of selected API operations in the [snippets](snippets) folder.
- console application examples in the [examples/Console](examples/Console) folder.
- an ASP.NET web application for handling Sinch Events in the [examples/WebApi](examples/WebApi) folder.

## Changelog & Migration

For information about the latest changes in the SDK, please refer to the [GitHub releases](https://github.com/sinch/sinch-sdk-dotnet/releases) page.

## License

This project is licensed under the Apache License. See the [LICENSE](LICENSE) file for the license text.

## Contact

Developer Experience engineering team: [team-developer-experience@sinch.com](mailto:team-developer-experience@sinch.com)
