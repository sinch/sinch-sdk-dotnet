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
- [FaxRegion moved from SinchOptions to SinchFaxConfiguration](#faxregion-moved-from-sinchoptions-to-sinchfaxconfiguration)
- [Numbers API: Callbacks renamed to CallbackConfiguration](#callbacks-renamed-to-callbackconfiguration)
- [Numbers API: ScheduledProvisioning.ErrorCodes type changed to IList\<FailureCode\>](#scheduledprovisioningerrorcodes-type-changed-to-ilistfailurecode)
- [SMS Webhooks: renamed and removed types](#sms-webhooks-renamed-and-removed-types)

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

## FaxRegion moved from SinchOptions to SinchFaxConfiguration

The `FaxRegion` property has been removed from `SinchOptions`. Use the `Region` property on `SinchFaxConfiguration` instead.

Version 1.*:
```csharp
var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET", options =>
{
    options.FaxRegion = FaxRegion.Europe;
});
```

Version 2.*:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    FaxConfiguration = new SinchFaxConfiguration()
    {
        Region = FaxRegion.Europe
    }
});
```

## Numbers API

The `Callbacks` property on `ISinchNumbers` has been renamed to `CallbackConfiguration`.

Version 1.*:
```csharp
var callbacks = sinchClient.Numbers.Callbacks;
```

Version 2.*:
```csharp
var callbackConfiguration = sinchClient.Numbers.CallbackConfiguration;
```

## ScheduledProvisioning.ErrorCodes type changed to IList\<FailureCode\>

The `ErrorCodes` property on `ScheduledProvisioning` has been changed from `List<string>?` to `IList<FailureCode>?`.

Version 1.*:
```csharp
using Sinch.Numbers;

var scheduledProvisioning = activeNumber.SmsConfiguration?.ScheduledProvisioning;
if (scheduledProvisioning?.ErrorCodes?.Contains("CAMPAIGN_NOT_AVAILABLE"))
{
    // handle error
}
```

Version 2.*:
```csharp
using Sinch.Numbers;
using Sinch.Numbers.Hooks;

var scheduledProvisioning = activeNumber.SmsConfiguration?.ScheduledProvisioning;
if (scheduledProvisioning?.ErrorCodes?.Contains(FailureCode.CampaignNotAvailable))
{
    // handle error
}
```

## SMS Webhooks: renamed and removed types

Several types used for SMS webhook payloads have been renamed to make their intent more explicit and to avoid collisions with other APIs. A couple of now-redundant inbound types were removed.

Renamed types

- `DeliveryReport` -> `BatchDeliveryReportSms`
- `RecipientDeliveryReport` -> `RecipientDeliveryReportSms`
- `IncomingTextSms` -> `TextMessage`

Example — batch delivery report

Version 1.*:
```csharp
// Deserialize directly to the old type
var report = JsonSerializer.Deserialize<DeliveryReport>(json);
```

Version 2.*:
```csharp
// Deserialize to the new explicit type
var report = JsonSerializer.Deserialize<BatchDeliveryReportSms>(json);

// or use the SMS webhooks helper which dispatches to the correct type based on the payload
var report = sinchClient.Sms.Webhooks.ParseEvent(json).As<BatchDeliveryReportSms>();
```

Example — recipient delivery report

Version 1.*:
```csharp
var recipientReport = JsonSerializer.Deserialize<RecipientDeliveryReport>(json);
```

Version 2.*:
```csharp
var recipientReport = JsonSerializer.Deserialize<RecipientDeliveryReportSms>(json);
// or via the webhooks parser:
var recipientReport = sinchClient.Sms.Webhooks.ParseEvent(json).As<RecipientDeliveryReportSms>();
```

Example — incoming text message

Version 1.*:
```csharp
var incoming = JsonSerializer.Deserialize<IncomingTextSms>(json);
```

Version 2.*:
```csharp
var incoming = JsonSerializer.Deserialize<TextMessage>(json);
// or via the webhooks parser:
var incoming = sinchClient.Sms.Webhooks.ParseEvent(json).As<TextMessage>();
```

Deleted files / types

The following source files were removed as part of the refactor and should be deleted from any code references you may have:

- `src/Sinch/SMS/Hooks/IIncomingSms.cs`
- `src/Sinch/SMS/Hooks/IncomingBinarySms.cs`

Replacement guidance

- If you previously relied on `IIncomingSms` or `IncomingBinarySms`, switch to the new strongly-typed webhook models (`TextMessage`, `BinaryMessage`, etc.) and prefer the `ISmsWebhooks.ParseEvent(string json)` helper which returns an `ISmsEvent` that you can pattern-match or cast as shown above.

- Example migrating code that previously deserialized the old incoming binary type:

Version 1.*:
```csharp
var bin = JsonSerializer.Deserialize<IncomingBinarySms>(json);
```

Version 2.*:
```csharp
var bin = sinchClient.Sms.Webhooks.ParseEvent(json).As<BinaryMessage>();
// or when using plain deserialization:
var bin = JsonSerializer.Deserialize<BinaryMessage>(json);
```
