# Migration to 2.0

## Content list:
- [Initialize SinchClient with unified credentials](#initialize-sinchclient-with-unified-credentials)
- [Initialize Voice and Verification clients](#initialize-voice-and-verification-clients)
- [Provide Logger, and your own HttpClient](#provide-logger-and-your-own-httpclient)
- [Set API Regions (where applicable)](#set-api-regionswhere-applicable)
- [Override API urls](#override-api-urls)
- [Use SMS API with ServicePlanId](#use-sms-api-with-serviceplanid)

## Initialize `SinchClient` with unified credentials:

Console application:

Version 1.*:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET");
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
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

This automatically integrates with `IHttpClientFactory` and `ILoggerFactory` from the DI container.

## Initialize `Voice` and `Verification` clients:

Version 1.*:
```csharp
var sinchClient = new SinchClient(null, null, null);
var sinchVoiceClient = sinchClient.Voice("APP_KEY", "APP_SECRET");
var sinchVerificationClient = sinchClient.Verification("APP_KEY", "APP_SECRET");
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    VoiceConfiguration = new SinchVoiceConfiguration()
    {
        AppKey = "APP_KEY",
        AppSecret = "APP_SECRET",
    },
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = "APP_KEY",
        AppSecret = "APP_SECRET",
    }
});
var sinchVoiceClient = sinch.Voice;
var sinchVerificationClient = sinch.Verification;
```

## Provide `Logger`, and your own `HttpClient`:

Version 1.*:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.HttpClient = new HttpClient();
    options.LoggerFactory =  Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance;
});
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    // ... set credentials
    SinchOptions = new SinchOptions()
    {
        LoggerFactory = Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance
    }
});
```

**Note:** In version 2.*, you no longer need to provide your own `HttpClient`. The SDK manages HTTP client lifecycle internally with proper connection pooling and DNS refresh. Only provide a custom `IHttpClientFactory` if you have specific requirements.

## Set API Regions(where applicable):

Version 1.*, with `SinchOptions`:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.SmsRegion = SmsRegion.Eu;
    options.ConversationRegion = ConversationRegion.Us;
});
```

Version 2.*, each `Region` is set in dedicated API config:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    // ... set unified credentials
    SmsConfiguration = new SinchSmsConfiguration()
    {
        Region = SmsRegion.Eu,
    },
    ConversationConfiguration = new SinchConversationConfiguration()
    {
       ConversationRegion = ConversationRegion.Us
    },
});
```

## Override API urls:

Version 1.*, with `ApiUrlOverrides` class:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.ApiUrlOverrides = new ApiUrlOverrides()
    {
        SmsUrl = "https://my-sms-proxy.io",
        ConversationUrl = "https://my-conversation-proxy.io",
        // ... and so on.
    };
});
```

Version 2.*, each URL is overriden in dedicated API config:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    SmsConfiguration = new SinchSmsConfiguration()
    {
        UrlOverride = "https://my-sms-proxy.io",
    },
    ConversationConfiguration = new SinchConversationConfiguration()
    {
        ConversationUrlOverride = "https://my-conversation-proxy.io",
    },
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        UrlOverride = "https://my-verification-proxy.io",
        AppKey = "APP_KEY",
        AppSecret = "APP_SECRET",
    }
});
```

## Use SMS API with `ServicePlanId`:

Version 1.*:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.UseServicePlanIdWithSms("SERVICE_PLAN_ID", "API_TOKEN", SmsServicePlanIdRegion.Eu);
});
```

Version 2.*:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("SERVICE_PLAN_ID",
        "API_TOKEN", SmsServicePlanIdRegion.Eu)
});
```

