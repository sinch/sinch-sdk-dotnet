# Backend application built using Sinch .NET SDK to handle incoming webhooks

This directory contains a server application based onto [Sinch .NET SDK](https://github.com/sinch/sinch-sdk-dotnet)

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
    }
  }
}
```

- [`Sinch:ProjectId`](appsettings.json) - your Sinch project id
- [`Sinch:KeyId`](appsettings.json) - your Sinch API key id
- [`Sinch:KeySecret`](appsettings.json) - your Sinch API key secret
- [`Sinch:Sms:WebhookSecret`](appsettings.json) - optional shared secret used to validate incoming webhook signatures

### AllowedHosts configuration
The `appsettings.json` sets `"AllowedHosts": "*"` to allow all hosts during development, which is necessary for webhook development where external services (e.g., nGrok) must be able to POST (forward requests) to your endpoint. **In production, you should restrict this to Sinch's domain(s) for security.**

### Server URLs and ports
- Development launch settings ([Properties/launchSettings.json](Properties/launchSettings.json)) set the local URLs to:
  - https://localhost:5001
  - http://localhost:5000

When running under `dotnet run` in development, the app will typically listen on those URLs.

## How to run

Restore and run the project from the `examples/templates/webhooks` folder:

```powershell
# from repository root or examples/templates/webhooks
dotnet restore
dotnet run --project .\Webhook.Template.csproj
```

**Authentication / Validating callbacks**
The controller includes an optional validation step that checks headers and payload against a shared secret (configured at `Sinch:Sms:WebhookSecret`). The example defaults to `ensureValidAuthentication = false` — enable and configure it if you require strict validation of incoming callbacks.

## Use ngrok to forward request to local server

1. Start the app locally (see "How to run").
2. In another shell, run ngrok to forward your local port (use 5000 for HTTP or 5001 for HTTPS):

```powershell
ngrok http 5000
```

3. ngrok will show a forwarding HTTPS URL such as `https://xxxx.ngrok.app`.
This value must be used to configure callback's URL from your [Sinch dashboard](https://dashboard.sinch.com/sms/api/services)

   For example, set your Sinch callback URL to `https://xxxx.ngrok.app/SmsEvent` in the Sinch dashboard above to handle SMS events.

