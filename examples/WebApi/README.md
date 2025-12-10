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

## API Configuration

### Lowercase URLs

This example is configured to use lowercase URLs for all endpoints:

```csharp
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
});
```

All routes will automatically be lowercase (e.g., `numbers/regions` instead of `Numbers/regions`).

### HttpClient Configuration

This example demonstrates the **recommended** way to configure the Sinch SDK in ASP.NET Core applications:

```csharp
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  // DNS refresh every 5 minutes
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),   // DNS refresh interval
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2), // Close idle connections
    MaxConnectionsPerServer = 10  // HTTP/1.1 best practice
});
```

### Why This Configuration?

- **IHttpClientFactory Integration**: Properly manages HttpClient lifetime and prevents socket exhaustion
- **DNS Refresh**: Ensures the application picks up DNS changes every 5 minutes
- **Connection Pooling**: Efficiently reuses connections while respecting server limits
- **Production-Ready**: Follows Microsoft's best practices for HttpClient usage
- **Lowercase URLs**: RESTful convention for better API consistency

## Production Configuration

For production environments, use one of these approaches instead of User Secrets:

### Option 1: Azure App Service Configuration
Set application settings in the Azure Portal:
- `Sinch__ProjectId`
- `Sinch__KeyId`
- `Sinch__KeySecret`

### Option 2: Azure Key Vault
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

### Option 3: Environment Variables
```bash
export Sinch__ProjectId="your-project-id"
export Sinch__KeyId="your-key-id"
export Sinch__KeySecret="your-key-secret"
```

### Option 4: appsettings.Production.json (with proper security)
Create `appsettings.Production.json` with encrypted values or references to secure storage:
```json
{
  "Sinch": {
    "ProjectId": "#{Sinch.ProjectId}#",  // Token replacement
    "KeyId": "#{Sinch.KeyId}#",
    "KeySecret": "#{Sinch.KeySecret}#"
  }
}
```

## Security Best Practices

✅ **DO:**
- Use User Secrets for local development
- Use Azure Key Vault or similar for production
- Use environment variables in containers/CI/CD
- Rotate credentials regularly

❌ **DON'T:**
- Commit credentials to source control
- Put credentials in appsettings.json files
- Share credentials in team chat/email
- Hardcode credentials in code

## Troubleshooting

### "Configuration value is null" error
- Ensure User Secrets are configured correctly
- Run `dotnet user-secrets list` to verify
- Check that key names match exactly (case-sensitive)

### HttpClient socket exhaustion
- This example uses `IHttpClientFactory` to prevent this issue
- Don't create `SinchClient` instances directly in controllers
- Always use dependency injection

## Additional Resources

- [Sinch .NET SDK Documentation](https://developers.sinch.com/docs/sdks/dotnet/)
- [User Secrets in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [HttpClient Best Practices](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines)
- [Lowercase URLs in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing#url-generation-reference)
- [Sinch Dashboard](https://dashboard.sinch.com)
