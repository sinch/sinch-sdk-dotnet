# Sinch WebAPI Example

This example demonstrates how to use the Sinch .NET SDK in an ASP.NET Core Web API application with proper dependency injection and HttpClient configuration.

## Prerequisites

- .NET 8.0 SDK or later
- Sinch account with API credentials ([Sign up here](https://dashboard.sinch.com))

## Configuration

This example uses **User Secrets** to securely store your Sinch API credentials during development. User Secrets are stored outside your project directory and are never committed to source control.

### Setting Up User Secrets

#### Option 1: Using Visual Studio
1. Right-click on the `WebApiExamples` project
2. Select **Manage User Secrets**
3. Add your Sinch credentials:

```json
{
  "Sinch": {
    "ProjectId": "your-actual-project-id",
    "KeyId": "your-actual-key-id",
    "KeySecret": "your-actual-key-secret"
  }
}
```

#### Option 2: Using .NET CLI
1. Navigate to the WebApi example directory:
   ```bash
   cd examples/WebApi
   ```

2. Initialize user secrets (if not already done):
   ```bash
   dotnet user-secrets init
   ```

3. Set your Sinch credentials:
   ```bash
   dotnet user-secrets set "Sinch:ProjectId" "your-actual-project-id"
   dotnet user-secrets set "Sinch:KeyId" "your-actual-key-id"
   dotnet user-secrets set "Sinch:KeySecret" "your-actual-key-secret"
   ```

4. Verify your secrets:
   ```bash
   dotnet user-secrets list
   ```

### Finding Your Sinch Credentials

1. Go to [Sinch Dashboard](https://dashboard.sinch.com)
2. Navigate to your project
3. Go to **Settings** → **Access Keys**
4. Copy your:
   - **Project ID**
   - **Key ID** 
   - **Key Secret**

## Running the Application

1. Ensure you've configured your User Secrets (see above)

2. Run the application:
   ```bash
   dotnet run
   ```

3. The API will be available at:
   - HTTP: `http://localhost:5000`
   - Swagger UI: `https://localhost:5000/swagger`

