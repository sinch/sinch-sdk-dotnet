using System;
using System.Net.Http;
using FluentAssertions;
using Xunit;

namespace Sinch.Tests
{
    public class SinchClientTests
    {
        [Fact]
        public void Should_instantiate_sinch_client_with_provided_required_params()
        {
            var sinch = new SinchClient("TEST_PROJECT_ID", "TEST_KEY", "TEST_KEY_SECRET");
            sinch.Should().NotBeNull();
            sinch.Numbers.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithCustomHttpClient()
        {
            var httpClient = new HttpClient();
            var sinch = new SinchClient("TEST_PROJECT_ID", "TEST_KEY", "TEST_KEY_SECRET",
                options => { options.HttpClient = httpClient; });
            sinch.Should().NotBeNull();
            Helpers.GetPrivateField<HttpClient, SinchClient>(sinch, "_httpClient").Should().Be(httpClient);
        }

        [Fact]
        public void ThrowNullKeyId()
        {
            Func<ISinchClient> initAction = () => new SinchClient("project", null, "secret");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullKeySecret()
        {
            Func<ISinchClient> initAction = () => new SinchClient("project", "secret", null);
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullProjectId()
        {
            Func<ISinchClient> initAction = () => new SinchClient(null, "id", "secret");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void InitializeOwnHttpIfNotPassed()
        {
            var sinch = new SinchClient("proj", "id", "secret");
            Helpers.GetPrivateField<HttpClient, SinchClient>(sinch, "_httpClient").Should().NotBeNull();
        }
    }
}
