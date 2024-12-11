using System;
using FluentAssertions;
using Sinch.Mailgun;
using Xunit;

namespace Sinch.Tests.Mailgun
{
    public class MailgunClientTests
    {
        [Fact]
        public void InitMailgunClient()
        {
            var mailgun = new SinchClient().Mailgun("apikey", MailgunRegion.Eu);

            mailgun.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FailEmptyApiKey(string apiKey)
        {
            var mailgunCreation = () => new SinchClient().Mailgun(apiKey, MailgunRegion.Eu);

            mailgunCreation.Should().Throw<ArgumentNullException>();
        }
    }
}
