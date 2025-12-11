using System;
using System.Net.Http;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sinch.Tests.Configuration
{
    public class SinchClientHttpClientAccessorTests
    {
        [Fact]
        public void SinchClient_WithoutHttpClientFactory_ShouldUseDefaultFactory()
        {
            // Arrange & Act
            var sinch = new SinchClient(new SinchClientConfiguration { });

            // Assert
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
            accessor().Should().NotBeNull();
        }

        [Fact]
        public void SinchClient_WithCustomHttpClientFactory_ShouldUseProvidedFactory()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            var mockHttpClient = new HttpClient();
            mockFactory.CreateClient(Arg.Any<string>()).Returns(mockHttpClient);

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = mockFactory
                }
            });

            // Assert
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
            var httpClient = accessor();
            httpClient.Should().BeSameAs(mockHttpClient);
            mockFactory.Received(1).CreateClient("SinchClient");
        }

        [Fact]
        public void SinchClient_HttpClientAccessor_ShouldReturnNewClientOnEachCall()
        {
            // Arrange
            var callCount = 0;
            var mockFactory = Substitute.For<IHttpClientFactory>();
            mockFactory.CreateClient(Arg.Any<string>()).Returns(_ =>
            {
                callCount++;
                return new HttpClient();
            });

            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = mockFactory
                }
            });

            // Act
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            var client1 = accessor();
            var client2 = accessor();
            var client3 = accessor();

            // Assert
            callCount.Should().Be(3);
            mockFactory.Received(3).CreateClient("SinchClient");
        }

        [Fact]
        public void SinchClient_DefaultFactory_ShouldReturnSameClientInstance()
        {
            // Arrange
            var sinch = new SinchClient(new SinchClientConfiguration { });

            // Act
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            var client1 = accessor();
            var client2 = accessor();

            // Assert
            client1.Should().BeSameAs(client2); // DefaultHttpClientFactory returns same instance
        }

        [Fact]
        public void SinchClient_HttpClientAccessor_ShouldUseCorrectClientName()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            mockFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = mockFactory
                }
            });

            // Act
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor();

            // Assert
            mockFactory.Received(1).CreateClient("SinchClient");
        }

        [Fact]
        public void SinchClient_WithNullHttpClientFactory_ShouldUseDefaultFactory()
        {
            // Arrange & Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = null
                }
            });

            // Assert
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
            accessor().Should().NotBeNull();
        }

        [Fact]
        public void SinchClient_HttpClientFactory_ShouldTakePrecedenceOverHandlerConfiguration()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            mockFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            // Act
            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = mockFactory,
                    HttpClientHandlerConfiguration = new HttpClientHandlerConfiguration
                    {
                        MaxConnectionsPerServer = 99
                    }
                }
            });

            // Assert
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor();
            mockFactory.Received(1).CreateClient("SinchClient");
        }

        [Fact]
        public void SinchClient_MultipleInstances_ShouldHaveIndependentAccessors()
        {
            // Arrange & Act
            var sinch1 = new SinchClient(new SinchClientConfiguration { });
            var sinch2 = new SinchClient(new SinchClientConfiguration { });

            // Assert
            var accessor1 = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch1, "_httpClientAccessor");
            var accessor2 = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch2, "_httpClientAccessor");

            accessor1.Should().NotBeSameAs(accessor2);
            accessor1().Should().NotBeSameAs(accessor2());
        }

        [Fact]
        public void SinchClient_HttpClientAccessor_ShouldBeThreadSafe()
        {
            // Arrange
            var mockFactory = Substitute.For<IHttpClientFactory>();
            mockFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            var sinch = new SinchClient(new SinchClientConfiguration
            {
                SinchOptions = new SinchOptions
                {
                    HttpClientFactory = mockFactory
                }
            });

            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");

            // Act - Simulate concurrent access
            System.Threading.Tasks.Parallel.For(0, 10, _ =>
            {
                var client = accessor();
                client.Should().NotBeNull();
            });

            // Assert
            mockFactory.Received(10).CreateClient("SinchClient");
        }
    }
}
