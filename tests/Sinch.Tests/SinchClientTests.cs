using System;
using System.Net.Http;
using FluentAssertions;
using Xunit;

namespace Sinch.Tests
{
    public class SinchClient
    {
        [Fact]
        public void Should_instantiate_sinch_client_with_provided_required_params()
        {
            var sinch = new Sinch.SinchClient("TEST_KEY", "TEST_KEY_SECRET", "TEST_PROJECT_ID");
            sinch.Should().NotBeNull();
            sinch.Numbers.Should().NotBeNull();
        }

        [Fact]
        public void Should_instantiate_sinch_client_with_custom_http_client()
        {
            var httpClient = new HttpClient();
            var sinch = new Sinch.SinchClient("TEST_KEY", "TEST_KEY_SECRET", "TEST_PROJECT_ID",
                options => { options.HttpClient = httpClient; });
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void ThrowNullKeyId()
        {
            Func<ISinch> initAction = () => new Sinch.SinchClient(null, "secret", "project");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullKeySecret()
        {
            Func<ISinch> initAction = () => new Sinch.SinchClient("secret", null, "project");
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }

        [Fact]
        public void ThrowNullProjectId()
        {
            Func<ISinch> initAction = () => new Sinch.SinchClient("id", "secret", null);
            initAction.Should().Throw<ArgumentNullException>("Should have a value");
        }
    }
}
