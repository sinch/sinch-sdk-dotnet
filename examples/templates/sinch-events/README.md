# ASP.NET Core template for receiving Sinch Events

This directory contains a server application based on the [Sinch .NET SDK](https://github.com/sinch/sinch-sdk-dotnet) that demonstrates how to receive and validate incoming Sinch Events.

## Requirements
- .NET 8 SDK
- [Sinch account](https://dashboard.sinch.com)
- [ngrok](https://ngrok.com/docs)

## Configuration
The sample reads configuration from [appsettings.json](appsettings.json):

```json
{
  "Sinch": {
    "Sms": {
      "SinchEventsSecret": ""
    },
    "Numbers": {
      "SinchEventsSecret": ""
    },
    "Verification": {
      "AppKey": "",
      "AppSecret": ""
    }
  }
}
```

- No unified credentials or SMS region are required to parse or validate incoming Sinch Events in this template.
- `Sinch:Sms:SinchEventsSecret` is an optional shared secret for validating SMS event signatures.
- `Sinch:Numbers:SinchEventsSecret` is an optional shared secret for validating Numbers event signatures.
- `Sinch:Verification:AppKey` and `Sinch:Verification:AppSecret` are only needed when you enable Verification signed-request validation in this template. They are not required for `ParseEvent(...)` by itself.

If you later extend this sample to make authenticated API calls, add a configured `SinchClientConfiguration` with unified credentials and any product-specific settings that those outbound API calls require.

## How to run

```powershell
cd examples\templates\sinch-events
dotnet restore
dotnet run
```

## SMS Events

The [Sms/](Sms/) folder contains a controller that receives SMS delivery reports and inbound messages. Authentication is controlled by the `[SmsSinchEvent(requireAuthentication: false)]` attribute. Set it to `true` and configure `Sinch:Sms:SinchEventsSecret` if you want to validate incoming signatures.

## Numbers Events

The [Numbers/](Numbers/) folder contains a controller that receives Numbers event notifications (e.g. number provisioning status changes). Authentication is controlled by the `[NumbersSinchEvent(requireAuthentication: false)]` attribute. Set it to `true` and configure `Sinch:Numbers:SinchEventsSecret` if you want to validate incoming signatures.

## Use ngrok to forward requests to your local server

1. Start the app locally (see "How to run").
2. In another shell, run ngrok to forward your local port (use 5000 for HTTP or 5001 for HTTPS):

```powershell
ngrok http 5000
```

3. ngrok will show a forwarding HTTPS URL such as `https://xxxx.ngrok.app`.
   Use this URL to configure your event destination in the [Sinch dashboard](https://dashboard.sinch.com).

   - **SMS**: Set your event destination URL to `https://xxxx.ngrok.app/SmsEvent`
   - **Numbers**: Set your event destination URL to `https://xxxx.ngrok.app/NumbersEvent`

## Verification Events

The [Verification/](Verification/) folder contains a controller that receives Verification Sinch Events. Authentication is controlled by the `[VerificationSinchEvent(requireAuthentication: false)]` attribute. Set it to `true` and configure `Sinch:Verification:AppKey` and `Sinch:Verification:AppSecret` if you want to validate incoming signed requests.

- The template parses the incoming Verification event and switches on the concrete event type.
- For `VerificationStartEvent`, the business logic returns a simple `ALLOW` response that the controller serializes with `SerializeResponse(...)`.
- For `VerificationResultEvent`, the business logic logs the incoming event and the controller returns `200 OK`.
- Use the endpoint `https://xxxx.ngrok.app/VerificationEvent` as your Verification event destination URL.
