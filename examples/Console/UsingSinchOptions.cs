using Sinch;

namespace Examples;

/// <summary>
/// Demonstrates various ways to use SinchOptions for configuring the Sinch SDK.
/// This includes HttpClientFactory, HttpClientHandlerConfiguration, and LoggerFactory.
/// </summary>
public class UsingSinchOptions
{
    public void Example()
    {
        // Example 1: Basic usage - No SinchOptions (uses defaults)
        // This is the recommended approach for most console applications
        var sinchBasic = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            }
            // No SinchOptions needed - uses DefaultHttpClientFactory with sensible defaults
            // ✅ Automatic DNS refresh every 5 minutes
            // ✅ Proper connection pooling
            // ✅ Socket exhaustion prevention
        });

        // Example 2: Custom HttpClientHandlerConfiguration
        // For scenarios where you need to customize DNS refresh timing or connection limits
        var sinchCustomHandler = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions
            {
                HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(3),    // Faster DNS refresh
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                    MaxConnectionsPerServer = 20  // More concurrent connections
                }
            }
        });

        // Example 3: Using HttpClientHandlerConfiguration.Default explicitly
        var sinchExplicitDefaults = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions
            {
                HttpClientHandlerConfiguration = HttpClientHandlerConfiguration.Default
            }
        });

        // Example 4: With LoggerFactory
        // Useful when you want to add logging to see what the SDK is doing
        /*
        using Microsoft.Extensions.Logging;
        
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        var sinchWithLogging = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions
            {
                LoggerFactory = loggerFactory
            }
        });
        */

        // Example 5: Custom IHttpClientFactory (Advanced)
        // For scenarios where you want full control over HttpClient creation
        // Note: Most console apps should use HttpClientHandlerConfiguration instead
        /*
        var customFactory = new MyCustomHttpClientFactory();
        
        var sinchWithFactory = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions
            {
                HttpClientFactory = customFactory
                // Note: When providing a custom IHttpClientFactory,
                // HttpClientHandlerConfiguration is ignored
            }
        });
        */

        // Example 6: Complete SinchOptions configuration
        var sinchComplete = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions
            {
                // Custom handler configuration for DNS refresh and connection pooling
                HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                    MaxConnectionsPerServer = 10
                },

                // Optional: Add logging
                // LoggerFactory = loggerFactory,

                // Optional: Override API URLs for testing/proxy
                // ApiUrlOverrides = new ApiUrlOverrides
                // {
                //     SmsUrl = "https://custom-sms-endpoint.com",
                //     ConversationUrl = "https://custom-conversation-endpoint.com"
                // }
            }
        });
    }
}
