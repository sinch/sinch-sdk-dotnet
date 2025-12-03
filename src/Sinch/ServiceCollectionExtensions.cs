using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sinch
{
    /// <summary>
    /// Extension methods for configuring Sinch services with dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="ISinchClient"/> to the service collection with proper HttpClient management.
        /// <para>
        /// This method configures IHttpClientFactory for proper connection pooling and DNS refresh,
        /// following Microsoft's best practices for HttpClient usage.
        /// See: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
        /// </para>
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureFactory"></param>
        /// <param name="configureClient">Optional action to configure the HttpClient.</param>
        /// <returns>The IHttpClientBuilder for further HttpClient configuration (e.g., Polly policies).</returns>
        /// <example>
        /// <code>
        /// builder.Services.AddSinchClient((projectId, keyId, keySecret, verificationConfig, voiceConfig) =>
        /// {
        ///     projectId = builder.Configuration["Sinch:ProjectId"]!;
        ///     keyId = builder.Configuration["Sinch:KeyId"]!;
        ///     keySecret = builder.Configuration["Sinch:KeySecret"]!;
        ///     // Optional configurations
        ///     verificationConfig = new SinchVerificationConfiguration { ... };
        ///     voiceConfig = new SinchVoiceConfiguration { ... };
        /// })
        /// .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        /// </code>
        /// </example>
        public static IHttpClientBuilder AddSinchClient(
            this IServiceCollection services,
            Func<SinchClientConfiguration> configureFactory,
            Action<HttpClient>? configureClient = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureFactory);

            services.AddSingleton<ISinchClient>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = sp.GetService<ILoggerFactory>();

                var configurationFactory = configureFactory();

                // If SinchOptions is null, create with factory and logger
                // If SinchOptions exists, merge factory and logger if not already set
                var sinchOptions = configurationFactory.SinchOptions ?? new SinchOptions
                {
                    HttpClientFactory = httpClientFactory,
                    LoggerFactory = loggerFactory
                };

                // If user provided SinchOptions but didn't set factory/logger, we need to create a new instance
                if (configurationFactory.SinchOptions != null)
                {
                    sinchOptions = new SinchOptions
                    {
                        HttpClientFactory = configurationFactory.SinchOptions.HttpClientFactory ?? httpClientFactory,
                        LoggerFactory = configurationFactory.SinchOptions.LoggerFactory ?? loggerFactory,
                        FaxRegion = configurationFactory.SinchOptions.FaxRegion,
                        ApiUrlOverrides = configurationFactory.SinchOptions.ApiUrlOverrides
                    };
                }

                // Create new configuration with merged options
                var finalConfiguration = new SinchClientConfiguration
                {
                    SinchUnifiedCredentials = configurationFactory.SinchUnifiedCredentials,
                    SinchOptions = sinchOptions,
                    NumbersConfiguration = configurationFactory.NumbersConfiguration,
                    SinchOAuthConfiguration = configurationFactory.SinchOAuthConfiguration,
                    SmsConfiguration = configurationFactory.SmsConfiguration,
                    ConversationConfiguration = configurationFactory.ConversationConfiguration,
                    FaxConfiguration = configurationFactory.FaxConfiguration,
                    VerificationConfiguration = configurationFactory.VerificationConfiguration,
                    VoiceConfiguration = configurationFactory.VoiceConfiguration
                };

                return new SinchClient(finalConfiguration);
            });

            var builder = services.AddHttpClient("SinchClient", client =>
            {
                configureClient?.Invoke(client);
            });

            return builder;
        }
    }
}
