using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sinch.Numbers.SinchEvents;
using Sinch.SMS.SinchEvents;
using Sinch.Fax.SinchEvents;
using Sinch.Verification.SinchEvents;
using Sinch.Voice.SinchEvents;

namespace Sinch
{
    /// <summary>
    /// Extension methods for configuring Sinch services with dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="ISinchClient"/> to the service collection with default configuration.
        /// <para>
        /// Use this overload when you only need SDK features that do not require outbound API
        /// credentials, such as parsing or validating incoming Sinch Events in ASP.NET applications.
        /// </para>
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureClient">Optional action to configure the HttpClient.</param>
        /// <returns>The IHttpClientBuilder for further HttpClient configuration (e.g., Polly policies).</returns>
        /// <example>
        /// <code>
        /// builder.Services.AddSinchClient();
        /// </code>
        /// </example>
        public static IHttpClientBuilder AddSinchClient(
            this IServiceCollection services,
            Action<HttpClient>? configureClient = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            return services.AddSinchClient(() => new SinchClientConfiguration(), configureClient);
        }

        /// <summary>
        /// Adds <see cref="ISinchClient"/> to the service collection with proper HttpClient management.
        /// <para>
        /// This method configures IHttpClientFactory for proper connection pooling and DNS refresh,
        /// following Microsoft's best practices for HttpClient usage.
        /// See: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
        /// </para>
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureFactory">Factory that returns the Sinch client configuration used for authenticated API access and other custom settings.</param>
        /// <param name="configureClient">Optional action to configure the HttpClient.</param>
        /// <returns>The IHttpClientBuilder for further HttpClient configuration (e.g., Polly policies).</returns>
        /// <example>
        /// <code>
        /// builder.Services.AddSinchClient(() => new SinchClientConfiguration
        /// {
        ///     // Configure your Sinch client here
        /// })
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

        /// <summary>
        /// Registers Sinch Events handler interfaces into the service collection so they can be
        /// injected directly where needed.
        /// <para>
        /// Requires one of the <see cref="ServiceCollectionExtensions.AddSinchClient(IServiceCollection, Action{HttpClient}?)"/>
        /// or <see cref="ServiceCollectionExtensions.AddSinchClient(IServiceCollection, Func{SinchClientConfiguration}, Action{HttpClient}?)"/>
        /// overloads to have been called first.
        /// </para>
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> for the registered event handler services.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <example>
        /// <code>
        /// builder.Services.AddSinchClient(() => config);
        /// builder.Services.AddSinchEventsHandlers(ServiceLifetime.Scoped);
        /// </code>
        /// </example>
        public static IServiceCollection AddSinchEventsHandlers(
            this IServiceCollection services,
            ServiceLifetime lifetime)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAdd(new ServiceDescriptor(
                typeof(INumbersSinchEvents),
                sp => sp.GetRequiredService<ISinchClient>().Numbers.SinchEvents,
                lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(ISmsSinchEvents),
                sp => sp.GetRequiredService<ISinchClient>().Sms.SinchEvents,
                lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(IFaxSinchEvents),
                sp => sp.GetRequiredService<ISinchClient>().Fax.SinchEvents,
                lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(IVerificationSinchEvents),
                sp => sp.GetRequiredService<ISinchClient>().Verification.SinchEvents,
                lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(IVoiceSinchEvents),
                sp => sp.GetRequiredService<ISinchClient>().Voice.SinchEvents,
                lifetime));

            return services;
        }
    }
}
