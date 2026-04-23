# Migration to 2.0

## Content list:
- [.NET Framework Support](#net-framework-support)
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
- [Fax API: Region is no longer supported](#fax-api-region-is-no-longer-supported)
- [Numbers API: Callbacks renamed to CallbackConfiguration](#callbacks-renamed-to-callbackconfiguration)
- [Numbers API: ScheduledProvisioning.ErrorCodes type changed to IList\<FailureCode\>](#scheduledprovisioningerrorcodes-type-changed-to-ilistfailurecode)
- [Removed obsolete MessageSource property from ListMessagesRequest](#removed-obsolete-messagesource-property-from-listmessagesrequest)
- [Removed TemplatesV1 from Conversation API](#removed-templatesv1-from-conversation-api)
- [SMS Webhooks: renamed and removed types](#sms-webhooks-renamed-and-removed-types)
- [SMS Webhooks: property rename for per-recipient delivery reports](#sms-webhooks-property-rename-for-per-recipient-delivery-reports)
- [ConversationChannelCredentials, InstagramCredentials and LineEnterpriseCredentials moved to new namespace](#conversationchanelcredentials-instagramcredentials-and-lineenterprisecredentials-moved-to-new-namespace)
- [Verification API: Callout renamed to PhoneCall and Seamless renamed to Data](#verification-api-callout-renamed-to-phonecall-and-seamless-renamed-to-data)
- [Fax API: ListEmailsResponse replaced with concrete response types](#fax-api-listemailsresponse-replaced-with-concrete-response-types)
- [Conversation API: Coordinates properties changed from float to double](#conversation-api-coordinates-properties-changed-from-float-to-double)
- [Fax API: SendFaxRequest constructors replaced with factory methods](#fax-api-sendfaxrequest-constructors-replaced-with-factory-methods)
- [Conversation API: InjectEventRequest now supports only AppEvent](#conversation-api-injecteventrequest-now-supports-only-appevent)
- [Region configuration is now required for SMS and Conversation](#region-configuration-is-now-required-for-sms-and-conversation)
- [Conversation API: WhatsApp payment_settings replaced by payment_buttons](#conversation-api-whatsapp-payment_settings-replaced-by-payment_buttons)
- [Conversation API: ConversationDirection.UndefinedDirection removed](#conversation-api-conversationdirectionundefineddirection-removed)
- [Conversation API: ListConversationsRequest.OnlyActive is no longer required](#conversation-api-listconversationsrequestonlyactive-is-no-longer-required)
- [Conversation API: InjectMessageRequest fields are now required](#conversation-api-injectmessagerequest-fields-are-now-required)
- [Conversation API: InjectMessageRequest requires a constructor for the message payload](#conversation-api-injectmessagerequest-requires-a-constructor-for-the-message-payload)
- [Conversation API: ConversationEvent.Event and ConversationEventEvent removed](#conversation-api-conversationeventevent-and-conversationeventevent-removed)
- [Conversation API: TemplatesV2 ChannelTemplateOverrides type changed to Dictionary with ConversationChannel keys](#conversation-api-templatesv2-channeltemplateoverrides-type-changed-to-dictionary-with-conversationchannel-keys)
- [Conversation API: TemplatesV2 ParameterMappings type changed to Dictionary](#conversation-api-templatesv2-parametermappings-type-changed-to-dictionary)
- [Conversation API: Template.Id is now nullable](#conversation-api-templateid-is-now-nullable)
- [Conversation API: CreateTemplateRequest and UpdateTemplateRequest no longer expose create\_time and update\_time](#conversation-api-createtemplaterequest-and-updatetemplaterequest-no-longer-expose-create_time-and-update_time)
- [Conversation API: Webhooks List now returns ListWebhooksResponse](#conversation-api-webhooks-list-now-returns-listwebhooksresponse)
- [Conversation API: Webhooks request fields are now optional per OAS spec](#conversation-api-webhooks-request-fields-are-now-optional-per-oas-spec)
- [Conversation API: Webhooks ValidateAuthenticationHeader no longer accepts JsonNode](#conversation-api-webhooks-validateauthenticationheader-no-longer-accepts-jsonnode)
- [Conversation API: Webhooks ValidateAuthenticationHeader no longer accepts StringValues headers](#conversation-api-webhooks-validateauthenticationheader-no-longer-accepts-stringvalues-headers)
- [Conversation API: Webhooks ParseEvent no longer accepts JsonNode](#conversation-api-webhooks-parseevent-no-longer-accepts-jsonnode)
- [Conversation API: Webhooks renamed to EventDestinations](#conversation-api-webhooks-renamed-to-eventdestinations)
- [Conversation API: Sinch.Conversation.Hooks namespace renamed to Sinch.Conversation.SinchEvents](#conversation-api-sinchconversationhooks-namespace-renamed-to-sinchconversationsinchevents)
- [Conversation API: CallbackSettings renamed to EventDestinationSettings](#conversation-api-callbacksettings-renamed-to-eventdestinationsettings)
- [Conversation API: SendEventRequest and SendMessageRequest CallbackUrl renamed to EventDestinationTarget](#conversation-api-sendeventrequest-and-sendmessagerequest-callbackurl-renamed-to-eventdestinationtarget)
- [Numbers API: CallbackConfiguration renamed to EventDestination](#numbers-api-callbackconfiguration-renamed-to-eventdestination)
- [Numbers API: Sinch.Numbers.Hooks namespace renamed to Sinch.Numbers.SinchEvents; Event renamed to NumberSinchEvent](#numbers-api-sinchnumbershooks-namespace-renamed-to-sinchnumberssinchevents-event-renamed-to-numbersinchemvent)
- [Numbers API: CallbackUrl renamed to EventDestinationTarget](#numbers-api-callbackurl-renamed-to-eventdestinationtarget)
- [Conversation API: ICallbackEvent and related types renamed](#conversation-api-icallbackevent-and-related-types-renamed)
- [SMS API: Sinch.SMS.Hooks namespace renamed to Sinch.SMS.SinchEvents](#sms-api-sinchsmshooks-namespace-renamed-to-sinchsmssinchevents)
- [SMS API: BatchBase and UpdateBatchBaseRequest CallbackUrl renamed to EventDestinationTarget](#sms-api-batchbase-and-updatebatchbaserequest-callbackurl-renamed-to-eventdestinationtarget)

## .NET Framework Support

Version 2.0 of the Sinch .NET SDK drops support for .NET 6 and .NET 7. The SDK now requires **.NET 8.0**, **.NET 9.0**, or **.NET 10.0**.

If you are currently targeting .NET 6 or 7, you must upgrade your project's target framework before using version 2.0 of the SDK. You can do this by updating the `<TargetFramework>` element in your `.csproj` file:

```xml
<!-- Update from .NET 6 or 7 -->
<TargetFramework>net8.0</TargetFramework>
<!-- or -->
<TargetFramework>net9.0</TargetFramework>
<!-- or -->
<TargetFramework>net10.0</TargetFramework>
```

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
       Region = ConversationRegion.Us
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

## Fax API: Region is no longer supported

The `Region` property on `SinchFaxConfiguration` has been removed. The Fax API now uses a single global endpoint regardless of region.

Version 1.*:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    FaxConfiguration = new SinchFaxConfiguration()
    {
        Region = FaxRegion.Europe // remove this
    }
});
```

Version 2.*:
```csharp
var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    FaxConfiguration = new SinchFaxConfiguration()
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

## Removed obsolete MessageSource property from ListMessagesRequest

The deprecated `MessageSource` property has been removed from `ListMessagesRequest`. Use `MessagesSource` instead.

Version 1.*:
```csharp
var request = new ListMessagesRequest
{
    MessageSource = MessageSource.DispatchSource
};
```

Version 2.*:
```csharp
var request = new ListMessagesRequest
{
    MessagesSource = MessageSource.DispatchSource
};
```

## Removed TemplatesV1 from Conversation API

The `TemplatesV1` property has been removed from the Conversation API client. Use `Templates` instead.
The templates client interface has also been renamed from `ISinchConversationTemplatesV2` to `ISinchConversationTemplates`.

Version 1.*:
```csharp
var templates = await sinchClient.Conversation.TemplatesV1.List();
```

Version 2.*:
```csharp
var templates = await sinchClient.Conversation.Templates.List();

ISinchConversationTemplates templatesClient = sinchClient.Conversation.Templates;
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

// or use the SMS sinch event helper which dispatches to the correct type based on the payload
var report = sinchClient.Sms.SinchEvents.ParseEvent(json).As<BatchDeliveryReportSms>();
```

Example — recipient delivery report

Version 1.*:
```csharp
var recipientReport = JsonSerializer.Deserialize<RecipientDeliveryReport>(json);
```

Version 2.*:
```csharp
var recipientReport = JsonSerializer.Deserialize<RecipientDeliveryReportSms>(json);
// or via the sinch event parser:
var recipientReport = sinchClient.Sms.SinchEvents.ParseEvent(json).As<RecipientDeliveryReportSms>();
```

Example — incoming text message

Version 1.*:
```csharp
var incoming = JsonSerializer.Deserialize<IncomingTextSms>(json);
```

Version 2.*:
```csharp
var incoming = JsonSerializer.Deserialize<TextMessage>(json);
// or via the sinch event parser:
var incoming = sinchClient.Sms.SinchEvents.ParseEvent(json).As<TextMessage>();
```

Deleted files / types

The following source files were removed as part of the refactor and should be deleted from any code references you may have:

- `src/Sinch/SMS/Hooks/IIncomingSms.cs`
- `src/Sinch/SMS/Hooks/IncomingBinarySms.cs`

Replacement guidance

- If you previously relied on `IIncomingSms` or `IncomingBinarySms`, switch to the new strongly-typed sinch event models (`TextMessage`, `BinaryMessage`, etc.) and prefer the `ISinchSmsSinchEvents.ParseEvent(string json)` helper which returns an `ISmsEvent` that you can pattern-match or cast as shown above.

- Example migrating code that previously deserialized the old incoming binary type:

Version 1.*:
```csharp
var bin = JsonSerializer.Deserialize<IncomingBinarySms>(json);
```

Version 2.*:
```csharp
var bin = sinchClient.Sms.SinchEvents.ParseEvent(json).As<BinaryMessage>();
// or when using plain deserialization:
var bin = JsonSerializer.Deserialize<BinaryMessage>(json);
```

## SMS Webhooks: property rename for recipient delivery reports

The property `OperatorStatusName` in `RecipientDeliveryReportSms` was renamed to `OperatorStatusAt`.

Version 1.*:
```csharp
public sealed class RecipientDeliveryReportSms : ISmsEvent
{
    [JsonPropertyName("operator_status_name")]
    public string? OperatorStatusName { get; set; }
}
```
Version 2.*:
```csharp
public sealed class RecipientDeliveryReportSms : ISmsEvent
{
    [JsonPropertyName("operator_status_at")]
    public DateTime OperatorStatusAt { get; set; }
}
```

## ConversationChannelCredentials, InstagramCredentials and LineEnterpriseCredentials moved to new namespace

The following classes have been moved from the `Sinch.Conversation.Apps` namespace to `Sinch.Conversation.Apps.Credentials`:

- `ConversationChannelCredentials`
- `InstagramCredentials`
- `LineEnterpriseCredentials`

Version 1.*:
```csharp
using Sinch.Conversation.Apps;
```

Version 2.*:
```csharp
using Sinch.Conversation.Apps.Credentials;
```

## Verification API: Callout renamed to PhoneCall and Seamless renamed to Data

### Renamed Classes

- `CalloutRequestEventResponse` → `PhoneCallRequestEventResponse`
- `ReportCalloutVerificationRequest` → `ReportPhoneCallVerificationRequest`
- `ReportCalloutVerificationResponse` → `ReportPhoneCallVerificationResponse`
- `StartCalloutVerificationResponse` → `StartPhoneCallVerificationResponse`
- `CalloutVerificationStatusResponse` → `PhoneCallVerificationStatusResponse`

### Renamed Methods

In `ISinchVerification` and `SinchVerification`:

- `StartCallout(...)` → `StartPhoneCall(...)`
- `StartSeamless(...)` → `StartData(...)`
- `ReportCalloutByIdentity(...)` → `ReportPhoneCallByIdentity(...)`
- `ReportCalloutById(...)` → `ReportPhoneCallById(...)`

In `ISinchVerificationStatus` and `SinchVerificationStatus`:

- `GetCalloutById(...)` → `GetPhoneCallById(...)`
- `GetCalloutByIdentity(...)` → `GetPhoneCallByIdentity(...)`
- `GetCalloutByReference(...)` → `GetPhoneCallByReference(...)`

## Fax API: ListEmailsResponse replaced with concrete response types

The generic `ListEmailsResponse<T>` class has been replaced with two concrete response types. The methods in `ISinchFaxEmails` and `ISinchFaxServices` now return explicitly-typed response objects instead of a generic type.

Version 1.*:
```csharp
// ListForNumber returns ListEmailsResponse<string>
var emailStringsResponse = await sinchClient.Fax.Emails.ListForNumber("serviceId", "+1234567890");
var emails = emailStringsResponse.Emails; // List<string>

// List returns ListEmailsResponse<EmailAddress>
var emailAddressesResponse = await sinchClient.Fax.Emails.List("serviceId");
var emailAddresses = emailAddressesResponse.Emails; // List<EmailAddress>
```

Version 2.*:
```csharp
// ListForNumber now returns ListEmailAddressesResponse
var emailAddressesResponse = await sinchClient.Fax.Emails.ListForNumber("serviceId", "+1234567890");
var emails = emailAddressesResponse.EmailAddresses; // List<string>

// List now returns ListEmailsResponse
var emailsResponse = await sinchClient.Fax.Emails.List("serviceId");
var emailObjects = emailsResponse.Emails; // List<Email>
```

## Conversation API: Coordinates properties changed from float to double

The `Latitude` and `Longitude` properties of the `Coordinates` record (used in `LocationMessage`) have been changed from `float` to `double`.

Version 1.*:
```csharp
public record Coordinates(float Latitude, float Longitude);

// Usage
new Coordinates(47.7981899f, -4.3727685f)
```

Version 2.*:
```csharp
public record Coordinates(double Latitude, double Longitude);

// Usage
new Coordinates(47.7981899, -4.3727685)
```

## Fax API: SendFaxRequest constructors replaced with factory methods

The `SendFaxRequest` class previously provided multiple constructors for different content sources. This has been refactored to use static factory methods.

### Removed Constructors

- `public SendFaxRequest(Stream fileContent, string fileName)` — use `FromStream()` instead
- `public SendFaxRequest(string filePath)` — use `FromFile()` instead
- `public SendFaxRequest(List<Base64File> base64Files)` — use `WithFiles()` instead

### Added Factory Methods

- `public static SendFaxRequest FromStream(Stream fileContent, string fileName)` — for sending fax content from a stream
- `public static SendFaxRequest FromFile(string filePath)` — for sending fax content from a file path
- `public static SendFaxRequest WithFiles(List<Base64File> base64Files)` — for sending base64-encoded file content

### Migration Examples

**Sending from a file path:**

Version 1.*:
```csharp
using var request = new SendFaxRequest("./fax.pdf")
{
    To = ["+12015555555"]
};

var response = await sinchClient.Fax.Faxes.Send(request);
```

Version 2.*:
```csharp
using var request = SendFaxRequest.FromFile("./fax.pdf");
request.To = ["+12015555555"];

var response = await sinchClient.Fax.Faxes.Send(request);
```

**Sending from a stream:**

Version 1.*:
```csharp
var fileContent = File.ReadAllBytes("./sample.txt");
await using var stream = new MemoryStream(fileContent);
using var request = new SendFaxRequest(stream, "sample.txt")
{
    To = ["+12015555555"]
};

var response = await sinchClient.Fax.Faxes.Send(request);
```

Version 2.*:
```csharp
var fileContent = File.ReadAllBytes("./sample.txt");
await using var stream = new MemoryStream(fileContent);
using var request = SendFaxRequest.FromStream(stream, "sample.txt");
request.To = ["+12015555555"];

var response = await sinchClient.Fax.Faxes.Send(request);
```

**Sending base64-encoded files:**

Version 1.*:
```csharp
var request = new SendFaxRequest(new List<Base64File>
{
    new()
    {
        File = base64FileContent,
        FileType = FileType.PDF
    }
})
{
    To = ["+12015555555"]
};

var response = await sinchClient.Fax.Faxes.Send(request);
```

Version 2.*:
```csharp
var request = SendFaxRequest.WithFiles(new List<Base64File>
{
    new()
    {
        File = base64FileContent,
        FileType = FileType.PDF
    }
});
request.To = ["+12015555555"];

var response = await sinchClient.Fax.Faxes.Send(request);
```

## Conversation API: InjectEventRequest now supports only AppEvent

The `InjectEventRequest` class now supports injecting only `AppEvent`. Support for `ContactEvent` and `ContactMessageEvent` has been removed.

Version 1.*:
```csharp
// Previously could inject ContactEvent, ContactMessageEvent, or AppEvent
var request = new InjectEventRequest
{
    Event = new ContactEvent { /* ... */ }
};
```

Version 2.*:
```csharp
// Only AppEvent is now supported
var request = new InjectEventRequest
{
    Event = new AppEvent { /* ... */ }
};
```

## Region configuration is now required for SMS and Conversation

The `Region` property in `SinchSmsConfiguration` and `ConversationRegion` in `SinchConversationConfiguration` are now **required**. Validation is performed at runtime when the Sinch client is first accessed and an `InvalidOperationException` is thrown if the region is not provided.

**SMS:**

Version 1.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ }
});
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ },
    SmsConfiguration = new SinchSmsConfiguration
    {
        Region = SmsRegion.Us
    }
});
```

**Conversation:**

Version 1.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ }
});
```

Version 2.*:
```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        Region = ConversationRegion.Us
    }
});
```

## Conversation API: WhatsApp payment_settings replaced by payment_buttons

The `PaymentSettings` property on `OrderDetailsPayment` has been removed. Use `PaymentButtons` instead, which accepts a list of 1–2 `IWhatsAppPaymentButton` items.

Version 1.*:
```csharp
var payment = new OrderDetailsPayment
{
    // ...
    PaymentSettings = new OrderDetailsPaymentSettings
    {
        DynamicPix = new OrderDetailsPaymentSettingsDynamicPix
        {
            Code = "MY_PIX_CODE",
            MerchantName = "My Store",
            Key = "MY_PIX_KEY",
            KeyType = "CPF"
        }
    }
};
```

Version 2.*:
```csharp
var payment = new OrderDetailsPayment
{
    // ...
    PaymentButtons = new List<IWhatsAppPaymentButton>
    {
        new WhatsAppPaymentSettingsButtonPix
        {
            Code = "MY_PIX_CODE",
            MerchantName = "My Store",
            Key = "MY_PIX_KEY",
            KeyType = WhatsAppPaymentSettingsButtonPix.PixKeyType.Cpf
        }
    }
};
```

Other available button types:
```csharp
new WhatsAppPaymentSettingsButtonPaymentLink { Uri = "https://pay.example.com/order123" }

new WhatsAppPaymentSettingsButtonBoleto { DigitableLine = "12345.67890 12345.678901 12345.678901 1 12340000012300" }
```

## Conversation API: ConversationDirection.UndefinedDirection removed

The `UndefinedDirection` static member has been removed from `ConversationDirection`.
The valid values are now `ToApp` and `ToContact` only.

Version 1.*:
```csharp
var direction = ConversationDirection.UndefinedDirection;
```

Version 2.*:
```csharp
// Use one of the remaining valid values:
var direction = ConversationDirection.ToApp;
var direction = ConversationDirection.ToContact;
```

## Conversation API: ConversationEvent.Event and ConversationEventEvent removed

`ConversationEvent.Event` (of type `ConversationEventEvent`) has been removed. `AppEvent`, `ContactEvent`, and `ContactMessageEvent` are now top-level properties directly on `ConversationEvent`.

Version 1.*:
```csharp
var appEvent = conversationEvent.Event?.AppEvent;
var contactEvent = conversationEvent.Event?.ContactEvent;
var contactMessageEvent = conversationEvent.Event?.ContactMessageEvent;
```

Version 2.*:
```csharp
var appEvent = conversationEvent.AppEvent;
var contactEvent = conversationEvent.ContactEvent;
var contactMessageEvent = conversationEvent.ContactMessageEvent;
```

## Conversation API: ListConversationsRequest.OnlyActive is no longer required

`OnlyActive` has been changed from `required bool` to `bool?`.

Version 1.*:
```csharp
var request = new ListConversationsRequest { OnlyActive = false, AppId = "app-id" };
```

Version 2.*:
```csharp
var request = new ListConversationsRequest { AppId = "app-id" };
```

## Conversation API: InjectMessageRequest fields are now required

`Direction`, `ChannelIdentity`, and `ContactId` are now `required` on `InjectMessageRequest`.

Version 1.*:
```csharp
var request = new InjectMessageRequest
{
    AcceptTime = DateTime.UtcNow,
    AppMessage = appMessage
    // Direction, ChannelIdentity, ContactId were optional
};
```

Version 2.*:
```csharp
var request = new InjectMessageRequest(appMessage)
{
    AcceptTime = DateTime.UtcNow,
    Direction = ConversationDirection.ToContact,
    ChannelIdentity = new ChannelIdentity { Channel = ConversationChannel.Sms, Identity = "+1234567890" },
    ContactId = "contact-id"
};
```

## Conversation API: InjectMessageRequest requires a constructor for the message payload

`AppMessage` and `ContactMessage` are no longer settable via object initializer.

Version 1.*:
```csharp
var request = new InjectMessageRequest
{
    AppMessage = new AppMessage(new TextMessage("Hello")),
    // ...other properties
};
```

Version 2.*:
```csharp
// App message (TO_CONTACT)
var request = new InjectMessageRequest(new AppMessage(new TextMessage("Hello")))
{
    // ...other properties
};

// Contact message (TO_APP)
var request = new InjectMessageRequest(new ContactMessage(new TextMessage("Hello")))
{
    // ...other properties
};
```

## Conversation API: TemplatesV2 ChannelTemplateOverrides type changed to Dictionary with ConversationChannel keys

`TemplateTranslation.ChannelTemplateOverrides` type changed from `ChannelTemplateOverride` to `Dictionary<ConversationChannel, ChannelTemplateOverride>`. The old `ChannelTemplateOverride` class (with fixed `WhatsApp`/`KakaoTalk` properties) has been replaced by a new `ChannelTemplateOverride` class that represents a single channel entry (previously named `OverrideTemplateReference`).

Version 1.*:
```csharp
var translation = new TemplateTranslation(new TextMessage("Hi"))
{
    LanguageCode = "en-US",
    ChannelTemplateOverrides = new ChannelTemplateOverride
    {
        WhatsApp = new OverrideTemplateReference { TemplateReference = new TemplateReference { TemplateId = "my-template" } },
        KakaoTalk = new OverrideTemplateReference { TemplateReference = new TemplateReference { TemplateId = "my-template" } }
    }
};
```

Version 2.*:
```csharp
var translation = new TemplateTranslation(new TextMessage("Hi"))
{
    LanguageCode = "en-US",
    ChannelTemplateOverrides = new Dictionary<ConversationChannel, ChannelTemplateOverride>
    {
        [ConversationChannel.WhatsApp] = new ChannelTemplateOverride { TemplateReference = new TemplateReference { TemplateId = "my-whatsapp-template" } },
        [ConversationChannel.KakaoTalk] = new ChannelTemplateOverride { TemplateReference = new TemplateReference { TemplateId = "my-kakaotalk-template" } }
    }
};
```

## Conversation API: TemplatesV2 ParameterMappings type changed to Dictionary

`ChannelTemplateOverride.ParameterMappings` type changed from `TemplateReferenceParameterMappings` to `Dictionary<string, string>`. The `TemplateReferenceParameterMappings` class has been removed.

Version 1.*:
```csharp
var overrideRef = new ChannelTemplateOverride
{
    ParameterMappings = new TemplateReferenceParameterMappings { Name = "name" }
};
```

Version 2.*:
```csharp
var overrideRef = new ChannelTemplateOverride
{
    ParameterMappings = new Dictionary<string, string>
    {
        ["bodytext"] = "name"
    }
};
```

## Conversation API: Template.Id is now nullable

`Template.Id` changed from `required string` to `string?`.

Version 1.*:
```csharp
var template = new Template { Id = "my-id" };
```

Version 2.*:
```csharp
var template = new Template();
```

## Conversation API: CreateTemplateRequest and UpdateTemplateRequest no longer expose create\_time and update\_time

`CreateTime` and `UpdateTime` have been removed from `CreateTemplateRequest` and `UpdateTemplateRequest`.

Version 1.*:
```csharp
var request = new CreateTemplateRequest
{
    DefaultTranslation = "en-US",
    Translations = [ /* ... */ ],
    CreateTime = DateTime.UtcNow
};
```

Version 2.*:
```csharp
var request = new CreateTemplateRequest
{
    DefaultTranslation = "en-US",
    Translations = [ /* ... */ ]
};
```

## Conversation API: Webhooks List now returns ListWebhooksResponse

`ISinchConversationWebhooks.List` now returns a response wrapper type instead of returning an enumerable directly.

Version 1.*:
```csharp
IEnumerable<Webhook> webhooks = await sinch.Conversation.Webhooks.List(appId);
```

Version 2.*:
```csharp
ListEventDestinationsResponse response = await sinch.Conversation.EventDestinations.List(appId);
IEnumerable<EventDestination> destinations = response.EventDestinations ?? Enumerable.Empty<EventDestination>();
```

## Conversation API: Webhooks request fields are now optional per OAS spec

The Webhook, CreateWebhookRequest, and UpdateWebhookRequest classes now have optional fields:

- **Webhook** class: `AppId` and `Triggers` are now optional (`string?` and `List<WebhookTrigger>?`).
- **CreateWebhookRequest** class: `Triggers` is now optional (`List<WebhookTrigger>?`).
- **UpdateWebhookRequest** class: `Target`, `AppId`, and `Triggers` are all now optional.

Version 1.*:
```csharp
var webhook = new CreateWebhookRequest
{
    AppId = appId,
    Target = "https://example.com/webhook",
    Triggers = new List<WebhookTrigger> { WebhookTrigger.MessageDelivery }
};
```

Version 2.*:
```csharp
// All fields remain available, but Triggers is now optional
var destination = new CreateEventDestinationRequest
{
    AppId = appId,
    Target = "https://example.com/webhook"
    // Triggers can be omitted
};

// UpdateEventDestinationRequest now allows partial updates
var update = new UpdateEventDestinationRequest
{
    Target = "https://new-endpoint.com/webhook"
    // AppId and Triggers can be omitted for partial updates
};
```

## Conversation API: Webhooks ValidateAuthenticationHeader no longer accepts JsonNode

`ISinchConversationWebhooks.ValidateAuthenticationHeader` now accepts only raw callback body strings.

Version 1.*:
```csharp
var body = JsonNode.Parse(rawBody);
var isValid = sinch.Conversation.Webhooks.ValidateAuthenticationHeader(headers, body!, secret);
```

Version 2.*:
```csharp
var isValid = sinch.Conversation.SinchEvents.ValidateAuthenticationHeader(headers, rawBody, secret);
```

## Conversation API: Webhooks ValidateAuthenticationHeader no longer accepts StringValues headers

`ISinchConversationWebhooks.ValidateAuthenticationHeader` no longer accepts `Dictionary<string, StringValues>`. Use either `IDictionary<string, string>` (single-value headers) or `IReadOnlyDictionary<string, IEnumerable<string>>` (multi-value headers).

Version 1.*:
```csharp
var headers = new Dictionary<string, StringValues>
{
    ["x-sinch-webhook-signature"] = new StringValues(signature)
};

var isValid = sinch.Conversation.Webhooks.ValidateAuthenticationHeader(headers, rawBody, secret);
```

Version 2.*:
```csharp
// Option A: single-value headers (e.g. from a plain dictionary)
var headers = new Dictionary<string, string>
{
    ["x-sinch-webhook-signature"] = signature
};

var isValid = sinch.Conversation.SinchEvents.ValidateAuthenticationHeader(headers, rawBody, secret);

// Option B: multi-value headers (e.g. from HttpContext.Request.Headers)
IReadOnlyDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>
{
    ["x-sinch-webhook-signature"] = new[] { signature }
};

var isValid = sinch.Conversation.SinchEvents.ValidateAuthenticationHeader(headers, rawBody, secret);
```

## Conversation API: Webhooks ParseEvent no longer accepts JsonNode

`ISinchConversationWebhooks.ParseEvent` now accepts raw JSON strings or streams only.

Version 1.*:
```csharp
var node = JsonNode.Parse(rawBody);
var callback = sinch.Conversation.Webhooks.ParseEvent(node!);
```

Version 2.*:
```csharp
var callback = sinch.Conversation.SinchEvents.ParseEvent(rawBody);
```

## Conversation API: Webhooks renamed to EventDestinations

The `Webhooks` subdomain and all related types have been renamed to use `EventDestinations` terminology.

Renamed types:

- `Webhook` → `EventDestination`
- `WebhookTargetType` → `EventDestinationTargetType`
- `WebhookTrigger` → `EventDestinationTrigger`
- `CreateWebhookRequest` → `CreateEventDestinationRequest`
- `UpdateWebhookRequest` → `UpdateEventDestinationRequest`
- `ListWebhooksResponse` → `ListEventDestinationsResponse` (property `.Webhooks` → `.EventDestinations`)
- `ISinchConversationWebhooks` → `ISinchConversationEventDestinations`
- `ISinchConversation.Webhooks` → `ISinchConversation.EventDestinations`
- Namespace `Sinch.Conversation.Webhooks` → `Sinch.Conversation.EventDestinations`

Version 1.*:
```csharp
using Sinch.Conversation.Webhooks;

var destination = await sinch.Conversation.Webhooks.Create(new CreateWebhookRequest
{
    AppId = appId,
    Target = "https://example.com/webhook",
    Triggers = [WebhookTrigger.MessageDelivery]
});

await sinch.Conversation.Webhooks.Delete(destination.Id!);
```

Version 2.*:
```csharp
using Sinch.Conversation.EventDestinations;

var destination = await sinch.Conversation.EventDestinations.Create(new CreateEventDestinationRequest
{
    AppId = appId,
    Target = "https://example.com/webhook",
    Triggers = [EventDestinationTrigger.MessageDelivery]
});

await sinch.Conversation.EventDestinations.Delete(destination.Id!);
```

## Conversation API: Sinch.Conversation.Hooks namespace renamed to Sinch.Conversation.SinchEvents

All Conversation event model types (`CapabilityEvent`, `ChannelEvent`, `ContactCreateEvent`, `InboundEvent`, etc.) have moved from the `Sinch.Conversation.Hooks` namespace to `Sinch.Conversation.SinchEvents`.

Version 1.*:
```csharp
using Sinch.Conversation.Hooks;
```

Version 2.*:
```csharp
using Sinch.Conversation.SinchEvents;
```

## Conversation API: CallbackSettings renamed to EventDestinationSettings

The `CallbackSettings` class and its `SecretForOverriddenCallbackUrls` property have been renamed. The underlying JSON wire format is unchanged (`callback_settings` / `secret_for_overridden_callback_urls`).

Version 1.*:
```csharp
using Sinch.Conversation.Apps.Create;

var request = new CreateAppRequest
{
    DisplayName = "My App",
    CallbackSettings = new CallbackSettings
    {
        SecretForOverriddenCallbackUrls = "my-secret"
    }
};
```

Version 2.*:
```csharp
using Sinch.Conversation.Apps.Create;

var request = new CreateAppRequest
{
    DisplayName = "My App",
    EventDestinationSettings = new EventDestinationSettings
    {
        SecretForOverriddenEventDestinationTarget = "my-secret"
    }
};
```

## Conversation API: SendEventRequest and SendMessageRequest CallbackUrl renamed to EventDestinationTarget

The `CallbackUrl` property on `SendEventRequest` and `SendMessageRequest` has been renamed to `EventDestinationTarget`. The underlying JSON wire format is unchanged (`callback_url`).

Version 1.*:
```csharp
var request = new SendMessageRequest
{
    AppId = appId,
    Message = AppMessage.From(new TextMessage("Hello")),
    Recipient = new ContactRecipient { ContactId = contactId },
    CallbackUrl = new Uri("https://example.com/delivery")
};
```

Version 2.*:
```csharp
var request = new SendMessageRequest
{
    AppId = appId,
    Message = AppMessage.From(new TextMessage("Hello")),
    Recipient = new ContactRecipient { ContactId = contactId },
    EventDestinationTarget = new Uri("https://example.com/delivery")
};
```

## Numbers API: CallbackConfiguration renamed to EventDestination

The `CallbackConfiguration` subdomain and all related types have been renamed to `EventDestination`.

- `ISinchNumbers.CallbackConfiguration` → `ISinchNumbers.EventDestination`
- `ISinchNumbersCallbackConfiguration` → `ISinchNumbersEventDestination`
- Class `CallbackConfiguration` → `EventDestination`
- Namespace `Sinch.Numbers.CallbackConfiguration` → `Sinch.Numbers.EventDestination`

Version 1.*:
```csharp
using Sinch.Numbers.CallbackConfiguration;

CallbackConfiguration config = await sinch.Numbers.CallbackConfiguration.Get();
```

Version 2.*:
```csharp
using Sinch.Numbers.EventDestination;

EventDestination config = await sinch.Numbers.EventDestination.Get();
```

## Numbers API: Sinch.Numbers.Hooks namespace renamed to Sinch.Numbers.SinchEvents; Event renamed to NumberSinchEvent

All Numbers event model types have moved from `Sinch.Numbers.Hooks` to `Sinch.Numbers.SinchEvents`. The `Event` class has been renamed to `NumberSinchEvent` to avoid ambiguity.

Version 1.*:
```csharp
using Sinch.Numbers.Hooks;

Event evt = JsonSerializer.Deserialize<Event>(json)!;
```

Version 2.*:
```csharp
using Sinch.Numbers.SinchEvents;

NumberSinchEvent evt = JsonSerializer.Deserialize<NumberSinchEvent>(json)!;
```

## Numbers API: CallbackUrl renamed to EventDestinationTarget

The `CallbackUrl` property has been renamed to `EventDestinationTarget` on the following classes. The underlying JSON wire format is unchanged (`callbackUrl`).

- `ActiveNumber`
- `UpdateActiveNumberRequest`
- `RentAnyNumberRequest`
- `RentActiveNumberRequest`

Version 1.*:
```csharp
var request = new RentAnyNumberRequest
{
    RegionCode = "US",
    Type = Types.Local,
    CallbackUrl = "https://example.com/callback"
};
```

Version 2.*:
```csharp
var request = new RentAnyNumberRequest
{
    RegionCode = "US",
    Type = Types.Local,
    EventDestinationTarget = "https://example.com/callback"
};
```

## Conversation API: ICallbackEvent and related types renamed

Renamed types:

- `ICallbackEvent` → `IConversationSinchEvent`
- `CallbackEventBase` → `ConversationSinchEventBase`
- `CallbackEventConverter` → `ConversationSinchEventConverter`
- `UnsupportedCallbackEvent` → `UnsupportedConversationSinchEvent`

Version 1.*:
```csharp
ICallbackEvent sinchEvent = sinch.Conversation.EventDestinations.ParseEvent(rawBody);
if (sinchEvent is UnsupportedCallbackEvent unsupported) { /* ... */ }
```

Version 2.*:
```csharp
IConversationSinchEvent sinchEvent = sinch.Conversation.EventDestinations.ParseEvent(rawBody);
if (sinchEvent is UnsupportedConversationSinchEvent unsupported) { /* ... */ }
```

## SMS API: Sinch.SMS.Hooks namespace renamed to Sinch.SMS.SinchEvents

All SMS sinch event types have moved from the `Sinch.SMS.Hooks` namespace to `Sinch.SMS.SinchEvents`. The subdomain property `sinch.Sms.Webhooks` has been renamed to `sinch.Sms.SinchEvents`, and the interface `ISmsWebhooks` has been renamed to `ISinchSmsSinchEvents`.

Version 1.*:
```csharp
using Sinch.SMS.Hooks;

var sinchEvent = sinch.Sms.Webhooks.ParseEvent(rawBody);
bool isValid = sinch.Sms.Webhooks.ValidateAuthenticationHeader(secret, headers, rawBody);
```

Version 2.*:
```csharp
using Sinch.SMS.SinchEvents;

var sinchEvent = sinch.Sms.SinchEvents.ParseEvent(rawBody);
bool isValid = sinch.Sms.SinchEvents.ValidateAuthenticationHeader(secret, headers, rawBody);
```

## SMS API: BatchBase and UpdateBatchBaseRequest CallbackUrl renamed to EventDestinationTarget

The `CallbackUrl` property on `BatchBase` (used when sending batches) and `UpdateBatchBaseRequest` (used when updating batches) has been renamed to `EventDestinationTarget`. The JSON wire format `callback_url` is unchanged.

Version 1.*:
```csharp
var batch = new SendSmsBatchRequest
{
    To = new List<string> { "+1234567890" },
    From = "+0987654321",
    Body = "Hello!",
    CallbackUrl = new Uri("https://example.com/sinch-events")
};
```

Version 2.*:
```csharp
var batch = new SendSmsBatchRequest
{
    To = new List<string> { "+1234567890" },
    From = "+0987654321",
    Body = "Hello!",
    EventDestinationTarget = new Uri("https://example.com/sinch-events")
};
```
