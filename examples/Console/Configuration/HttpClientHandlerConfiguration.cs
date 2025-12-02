using DotNetEnv;
using Sinch;
using Sinch.Numbers;

namespace Examples.Configuration;

/// <summary>
/// Demonstrates how to use HttpClientHandlerConfiguration to customize
/// DefaultHttpClientFactory behavior for console applications.
/// </summary>
public class HttpClientHandlerConfiguration
{
    public async Task Run()
    {
        // Load environment variables from .env file
        Env.Load();

        Console.WriteLine("=== HttpClientHandlerConfiguration Examples ===\n");

        await DefaultConfiguration();
        await FastDnsRefresh();
        await HighThroughput();
        await RateLimited();
        await UsingConstants();
        await PartialConfiguration();
        await UsingDefaultProperty();

        Console.WriteLine("\n=== All Examples Completed ===\n");
        PrintKeyTakeaways();
    }

    /// <summary>
    /// Default Configuration - Shows that you don't need to configure anything
    /// </summary>
    private async Task DefaultConfiguration()
    {
        Console.WriteLine("1. Default Configuration (No customization needed)");
        Console.WriteLine("   DNS refresh: Every 5 minutes (default)");
        Console.WriteLine("   Idle timeout: 2 minutes (default)");
        Console.WriteLine("   Max connections: 10 per server (default)\n");

        var sinch = new SinchClient(new SinchClientConfiguration
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            }
            // No SinchOptions needed - uses DefaultHttpClientFactory with default values!
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Default config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// Faster DNS Refresh - For multi-region deployments where DNS changes frequently
    /// </summary>
    private async Task FastDnsRefresh()
    {
        Console.WriteLine("2. Custom Configuration: Faster DNS Refresh");
        Console.WriteLine("   DNS refresh: Every 2 minutes (faster)");
        Console.WriteLine("   Idle timeout: 1 minute");
        Console.WriteLine("   Max connections: 10 per server\n");

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
                HttpClientHandlerConfiguration = new Sinch.HttpClientHandlerConfiguration
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(2),    // Faster DNS refresh
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                    MaxConnectionsPerServer = 10
                }
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Fast DNS config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// High-Throughput Configuration - For applications that need to send many concurrent requests
    /// </summary>
    private async Task HighThroughput()
    {
        Console.WriteLine("3. High-Throughput Configuration");
        Console.WriteLine("   DNS refresh: Every 5 minutes (standard)");
        Console.WriteLine("   Idle timeout: 3 minutes (keep connections warm)");
        Console.WriteLine("   Max connections: 30 per server (high concurrency)\n");

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
                HttpClientHandlerConfiguration = new Sinch.HttpClientHandlerConfiguration
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(3),  // Keep connections warm
                    MaxConnectionsPerServer = 30  // High concurrency for bulk operations
                }
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ High-throughput config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// Rate-Limited Configuration - For staying under API rate limits
    /// </summary>
    private async Task RateLimited()
    {
        Console.WriteLine("4. Rate-Limited Configuration");
        Console.WriteLine("   DNS refresh: Every 10 minutes (reduce overhead)");
        Console.WriteLine("   Idle timeout: 5 minutes");
        Console.WriteLine("   Max connections: 3 per server (stay under rate limits)\n");

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
                HttpClientHandlerConfiguration = new Sinch.HttpClientHandlerConfiguration
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(10),  // Less frequent rotation
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                    MaxConnectionsPerServer = 3  // Avoid rate limits
                }
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Rate-limited config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// Using Default Constants - Shows how to reference the default values via HttpClientHandlerConfiguration.Default
    /// </summary>
    private async Task UsingConstants()
    {
        Console.WriteLine("5. Using HttpClientHandlerConfiguration.Default");
        Console.WriteLine("   Shows how to reference the default values programmatically\n");

        // Display the default configuration values
        var defaultConfig = Sinch.HttpClientHandlerConfiguration.Default;
        Console.WriteLine($"   Default PooledConnectionLifetime: {defaultConfig.PooledConnectionLifetime}");
        Console.WriteLine($"   Default PooledConnectionIdleTimeout: {defaultConfig.PooledConnectionIdleTimeout}");
        Console.WriteLine($"   Default MaxConnectionsPerServer: {defaultConfig.MaxConnectionsPerServer}\n");

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
                // Using the Default static property
                HttpClientHandlerConfiguration = Sinch.HttpClientHandlerConfiguration.Default
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Default property works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// Partial Configuration - Only override what you need
    /// </summary>
    private async Task PartialConfiguration()
    {
        Console.WriteLine("6. Partial Configuration");
        Console.WriteLine("   Only override MaxConnectionsPerServer, use defaults for rest\n");

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
                HttpClientHandlerConfiguration = new Sinch.HttpClientHandlerConfiguration
                {
                    // Only override what you need - others use defaults
                    MaxConnectionsPerServer = 20
                    // PooledConnectionLifetime = null → uses default (5 min)
                    // PooledConnectionIdleTimeout = null → uses default (2 min)
                }
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Partial config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    /// <summary>
    /// Using HttpClientHandlerConfiguration.Default - Demonstrates the static Default property
    /// </summary>
    private async Task UsingDefaultProperty()
    {
        Console.WriteLine("7. Comparing Custom vs Default Configuration");
        Console.WriteLine("   Shows the difference between custom and default values\n");

        // Show custom configuration
        var customConfig = new Sinch.HttpClientHandlerConfiguration
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(3),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
            MaxConnectionsPerServer = 15
        };

        Console.WriteLine("   Custom Configuration:");
        Console.WriteLine($"   - PooledConnectionLifetime: {customConfig.PooledConnectionLifetime}");
        Console.WriteLine($"   - PooledConnectionIdleTimeout: {customConfig.PooledConnectionIdleTimeout}");
        Console.WriteLine($"   - MaxConnectionsPerServer: {customConfig.MaxConnectionsPerServer}\n");

        // Show default configuration
        var defaultConfig = Sinch.HttpClientHandlerConfiguration.Default;
        Console.WriteLine("   Default Configuration:");
        Console.WriteLine($"   - PooledConnectionLifetime: {defaultConfig.PooledConnectionLifetime}");
        Console.WriteLine($"   - PooledConnectionIdleTimeout: {defaultConfig.PooledConnectionIdleTimeout}");
        Console.WriteLine($"   - MaxConnectionsPerServer: {defaultConfig.MaxConnectionsPerServer}\n");

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
                HttpClientHandlerConfiguration = customConfig
            }
        });

        try
        {
            var regions = await sinch.Numbers.Regions.List(new List<Types>());
            Console.WriteLine($"✅ Custom config works! Found {regions.Count()} regions\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n");
        }
    }

    private void PrintKeyTakeaways()
    {
        Console.WriteLine("Key Takeaways:");
        Console.WriteLine("1. Default configuration works great for most scenarios - no customization needed!");
        Console.WriteLine("2. Customize only when you have specific requirements (DNS timing, throughput, rate limits)");
        Console.WriteLine("3. You can override just the properties you need - others use defaults");
        Console.WriteLine("4. Use HttpClientHandlerConfiguration.Default to get the default values");
        Console.WriteLine("5. Partial configuration is supported - only specify what you want to change");
        Console.WriteLine("\nFor more information, see: docs/Configuring-HttpClient-Handler.md");
    }
}
