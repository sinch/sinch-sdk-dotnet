using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Configuration
{
    public class DefaultHttpClientFactoryTests
    {
        [Fact]
        public void Constructor_WithoutConfiguration_ShouldUseDefaults()
        {
            // Act
            var factory = new DefaultHttpClientFactory();
            var httpClient = factory.CreateClient("test");

            // Assert
            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpClient>();
        }

        [Fact]
        public void Constructor_WithNullConfiguration_ShouldUseDefaults()
        {
            // Act
            var factory = new DefaultHttpClientFactory(null);
            var httpClient = factory.CreateClient("test");

            // Assert
            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpClient>();
        }

        [Fact]
        public void Constructor_WithDefaultConfiguration_ShouldCreateHttpClient()
        {
            // Act
            var factory = new DefaultHttpClientFactory(HttpClientHandlerConfiguration.Default);
            var httpClient = factory.CreateClient("test");

            // Assert
            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpClient>();
        }

        [Fact]
        public void Constructor_WithCustomConfiguration_ShouldCreateHttpClient()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 20
            };

            // Act
            var factory = new DefaultHttpClientFactory(config);
            var httpClient = factory.CreateClient("test");

            // Assert
            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpClient>();
        }

        [Fact]
        public void Constructor_WithPartialConfiguration_ShouldCreateHttpClient()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };

            // Act
            var factory = new DefaultHttpClientFactory(config);
            var httpClient = factory.CreateClient("test");

            // Assert
            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpClient>();
        }

        [Fact]
        public void CreateClient_ShouldReturnSameInstance()
        {
            // Arrange
            var factory = new DefaultHttpClientFactory();

            // Act
            var client1 = factory.CreateClient("test1");
            var client2 = factory.CreateClient("test2");
            var client3 = factory.CreateClient("different-name");

            // Assert
            client1.Should().BeSameAs(client2); // Should return same instance
            client1.Should().BeSameAs(client3); // Name doesn't matter
        }

        [Fact]
        public void DefaultConstants_ShouldHaveExpectedValues()
        {
            // Assert
            DefaultHttpClientFactory.DefaultPooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(5));
            DefaultHttpClientFactory.DefaultPooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(2));
            DefaultHttpClientFactory.DefaultMaxConnectionsPerServer.Should().Be(10);
        }

        [Fact]
        public async Task Dispose_ShouldDisposeHttpClient()
        {
            // Arrange
            var factory = new DefaultHttpClientFactory();
            var httpClient = factory.CreateClient("test");

            // Act
            factory.Dispose();

            // Assert - HttpClient should be disposed (attempting to use it should throw)
            Func<Task> act = async () => await httpClient.GetAsync("http://localhost");
            await act.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var factory = new DefaultHttpClientFactory();

            // Act & Assert - multiple dispose calls should not throw
            Action act = () =>
            {
                factory.Dispose();
                factory.Dispose();
                factory.Dispose();
            };
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldImplementIDisposable()
        {
            // Assert
            typeof(DefaultHttpClientFactory).Should().Implement<IDisposable>();
        }
    }
}
