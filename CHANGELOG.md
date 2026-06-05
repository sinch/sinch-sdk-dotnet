# Changelog

All notable changes to the **Sinch .NET SDK** are documented in this file.

> **Tag legend**
> - `[feature]` — new capability
> - `[fix]` — bug fix
> - `[deprecation notice]` — will be removed in a future release
> - `[refactor]` — internal restructuring
> - `[doc]` — documentation only
> - `[test]` — test coverage
> - `[design]` — API design change

---

## vNext – unreleased

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

---
