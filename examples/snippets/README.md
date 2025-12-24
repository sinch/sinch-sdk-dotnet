# Sinch .NET SDK Code Snippets

Code snippets demonstrating how to use the Sinch .NET SDK.

Snippets can be used as a starting point to integrate Sinch products into your own application.

## Requirements

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) or later
- [Sinch account](https://dashboard.sinch.com)

## Snippet Execution

Each snippet is a standalone project that can be run directly using the `dotnet` CLI.

### Configuration

When executing a snippet you will need to provide certain information about your Sinch account (credentials, Sinch virtual phone number, etc.).

#### Option 1: Using Shared `appsettings.json` (Recommended)

Create a single configuration file at the snippets root that all snippets will use:

```powershell
cd examples/snippets
copy appsettings.example.json appsettings.json
```

Then edit `appsettings.json` with your credentials:

```json
{
  "SINCH_PROJECT_ID": "your-project-id",
  "SINCH_KEY_ID": "your-key-id",
  "SINCH_KEY_SECRET": "your-key-secret",
  "SINCH_PHONE_NUMBER": "your-sinch-phone-number",
  "SINCH_SERVICE_PLAN_ID": "your-service-plan-id"
}
```

This single file will be used by all snippets, so you only need to configure your credentials once.

#### Option 2: Using Environment Variables

Alternatively, set environment variables directly in your shell:

**PowerShell:**
```powershell
$env:SINCH_PROJECT_ID = "your-project-id"
$env:SINCH_KEY_ID = "your-key-id"
$env:SINCH_KEY_SECRET = "your-key-secret"
```

**Command Prompt:**
```cmd
set SINCH_PROJECT_ID=your-project-id
set SINCH_KEY_ID=your-key-id
set SINCH_KEY_SECRET=your-key-secret
```

**Bash (Linux/macOS):**
```bash
export SINCH_PROJECT_ID="your-project-id"
export SINCH_KEY_ID="your-key-id"
export SINCH_KEY_SECRET="your-key-secret"
```

#### Configuration Priority

The configuration is loaded in the following order (highest priority first):

1. **Environment variables**
2. **Shared `appsettings.json`** at the snippets root

### Running a Snippet

From the SDK root:

```powershell
dotnet run --project examples/snippets/<SNIPPET_PATH>/<Snippet>.csproj
```

Or navigate to the snippet folder and run:

```powershell
dotnet run
```

#### Examples

From the SDK root:

```powershell
dotnet run --project examples/snippets/numbers/availableNumbers/List/List.csproj
```

From snippet folder:

```powershell
cd examples/snippets/numbers/availableNumbers/List
dotnet run
```

### Running from Visual Studio

Open `Snippets.sln` in Visual Studio. This solution contains all snippet projects, allowing you to browse, edit, and run any snippet directly from the IDE.

To run a snippet:
1. Right-click on the desired snippet project in the Solution Explorer
2. Select **Set as Startup Project**
3. Press `F5` or click **Start**

## Available Snippets

- Numbers: [numbers/](numbers/)
