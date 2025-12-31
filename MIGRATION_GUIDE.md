# Migration to 2.0

## Content list:
- [Initialize SinchClient with unified credentials](#initialize-sinchclient-with-unified-credentials)
- [Initialize Voice and Verification clients](#initialize-voice-and-verification-clients)
- [Provide Logger, and your own HttpClient](#provide-logger-and-your-own-httpclient)
- [Set API Regions (where applicable)](#set-api-regionswhere-applicable)
- [Override API urls](#override-api-urls)
- [Use SMS API with ServicePlanId](#use-sms-api-with-serviceplanid)
- [VoiceConfiguration is now abstract](#voiceconfiguration-is-now-abstract)
- [ScheduledVoiceProvisioning is now abstract](#scheduledvoiceprovisioning-is-now-abstract)
- [VoiceConfiguration and ScheduledVoiceProvisioning classes moved to new namespace](#voiceconfiguration-and-scheduledvoiceprovisioning-classes-moved-to-new-namespace)
- [VoiceConfiguration Type property is now internal](#voiceconfiguration-type-property-is-now-internal)
- [Removed obsolete UrlMessage and CallMessage constructors](#removed-obsolete-urlmessage-and-callmessage-constructors)

## Initialize `SinchClient` with unified credentials:

Console application:

Version 1:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET");
```

Version 2:
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
var builder = WebApplication.CreateBuilder();
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

Version 1:
```csharp
var sinchClient = new SinchClient(null, null, null);
var sinchVoiceClient = sinchClient.Voice("APPLICATION_KEY", "APPLICATION_SECRET");
var sinchVerificationClient = sinchClient.Verification("APPLICATION_KEY", "APPLICATION_SECRET");
```

Version 2:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration()
{
    VoiceConfiguration = new SinchVoiceConfiguration()
    {
        AppKey = "APPLICATION_KEY",
        AppSecret = "APPLICATION_SECRET",
    },
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = "APPLICATION_KEY",
        AppSecret = "APPLICATION_SECRET",
    }
});
var sinchVoiceClient = sinch.Voice;
var sinchVerificationClient = sinch.Verification;
```

## Provide `Logger`, and your own `HttpClient`:

Version 1:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.HttpClient = new HttpClient();
    options.LoggerFactory =  Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance;
});
```

Version 2:
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

**Note:** In version 2, you no longer need to provide your own `HttpClient`. The SDK manages HTTP client lifecycle internally with proper connection pooling and DNS refresh. Only provide a custom `IHttpClientFactory` if you have specific requirements.

## Set API Regions(where applicable):

Version 1, with `SinchOptions`:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.SmsRegion = SmsRegion.Eu;
    options.ConversationRegion = ConversationRegion.Us;
});
```

Version 2, each `Region` is set in dedicated API config:
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

Version 1, with `ApiUrlOverrides` class:
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

Version 2, each URL is overriden in dedicated API config:
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

Version 1:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.UseServicePlanIdWithSms("SERVICE_PLAN_ID", "API_TOKEN", SmsServicePlanIdRegion.Eu);
});
```

Version 2:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("SERVICE_PLAN_ID",
        "API_TOKEN", SmsServicePlanIdRegion.Eu)
});
```

## VoiceConfiguration is now abstract

The `VoiceConfiguration` class in the Numbers API is now abstract. You must use one of the concrete implementations based on your voice application type:

Version 1.*:
```csharp
var voiceConfig = new VoiceConfiguration()
{
    Type = VoiceApplicationType.Rtc
};
```

Version 2.*:
```csharp
// For RTC (Voice) applications
var voiceConfig = new VoiceRtcConfiguration();

// For EST (Elastic SIP Trunking)
var voiceConfig = new VoiceEstConfiguration();

// For FAX services
var voiceConfig = new VoiceFaxConfiguration();
```

## ScheduledVoiceProvisioning is now abstract

The `ScheduledVoiceProvisioning` class is now abstract. You must use one of the concrete implementations based on your voice application type:

Version 1.*:
```csharp
var scheduledProvisioning = new ScheduledVoiceProvisioning();
```

Version 2.*:
```csharp
// For RTC (Voice) applications
var scheduledProvisioning = new ScheduledVoiceRtcProvisioning();

// For EST (Elastic SIP Trunking)
var scheduledProvisioning = new ScheduledVoiceEstProvisioning();

// For FAX services
var scheduledProvisioning = new ScheduledVoiceFaxProvisioning();
```

## VoiceConfiguration and ScheduledVoiceProvisioning classes moved to new namespace

The following classes have been moved from the `Sinch.Numbers` namespace to `Sinch.Numbers.VoiceConfigurations`:

- `VoiceConfiguration`
- `ScheduledVoiceProvisioning`
- `VoiceConfigurationConverter`
- `ScheduledVoiceProvisioningConverter`

Version 1.*:
```csharp
using Sinch.Numbers;
```

Version 2.*:
```csharp
using Sinch.Numbers.VoiceConfigurations;
```

## VoiceConfiguration Type property is now internal

The `Type` property on `VoiceConfiguration` and its derived classes (`VoiceRtcConfiguration`, `VoiceEstConfiguration`, `VoiceFaxConfiguration`) is now `internal`. The same applies to `ScheduledVoiceProvisioning` derived classes.

**Impact:**
- Code that accessed the `Type` property directly will no longer compile
- The SDK automatically handles the `type` field during serialization/deserialization
- Use pattern matching or type checking to determine the concrete type

Version 1.*:
```csharp
var voiceConfig = activeNumber.VoiceConfiguration;
if (voiceConfig.Type == VoiceApplicationType.Rtc)
{
    // handle RTC configuration
}
```

Version 2.*:
```csharp
var voiceConfig = activeNumber.VoiceConfiguration;
if (voiceConfig is VoiceRtcConfiguration rtcConfig)
{
    // handle RTC configuration
    var appId = rtcConfig.AppId;
}
```

**Serialization behavior:** When serializing `VoiceConfiguration` objects, the SDK automatically includes the correct `type` field in the JSON output based on the concrete type. You don't need to set the type manually.

## Removed obsolete UrlMessage and CallMessage constructors

The obsolete constructors for `UrlMessage` and `CallMessage` (used in Choice messages in the Conversation API) have been removed. Use object initializer syntax instead:

Version 1.*:
```csharp
var urlMessage = new UrlMessage("Click here", new Uri("https://example.com"));
var callMessage = new CallMessage("+1234567890", "Call us");
```

Version 2.*:
```csharp
var urlMessage = new UrlMessage
{
    Title = "Click here",
    Url = "https://example.com"
};

var callMessage = new CallMessage
{
    PhoneNumber = "+1234567890",
    Title = "Call us"
};
```
