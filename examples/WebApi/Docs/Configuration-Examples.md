# Program.cs Configuration Examples

This document shows various ways to configure the Sinch SDK in ASP.NET Core's `Program.cs` file.

## Example 1: Basic Configuration (Most Common)

This is the standard setup - works great for most scenarios:

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

**When to use:** Default choice for most applications.

---

## Example 2: High-Throughput Configuration

For applications that need more concurrent connections:

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
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
    MaxConnectionsPerServer = 30  // Increased for high throughput
});
```

**When to use:** Bulk SMS sending, high-volume API calls.

---

## Example 3: With Polly Retry Policy

Adds automatic retries with exponential backoff:

```csharp
// Add Polly package first:
// dotnet add package Polly.Extensions.Http

using Polly;

builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.AddPolicyHandler(Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
```

**When to use:** Unreliable network conditions, transient failures.

---

## Example 4: With Circuit Breaker

Prevents cascading failures:

```csharp
// Add Polly package first:
// dotnet add package Polly.Extensions.Http

using Polly;

builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.AddPolicyHandler(Policy
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

**When to use:** Protecting downstream services, preventing cascade failures.

---

## Example 5: Fast DNS Refresh

For multi-region deployments where DNS changes frequently:

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
.SetHandlerLifetime(TimeSpan.FromMinutes(2))  // Faster DNS refresh
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(2),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
    MaxConnectionsPerServer = 10
});
```

**When to use:** Active-active deployments, frequent DNS changes.

---

## Example 6: Rate-Limited Configuration

For staying under API rate limits:

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
.SetHandlerLifetime(TimeSpan.FromMinutes(10))  // Less frequent rotation
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
    MaxConnectionsPerServer = 3  // Fewer connections to avoid rate limits
});
```

**When to use:** Staying under API rate limits, controlled request rates.

---

## Example 7: Complete Configuration

All features combined:

```csharp
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = builder.Configuration["Sinch:ProjectId"]!,
        KeyId = builder.Configuration["Sinch:KeyId"]!,
        KeySecret = builder.Configuration["Sinch:KeySecret"]!
    }
    // Optional: Add more service configurations
    // VerificationConfiguration = new SinchVerificationConfiguration { ... },
    // VoiceConfiguration = new SinchVoiceConfiguration { ... }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  // DNS refresh interval
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
    MaxConnectionsPerServer = 20
})
.ConfigureHttpClient(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    // Add custom headers if needed
    // client.DefaultRequestHeaders.Add("X-Custom-Header", "value");
});
```

---

## Example 8: With Custom Polly Policies

Advanced resilience configuration:

```csharp
// Helper methods
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s delay");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (outcome, timespan) =>
            {
                Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker reset");
            });
}

// Usage
builder.Services.AddSinchClient(() => config)
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
```

---

## Configuration Comparison

| Scenario | DNS Refresh | Idle Timeout | Max Connections | When to Use |
|----------|-------------|--------------|-----------------|-------------|
| **Default** | 5 min | 2 min | 10 | Most applications |
| **High-Throughput** | 5 min | 2 min | 30 | Bulk operations |
| **Fast DNS** | 2 min | 1 min | 10 | Multi-region deployments |
| **Rate-Limited** | 10 min | 5 min | 3 | Avoiding rate limits |

---

## What NOT to Do

### ❌ Don't Use HttpClientHandlerConfiguration

```csharp
// This is IGNORED in ASP.NET Core!
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration  // ❌ Ignored!
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(3)
        }
    }
});
```

`HttpClientHandlerConfiguration` is for console apps only! In ASP.NET Core, use `IHttpClientBuilder` methods.

### ❌ Don't Create Multiple Registrations

```csharp
// ❌ Don't do this!
builder.Services.AddSinchClient(() => config1);
builder.Services.AddSinchClient(() => config2);  // Overwrites the first one!
```

Only call `AddSinchClient()` once.

---

## See Also

- [README.md](README.md) - WebAPI examples overview
- [HttpClientConfigurationController.cs](Controllers/Configuration/HttpClientConfigurationController.cs) - Controller implementation
- [Configuring HttpClient Handler](../../docs/Configuring-HttpClient-Handler.md) - Complete documentation
