using Microsoft.Extensions.Configuration;

namespace Sinch.Snippets.Shared;

/// <summary>
/// Provides centralized configuration loading for Sinch SDK snippets.
/// Configuration is loaded from appsettings.json (copied to output directory),
/// with environment variables taking precedence.
/// </summary>
public static class ConfigurationHelper
{
    private static readonly Lazy<IConfiguration> _configuration = new(BuildConfiguration);

    /// <summary>
    /// Gets the configuration instance.
    /// </summary>
    public static IConfiguration Configuration => _configuration.Value;

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    /// Gets a configuration value by key. Returns null if not found.
    /// </summary>
    /// <param name="key">The configuration key (e.g., "SINCH_PROJECT_ID").</param>
    /// <returns>The configuration value or null.</returns>
    public static string? GetValue(string key) => Configuration[key];

    /// <summary>
    /// Gets the Sinch Project ID from configuration.
    /// </summary>
    public static string? GetProjectId() => GetValue("SINCH_PROJECT_ID");

    /// <summary>
    /// Gets the Sinch Key ID from configuration.
    /// </summary>
    public static string? GetKeyId() => GetValue("SINCH_KEY_ID");

    /// <summary>
    /// Gets the Sinch Key Secret from configuration.
    /// </summary>
    public static string? GetKeySecret() => GetValue("SINCH_KEY_SECRET");

    /// <summary>
    /// Gets the Sinch phone number from configuration.
    /// </summary>
    public static string? GetPhoneNumber() => GetValue("SINCH_PHONE_NUMBER");

    /// <summary>
    /// Gets the Sinch Service Plan ID from configuration.
    /// </summary>
    public static string? GetServicePlanId() => GetValue("SINCH_SERVICE_PLAN_ID");
}
