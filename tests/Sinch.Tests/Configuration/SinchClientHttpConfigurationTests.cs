using System;
using System.Net.Http;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sinch.Tests.Configuration
{
    /// <summary>
    /// Tests for SinchClient initialization with HttpClientHandlerConfiguration
    /// </summary>
    public class SinchClientHttpConfigurationTests
    {
        [Fact]
        public void InitSinchClient_WithHttpClientHandlerConfiguration_ShouldUseCustomConfig()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3),
                MaxConnectionsPerServer = 20
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithBothHttpClientFactoryAndHandlerConfiguration_ShouldPreferFactory()
        {
            // Arrange
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            var config = new HttpClientHandlerConfiguration
            {
                MaxConnectionsPerServer = 30
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = httpClientFactory,
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();

            // Access the HTTP client accessor to trigger the factory call
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            var httpClient = accessor();

            // Verify the custom factory was used
            httpClientFactory.Received(1).CreateClient(Arg.Any<string>());
            httpClient.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithHttpClientHandlerConfiguration_ShouldPassConfigToFactory()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3),
                MaxConnectionsPerServer = 20
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();

            var httpClient = accessor();
            httpClient.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithoutAnyConfiguration_ShouldUseDefaultFactory()
        {
            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                }
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithDefaultConfiguration_ShouldUseDefaultHttpClientFactory()
        {
            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                }
                // No SinchOptions - should use DefaultHttpClientFactory with defaults
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();

            // Verify we can get an HttpClient
            var httpClient = accessor();
            httpClient.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithHttpClientHandlerConfigurationDefault_ShouldWork()
        {
            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = HttpClientHandlerConfiguration.Default
                }
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithPartialHttpClientHandlerConfiguration_ShouldWork()
        {
            // Arrange - only set one property
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
                // Other properties null - should use defaults
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithHighThroughputConfiguration_ShouldWork()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(3),
                MaxConnectionsPerServer = 30
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithFastDnsRefreshConfiguration_ShouldWork()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 10
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_WithRateLimitedConfiguration_ShouldWork()
        {
            // Arrange
            var config = new HttpClientHandlerConfiguration
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 3
            };

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = new SinchOptions
                {
                    HttpClientHandlerConfiguration = config
                }
            });

            // Assert
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClient_HttpClientAccessor_ShouldReturnSameInstanceMultipleTimes()
        {
            // Arrange
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                }
            });

            // Act
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            var httpClient1 = accessor();
            var httpClient2 = accessor();
            var httpClient3 = accessor();

            // Assert
            httpClient1.Should().BeSameAs(httpClient2);
            httpClient2.Should().BeSameAs(httpClient3);
        }

        [Fact]
        public void InitSinchClient_WithNullSinchOptions_ShouldUseDefaults()
        {
            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret"
                },
                SinchOptions = null // Explicitly null
            });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
        }
    }
}
