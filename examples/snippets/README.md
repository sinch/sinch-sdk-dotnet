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

These settings can be placed directly in the snippet source or you can set environment variables, in which case they will be shared and used automatically by each snippet.

```powershell
$env:SINCH_PROJECT_ID = "your-project-id"
$env:SINCH_KEY_ID = "your-key-id"
$env:SINCH_KEY_SECRET = "your-key-secret"
```

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

Open `Snippets.slnx` in Visual Studio. This solution contains all snippet projects, allowing you to browse, edit, and run any snippet directly from the IDE.

To run a snippet:
1. Right-click on the desired snippet project in the Solution Explorer
2. Select **Set as Startup Project**
3. Press `F5` or click **Start**

## Available Snippets

- Numbers: [numbers/](numbers/)
