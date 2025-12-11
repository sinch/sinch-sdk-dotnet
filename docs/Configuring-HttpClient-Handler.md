# Configuring HTTP Client Handler

## Quick Reference

[`HttpClientHandlerConfiguration`](https://github.com/sinch/sinch-sdk-dotnet/blob/main/src/Sinch/HttpClientHandlerConfiguration.cs) allows **console apps and other non-DI scenarios** to customize HTTP client behavior (DNS refresh, connection pooling, connection limits).

### When to Use

| Scenario | Use HttpClientHandlerConfiguration? | Configure How? |
|----------|-------------------------------------|----------------|
| **Console App** | ✅ Yes | Via `SinchOptions.HttpClientHandlerConfiguration` |
| **Desktop App** | ✅ Yes | Via `SinchOptions.HttpClientHandlerConfiguration` |
| **ASP.NET Core** | ❌ No | Via `IHttpClientBuilder.ConfigurePrimaryHttpMessageHandler()` |
| **Worker Service with DI** | ❌ No | Via `IHttpClientBuilder.ConfigurePrimaryHttpMessageHandler()` |

---

## Overview

For **console apps** and other **non-DI scenarios**, you can now customize the `SocketsHttpHandler` configuration that controls DNS refresh, connection pooling, and connection limits.

**Note:** This configuration only applies to non-DI scenarios. ASP.NET Core apps should use `ConfigurePrimaryHttpMessageHandler()` on `IHttpClientBuilder` instead.

---

## Custom Configuration

### Example: Faster DNS Updates

```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
    },
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration()
    }
});
```

### Example: High-Throughput Application

```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ },
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration()
    }
});
```

### Example: Rate-Limited API

```csharp
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ },
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration()
    }
});
```

---

## ASP.NET Core: Different Approach

For ASP.NET Core, **don't use `HttpClientHandlerConfiguration`** - it's ignored when you provide `IHttpClientFactory`.

### ❌ Wrong (Ignored)

```csharp
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration { /* ... */ } // ❌ Ignored!
    }
});
```

### ✅ Correct (Via IHttpClientBuilder)

```csharp
builder.Services.AddSinchClient(() => new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials { /* ... */ }
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
    MaxConnectionsPerServer = 20
});
```

---

## FAQ

### Q: When does this configuration apply?

**A:** Only for **console apps, desktop apps, Unity, etc.**—scenarios where you use `new SinchClient()` without dependency injection.

### Q: What if I provide both `HttpClientFactory` and `HttpClientHandlerConfiguration`?

**A:** The `HttpClientHandlerConfiguration` is ignored. When you provide a custom `IHttpClientFactory`, you're responsible for configuring it.

```csharp
var myFactory = /* ... your factory ... */;

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchOptions = new SinchOptions
    {
        HttpClientFactory = myFactory, // ✅ Used
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration { /* ... */ } // ❌ Ignored
    }
});
```

### Q: Can I get the default configuration?

**A:** Yes, use the static `Default` property:

```csharp
var defaultConfig = HttpClientHandlerConfiguration.Default;
// PooledConnectionLifetime: 5 minutes
// PooledConnectionIdleTimeout: 2 minutes
// MaxConnectionsPerServer: 10
```

### Q: How do I know which values to use?

**A:** Start with the defaults. Only adjust if you're experiencing specific issues:

| Issue | Adjustment |
|-------|------------|
| DNS changes not picked up fast enough | Lower `PooledConnectionLifetime` |
| Too many connections being created | Increase `PooledConnectionLifetime` |
| Hitting rate limits | Decrease `MaxConnectionsPerServer` |
| Need higher throughput | Increase `MaxConnectionsPerServer` |
| Connections timing out from server side | Lower `PooledConnectionIdleTimeout` |
| Too much connection overhead | Increase `PooledConnectionIdleTimeout` |

### Q: My configuration doesn't seem to apply

**A:** Check if you're using ASP.NET Core. For DI scenarios, use `ConfigurePrimaryHttpMessageHandler()` instead.

### Q: Should I always customize this?

**A:** No! The defaults work well for most scenarios. Only customize if you have specific needs.

### Q: Can I use these constants elsewhere?

**A:** Yes! The constants are public:
```csharp
DefaultHttpClientFactory.DefaultPooledConnectionLifetime     // 5 minutes
DefaultHttpClientFactory.DefaultPooledConnectionIdleTimeout  // 2 minutes
DefaultHttpClientFactory.DefaultMaxConnectionsPerServer      // 10
```

---

## Complete Example

Here's a complete console app example with custom configuration:

```csharp
using Sinch;

// Load environment variables
var projectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!;
var keyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!;
var keySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!;

// Create client with custom handler configuration
var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    SinchOptions = new SinchOptions
    {
        HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration
        {
            // Customize based on your needs
            PooledConnectionLifetime = TimeSpan.FromMinutes(3), // Faster DNS updates
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
            MaxConnectionsPerServer = 15 // Higher throughput
        }
    }
});

// Use the client
var numbers = await sinch.Numbers.List(new ListActiveNumbersRequest
{
    RegionCode = "US",
    Type = NumberType.Mobile
});

Console.WriteLine($"Found {numbers.ActiveNumbers.Count()} numbers");
```

---

## Related Documentation

- [HTTP Client Refactoring Overview](HTTP-Client-Refactoring-Overview.md) - Why we made these changes
- [Documentation Index](README.md) - All HTTP client management documentation
- [Microsoft: SocketsHttpHandler](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.socketshttphandler) - Official docs

---

**Document Version:** 1.0  
**Last Updated:** 2025  
**SDK Version:** v2.0  
**Target Frameworks:** .NET 8, .NET 9
