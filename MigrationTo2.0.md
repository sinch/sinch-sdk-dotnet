# Code samples for migration to version 2.0

# ADD CONTENT LIST
## Initialize `SinchClient` with unified credentials:

Version 1.*:
```csharp
var sinchClient = new SinchClient("YOUR_project_id", "YOUR_key", "YOUR_secret");
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = "YOUR_project_id",
        KeyId = "YOUR_key_id",
        KeySecret = "YOUR_key_secret"
    }
});
```

## Initialize `Voice` and `Verification` clients:

Version 1.*:
```csharp
var sinchClient = new SinchClient(null, null, null);
var sinchVoiceClient = sinchClient.Voice("YOUR_app_key", "YOUR_app_secret");
var sinchVerificationClient = sinchClient.Verification("YOUR_app_key", "YOUR_app_secret");
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    VoiceConfiguration = new SinchVoiceConfiguration()
    {
        AppKey = "YOUR_app_key",
        AppSecret = "YOUR_app_secret",
    },
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = "YOUR_app_key",
        AppSecret = "YOUR_app_secret",
    }
});
var sinchVoiceClient = sinch.Voice;
var sinchVerificationClient = sinch.Verification;
```

## Provide `Logger`, and your own `HttpClient`:

Version 1.*:
```csharp
var sinchClient = new SinchClient("YOUR_project_id", "YOUR_key", "YOUR_secret", options =>
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
        HttpClient = new HttpClient(),
        LoggerFactory = Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance
    }
});
```

## Set API Regions(where applicable):

Version 1.*, with `SinchOptions`:
```csharp
var sinchClient = new SinchClient("YOUR_project_id", "YOUR_key", "YOUR_secret", options =>
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
var sinchClient = new SinchClient("YOUR_project_id", "YOUR_key", "YOUR_secret", options =>
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
        AppKey = "YOUR_app_key",
        AppSecret = "YOUR_app_secret",
    }
});
```

## Use SMS API with `ServicePlanId`:

Version 1.*:
```csharp
var sinchClient = new SinchClient("YOUR_project_id", "YOUR_key", "YOUR_secret", options =>
{
    options.UseServicePlanIdWithSms("YOUR_service_plan_id", "YOUR_api_token", SmsServicePlanIdRegion.Eu);
});
```

Version 2.*:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("YOUR_service_plan_id",
        "YOUR_api_token", SmsServicePlanIdRegion.Eu)
});
```

