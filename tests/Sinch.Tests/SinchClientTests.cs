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
            var sinch = new SinchClient("TEST_KEY", "TEST_KEY_SECRET", "TEST_PROJECT_ID");
            sinch.Should().NotBeNull();
            sinch.Numbers.Should().NotBeNull();
        }

        [Fact]
        public void Should_instantiate_sinch_client_with_custom_http_client()
        {
            var httpClient = new HttpClient();
            var sinch = new SinchClient("TEST_KEY", "TEST_KEY_SECRET", "TEST_PROJECT_ID",
                options => { options.HttpClient = httpClient; });
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void ThrowNullKeyId()
        {
            Func<ISinchClient> initAction = () => new SinchClient(null, "secret", "project");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullKeySecret()
        {
            Func<ISinchClient> initAction = () => new SinchClient("secret", null, "project");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullProjectId()
        {
            Func<ISinchClient> initAction = () => new SinchClient("id", "secret", null);
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }
    }
}
