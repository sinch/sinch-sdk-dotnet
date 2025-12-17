using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Sinch.Tests.Configuration
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSinchClient_WithNullServices_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection services = null!;

            // Act
            Action act = () => services.AddSinchClient(() => new SinchClientConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("services");
        }

        [Fact]
        public void AddSinchClient_WithNullConfigureFactory_ShouldThrowArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            Action act = () => services.AddSinchClient(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("configureFactory");
        }

        [Fact]
        public void AddSinchClient_WithBasicConfiguration_ShouldRegisterSinchClient()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSinchClient(() => new SinchClientConfiguration { });

            var serviceProvider = services.BuildServiceProvider();
            var sinchClient = serviceProvider.GetService<ISinchClient>();

            // Assert
            sinchClient.Should().NotBeNull();
            sinchClient.Should().BeOfType<SinchClient>();
        }

        [Fact]
        public void AddSinchClient_ShouldRegisterHttpClient()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSinchClient(() => new SinchClientConfiguration { });

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert
            httpClientFactory.Should().NotBeNull();
        }

        [Fact]
        public void AddSinchClient_ShouldUseInjectedLoggerFactory_WhenNotProvidedInOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSinchClient(() => new SinchClientConfiguration { });

            var serviceProvider = services.BuildServiceProvider();
            var sinchClient = serviceProvider.GetService<ISinchClient>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            // Assert
            sinchClient.Should().NotBeNull();
            loggerFactory.Should().NotBeNull();
        }

        [Fact]
        public void AddSinchClient_ShouldReturnHttpClientBuilder()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var builder = services.AddSinchClient(() => new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "test-project",
                    KeyId = "test-key",
                    KeySecret = "test-secret"
                }
            });

            // Assert
            builder.Should().NotBeNull();
            builder.Should().BeAssignableTo<IHttpClientBuilder>();
        }

        [Fact]
        public void AddSinchClient_WithConfigureClient_ShouldApplyConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            var configureClientCalled = false;

            // Act
            services.AddSinchClient(
                () => new SinchClientConfiguration
                {
                },
                client =>
                {
                    configureClientCalled = true;
                    client.Timeout = TimeSpan.FromSeconds(30);
                });

            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("SinchClient");

            // Assert
            configureClientCalled.Should().BeTrue();
            httpClient.Timeout.Should().Be(TimeSpan.FromSeconds(30));
        }

        [Fact]
        public void AddSinchClient_MultipleTimes_ShouldRegisterMultipleInstances()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSinchClient(() => new SinchClientConfiguration { });

            services.AddSinchClient(() => new SinchClientConfiguration { });

            var serviceProvider = services.BuildServiceProvider();
            var sinchClients = serviceProvider.GetServices<ISinchClient>();

            // Assert
            sinchClients.Should().HaveCount(2);
        }

        [Fact]
        public void AddSinchClient_ShouldRegisterAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSinchClient(() => new SinchClientConfiguration { });

            var serviceProvider = services.BuildServiceProvider();
            var sinchClient1 = serviceProvider.GetService<ISinchClient>();
            var sinchClient2 = serviceProvider.GetService<ISinchClient>();

            // Assert
            sinchClient1.Should().BeSameAs(sinchClient2);
        }

        private class TestHttpClientFactory : IHttpClientFactory
        {
            public HttpClient CreateClient(string name)
            {
                return new HttpClient();
            }
        }
    }
}
