using System;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Configuration
{
    public class HttpClientHandlerConfigurationTests
    {
        [Fact]
        public void Default_ShouldReturnConfigurationWithDefaultValues()
        {
            // Act
            var config = HttpClientHandlerConfiguration.Default;

            // Assert
            config.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(5));
            config.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(2));
            config.MaxConnectionsPerServer.Should().Be(10);
        }

        [Fact]
        public void Default_ShouldUseDefaultHttpClientFactoryConstants()
        {
            // Act
            var config = HttpClientHandlerConfiguration.Default;

            // Assert
            config.PooledConnectionLifetime.Should().Be(DefaultHttpClientFactory.DefaultPooledConnectionLifetime);
            config.PooledConnectionIdleTimeout.Should().Be(DefaultHttpClientFactory.DefaultPooledConnectionIdleTimeout);
            config.MaxConnectionsPerServer.Should().Be(DefaultHttpClientFactory.DefaultMaxConnectionsPerServer);
        }

        [Fact]
        public void CustomConfiguration_ShouldAllowPartialOverrides()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                MaxConnectionsPerServer = 20
                // Other properties remain null
            };

            // Assert
            config.MaxConnectionsPerServer.Should().Be(20);
            config.PooledConnectionLifetime.Should().BeNull();
            config.PooledConnectionIdleTimeout.Should().BeNull();
        }

        [Fact]
        public void CustomConfiguration_ShouldAllowFullCustomization()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 30
            };

            // Assert
            config.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(3));
            config.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(1));
            config.MaxConnectionsPerServer.Should().Be(30);
        }

        [Fact]
        public void CustomConfiguration_WithZeroMinutes_ShouldBeAllowed()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.Zero,
                PooledConnectionIdleTimeout = TimeSpan.Zero,
                MaxConnectionsPerServer = 1
            };

            // Assert
            config.PooledConnectionLifetime.Should().Be(TimeSpan.Zero);
            config.PooledConnectionIdleTimeout.Should().Be(TimeSpan.Zero);
            config.MaxConnectionsPerServer.Should().Be(1);
        }

        [Fact]
        public void CustomConfiguration_WithNegativeMaxConnections_ShouldBeAllowed()
        {
            // Arrange - Note: SocketsHttpHandler treats negative as unlimited
            var config = new HttpClientHandlerConfiguration
            {
                MaxConnectionsPerServer = -1
            };

            // Assert
            config.MaxConnectionsPerServer.Should().Be(-1);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public void CustomConfiguration_WithVariousMinutes_ShouldWork(int minutes)
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(minutes),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(minutes)
            };

            // Assert
            config.PooledConnectionLifetime.Should().Be(TimeSpan.FromMinutes(minutes));
            config.PooledConnectionIdleTimeout.Should().Be(TimeSpan.FromMinutes(minutes));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(50)]
        public void CustomConfiguration_WithVariousMaxConnections_ShouldWork(int maxConnections)
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                MaxConnectionsPerServer = maxConnections
            };

            // Assert
            config.MaxConnectionsPerServer.Should().Be(maxConnections);
        }

        [Fact]
        public void Default_ShouldCreateNewInstanceEachTime()
        {
            // Act
            var config1 = HttpClientHandlerConfiguration.Default;
            var config2 = HttpClientHandlerConfiguration.Default;

            // Assert
            config1.Should().NotBeSameAs(config2); // Different instances
            config1.Should().BeEquivalentTo(config2); // But same values
        }
    }
}
