using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Sinch.Tests.Configuration
{
    public class SinchOptionsHttpClientTests
    {
        [Fact]
        public void SinchOptions_HttpClientFactory_ShouldBeSettable()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();

            // Act
            var options = new SinchOptions
            {
                HttpClientFactory = mockFactory
            };

            // Assert
            options.HttpClientFactory.Should().BeSameAs(mockFactory);
        }

        [Fact]
        public void SinchOptions_HttpClientHandlerConfiguration_ShouldBeSettable()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3),
                MaxConnectionsPerServer = 20
            };

            // Act
            var options = new SinchOptions
            {
                HttpClientHandlerConfiguration = config
            };

            // Assert
            options.HttpClientHandlerConfiguration.Should().BeSameAs(config);
        }

        [Fact]
        public void SinchOptions_WithBothFactoryAndConfiguration_ShouldAllowBoth()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            var config = new HttpClientHandlerConfiguration
            {
                MaxConnectionsPerServer = 15
            };

            // Act
            var options = new SinchOptions
            {
                HttpClientFactory = mockFactory,
                HttpClientHandlerConfiguration = config
            };

            // Assert
            options.HttpClientFactory.Should().BeSameAs(mockFactory);
            options.HttpClientHandlerConfiguration.Should().BeSameAs(config);
        }

        [Fact]
        public void SinchOptions_WithAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            var mockLogger = Substitute.For<ILoggerFactory>();
            var config = new HttpClientHandlerConfiguration();

            // Act
            var options = new SinchOptions
            {
                HttpClientFactory = mockFactory,
                LoggerFactory = mockLogger,
                HttpClientHandlerConfiguration = config,
                ApiUrlOverrides = new ApiUrlOverrides
                {
                    SmsUrl = "https://custom-sms.example.com"
                }
            };

            // Assert
            options.HttpClientFactory.Should().BeSameAs(mockFactory);
            options.LoggerFactory.Should().BeSameAs(mockLogger);
            options.HttpClientHandlerConfiguration.Should().BeSameAs(config);
            options.ApiUrlOverrides.Should().NotBeNull();
            options.ApiUrlOverrides!.SmsUrl.Should().Be("https://custom-sms.example.com");
        }

        [Fact]
        public void SinchOptions_Default_ShouldHaveNullValues()
        {
            // Act
            var options = new SinchOptions();

            // Assert
            options.HttpClientFactory.Should().BeNull();
            options.LoggerFactory.Should().BeNull();
            options.HttpClientHandlerConfiguration.Should().BeNull();
            options.ApiUrlOverrides.Should().BeNull();
        }

        [Fact]
        public void SinchOptions_Multiple_ShouldBeIndependent()
        {
            // Arrange
            var factory1 = Substitute.For<IHttpClientFactory>();
            var factory2 = Substitute.For<IHttpClientFactory>();

            // Act
            var options1 = new SinchOptions { HttpClientFactory = factory1 };
            var options2 = new SinchOptions { HttpClientFactory = factory2 };

            // Assert
            options1.HttpClientFactory.Should().BeSameAs(factory1);
            options2.HttpClientFactory.Should().BeSameAs(factory2);
            options1.HttpClientFactory.Should().NotBeSameAs(options2.HttpClientFactory);
        }
    }
}
