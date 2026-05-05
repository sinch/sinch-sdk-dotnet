# ASP.NET Core template for receiving Sinch Events

This directory contains a server application based on the [Sinch .NET SDK](https://github.com/sinch/sinch-sdk-dotnet) that demonstrates how to receive and validate incoming Sinch Events for **SMS** and **Numbers**.

## Requirements
- .NET 8 SDK
- [Sinch account](https://dashboard.sinch.com)
- [ngrok](https://ngrok.com/docs)

## Configuration
The sample reads configuration from [appsettings.json](appsettings.json):

```json
{
  "Sinch": {
    "ProjectId": "",
    "KeyId": "",
    "KeySecret": "",
    "Sms": {
      "WebhookSecret": ""
    },
    "Numbers": {
      "HmacSecret": ""
    }
  }
}
```

- `Sinch:ProjectId`, `Sinch:KeyId`, `Sinch:KeySecret` are unified credentials required for SMS event parsing
- `Sinch:Sms:WebhookSecret` is optional shared secret for validating SMS event signatures
- `Sinch:Numbers:HmacSecret` is optional shared secret for validating Numbers event signatures

## How to run

```powershell
cd examples\templates\sinchevents
dotnet restore
dotnet run
```

## SMS Events

The [Sms/](Sms/) folder contains a controller that receives SMS delivery reports and inbound messages. Authentication is controlled by the `[SmsSinchEvent(requireAuthentication: false)]` attribute — set to `true` and configure `Sinch:Sms:WebhookSecret` to validate signatures.

## Numbers Events

The [Numbers/](Numbers/) folder contains a controller that receives Numbers event notifications (e.g. number provisioning status changes). Authentication is controlled by the `[NumbersSinchEvent(requireAuthentication: false)]` attribute — set to `true` and configure `Sinch:Numbers:HmacSecret` to validate signatures.

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
