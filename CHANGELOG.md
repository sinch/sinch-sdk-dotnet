# Changelog

All notable changes to the **Sinch .NET SDK** are documented in this file.

> **Tag legend**
> - `[feature]` — new capability
> - `[fix]` — bug fix
> - `[deprecation notice]` — will be removed in a future release
> - `[dependency]` — third-party library update
> - `[doc]` — documentation only
> - `[test]` — test coverage
> - `[refactor]` — internal restructuring
> - `[releasing]` — release infrastructure
> - `[design]` — API design change
> - `[tech]` — technical improvement

---

## unreleased

### Major breaking changes with major release
- see [MIGRATION_GUIDE](MIGRATION_GUIDE.md#200)

### Verification
- **[feature]** `ISinchVerificationClient.SinchEvents` exposes `ParseEvent` and `SerializeResponse`
- **[feature]** `IVerificationSinchEvent` interface introduced — `ParseEvent` returns `IVerificationSinchEvent`
- **[feature]** `VerificationSmsDeliveredEvent` Sinch event added
- **[feature]** New `SmsCodeType` enum (`Numeric`, `Alpha`, `Alphanumeric`)
- SMS
    - **[feature]** `VerificationStartEventResponseSms.Sms` aligned with OAS: added `CodeType` (`SmsCodeType`), `Expiry` (`string`), and `AdditionalProperties`
- FlashCall
    - **[feature]** `VerificationStartEventResponseFlashCall.FlashCall` aligned with OAS: added `InterceptionTimeout` (`int`) and `AdditionalProperties`
- PhoneCall
    - **[feature]** `VerificationStartEventResponsePhoneCall.PhoneCall` aligned with OAS: added `AdditionalProperties`

### Fax
- **[feature]** `DownloadContent` no longer calls the deprecated `GET /faxes/{id}/file.{fileFormat}` endpoint; uses the current `GET /faxes/{id}/file` path instead

## v1.5.1 - 2026-09-02

### Core

- **[fix]** Fix [#214](https://github.com/sinch/sinch-sdk-dotnet/issues/214): 
  - `ObjectDisposedException` when the Sinch SDK retries after a `401 Unauthorized` response
  - Deterministic HTTP response disposal on JSON, empty responses and streaming PDF downloads

## v1.5.0 - 2026-06-01

### Conversation

- **[feature]** Support additional choice messages: `Share Location` and `Calendar`

## v1.4.3 - 2026-02-27

### Core

- **[fix]** API error response deserialization issue

## v1.4.2 - 2026-02-27

### Core

- **[fix]** API error response deserialization issue

## v1.4.1 - 2026-01-12

### Core

- **[fix]** Added SDK `User-Agent` header to authentication requests

### Conversation

- **[fix]** Fixed Webhooks deserialization containing unexpected properties

## v1.4.0 - 2025-09-02

### Conversation

- **[deprecation notice]** `ListMessageRequest.MessageSource` deprecated in favor of `ListMessageRequest.MessagesSource` (plural form)
- **[feature]** `ConversationMessage` supports `SenderId` and `ProcessingMode` fields

### Numbers

- **[fix]** Fix end of auto pagination detection based on empty token
- **[fix]** Fix null value usage for `types` on `Regions.List`
- **[fix]** Fix scheduled provisioning serialization/deserialization
- **[feature]** Support `CallbackUrl` field on `ActiveNumber` and related requests
- **[feature]** Enhanced `EventType` values for Webhooks
- **[feature]** Provide Webhooks header validation `ValidateAuthenticationHeader` helper

### SMS

- **[fix]** Fix end of auto pagination detection for `Batches`, `DeliveryReport`, `Groups`, `InBounds`

### Verification

- **[fix]** Fix `ReportFlashCallVerificationRequest` serialization
- **[feature]** Provide Webhooks header validation `ValidateAuthenticationHeader` helper
- **[feature]** Provide generic `GetById`, `GetByIdentity` and `GetByReference` functions
- **[feature] [BETA]** Support WhatsApp verification. This is a beta feature, contact your account manager.

### Voice

- **[fix]** Fix `QueryNumberResponse` deserialization
- **[fix]** Fix `PlayFiles.Ids` field type
- **[feature]** Provide Webhooks header validation `ValidateAuthenticationHeader` helper
- **[feature]** Support Webhooks `AnsweredCallEvent.ApplicationKey` field
- **[feature]** Support Webhooks `DisconnectedCallEvent.ConferenceId` field
- **[feature]** Support Webhooks `IncomingCallEvent.ConferenceId` field
- **[feature]** Support Webhooks `PromptInputEvent.ConferenceId` and `PromptInputEvent.Custom` fields

### Tests

- **[test]** Added end-to-end tests to CI

## v1.3.1 - 2025-08-08

### Conversation

- **[fix]** Messages: `message_source` vs `messages_source` query parameter
- **[feature]** Messages: Support missing `sender_id` and `processing_mode` fields
- **[deprecation notice]** Messages: `message_source` is deprecated in favor of `messages_source`

## v1.3.0 - 2025-05-08

### Conversation

- **[feature]** Templates v1 support
- **[fix]** OAS sync
- **[feature]** Change `ExpirationTime` to `DateTime`

### SMS

- **[fix]** OAS sync

### Numbers

- **[feature]** Add callback configuration endpoints

### Voice

- **[feature]** Implement distinct voice configs and provisioning

### Core

- **[refactor]** Make classes sealed
- **[feature]** Update `PaymentOrderDetailsChannelSpecificMessagePayment` type enum with additional value

## v1.2.2 - 2025-05-08

### Voice

- **[fix]** Voice events deserialization

## v1.2.1 - 2025-05-06

### Conversation

- **[fix]** Split template and omnitemplate

## v1.2.0 - 2025-02-13

### Conversation

- **[fix]** Make locale non-required
- **[feature]** Add `BR` region, extract and test URL resolutions
- **[fix]** Deserialization of callback events

### Numbers

- **[fix]** Make `ScheduledProvisioning` and `ScheduledVoiceProvisioning` read-only
- **[fix]** Rename `MessagePart` to `NumberOfParts` and change type `string` to `int`

### Verification

- **[fix]** Start SMS empty options

### Voice

- **[feature]** Sync voice models

### Fax

- **[feature]** Add Fax API support

### Core

- **[fix]** Remove unused `RestSharp` package
- **[dependency]** Update CODEOWNERS

## v1.1.2 - 2024-10-16

### Numbers

- **[feature]** Methods to root of numbers

### Core

- **[fix]** Upgrade `System.Text.Json` to `8.0.5`

### Build & CI

- **[tech]** Build restore with correct framework

### Tests

- **[test]** Return test and check unicode test

## v1.1.1 - 2024-07-16

### Core

- **[fix]** Check for expired token in any header value

## v1.1.0 - 2024-07-15

### Verification

- **[feature]** Add `Accept-Language` header for SMS verification

### Core

- **[feature]** Use `int` for TTL
- **[dependency]** Update `System.Text.Json` to `8.0.4`

## v1.0.1 - 2024-05-13

### Voice

- **[fix]** Handle zero byte array in app signature

## v1.0.0 - 2024-05-07

### Core

- **[feature]** Enable nullable reference types
- **[refactor]** Make region names consistent
- **[refactor]** Adjust data types

### Voice

- **[refactor]** Refs cleanup
- **[fix]** Update models
- **[fix]** Remove unused `eventType` field
- **[feature]** Add webhook validation method
- **[fix]** Get call model, adjust field types

### Conversation

- **[feature]** Add `KakaoTalkChatCredentials`
- **[fix]** Add specific object into list message
- **[feature]** Add `OmniMessageOverride`

### Examples

- **[feature]** Add handling of incoming call event

### Documentation

- **[doc]** Update README, remove warning

## v0.1.19-alpha - 2024-04-17

### Conversation

- **[fix]** Conversation models
- **[feature]** Support service plan ID
- **[fix]** Channel specific message type

### Build & CI

- **[tech]** Export `.editorconfig`
- **[tech]** Add formatting check

### Documentation

- **[doc]** Add linking in README.md

## v0.1.18-alpha - 2024-03-25

### Conversation

- **[feature]** Support for Conversation API Events, Transcoding, and Capability endpoints
- **[feature]** Support for Conversation Templates v2
- **[feature]** Support for Conversation Callbacks

## v0.1.17-alpha - 2024-02-21

### Verification

- **[feature]** Dedicated report for each method

### Conversation

- **[feature]** Webhooks support

### Core

- **[feature]** Allow client initialization without providing parameters

### Documentation

- **[doc]** Update README.md

## v0.1.16-alpha - 2024-02-05

### Build & CI

- **[tech]** Add documentation XML file in NuGet package

## v0.1.15-alpha - 2024-02-01

### Conversation

- **[feature]** Conversation contacts
- **[feature]** Conversations

### Voice

- **[feature]** Dedicated start verification functions

## v0.1.14-alpha - 2024-01-22

### Voice

- **[feature]** Added support for [Voice API](https://developers.sinch.com/docs/voice/)

### Core

- **[feature]** Allow overriding API URL with `SinchOptions`
- **[feature]** Add `User-Agent` header to share SDK version and .NET runtime version

## v0.1.13-alpha - 2023-12-13

### Build & CI

- **[tech]** Add support for net8

## v0.1.12-alpha - 2023-12-01

### Build & CI

- **[tech]** Drop net5 support

## v0.1.11-alpha - 2023-11-30

### Voice

- **[feature]** Implement Calls endpoints

### Core

- **[fix]** Initialization of `HttpClient` in `SinchClient`

## v0.1.10-alpha - 2023-11-29

### SMS

- **[refactor]** Rename `SmsRegion` to `SmsHostingRegion`
- **[refactor]** Great renaming of types
- **[feature]** Support Media and Binary SMS with batches

### Voice

- **[feature]** Introduce voice (work-in-progress)

## v0.1.9-alpha - 2023-10-27

### SMS

- **[fix]** Fix group ID in update group request

### Core

- **[fix]** Extend `ApiException` with more detailed message

## v0.1.8-alpha - 2023-10-26

### Verification

- **[feature]** Add support for [Verification API](https://developers.sinch.com/docs/verification/api-reference/)

## v0.1.7-alpha - 2023-09-21

### Core

- **[refactor]** Move to loose enums in form of a record
- **[fix]** Minor fixes

## v0.1.6-alpha - 2023-09-11

### Conversation

- **[feature]** Add coverage of [Conversation App endpoints](https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/App/)

## v0.1.5-alpha - 2023-08-31

### Conversation

- **[feature]** Add [Conversation API Messages](https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/Messages/)

## v0.1.4-alpha - 2023-08-21

### SMS

- **[feature]** Add SMS, Numbers webhook classes

### Core

- **[fix]** Minor Request and Response type adjustments

## v0.1.3-alpha - 2023-08-04

- **[releasing]** Initial alpha release
