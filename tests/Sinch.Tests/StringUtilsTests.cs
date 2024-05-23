using System;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests
{
    public class StringUtilsTests
    {
        [Theory]
        [InlineData("Name", "name")]
        [InlineData("DataBroker", "dataBroker")]
        [InlineData("HelloWorldAgain", "helloWorldAgain")]
        [InlineData("test", "test")]
        public void ToCamelCase(string input, string output)
        {
            StringUtils.PascalToCamelCase(input).Should().BeEquivalentTo(output);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ThrowCamelCase(string input)
        {
            var op = () => StringUtils.PascalToCamelCase(input);
            op.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Iso8160NoTicks()
        {
            StringUtils.ToIso8601NoTicks(new DateTime(2024, 05, 21, 12, 23, 11)).Should()
                .BeEquivalentTo("2024-05-21T12:23:11Z");
        }

        [Fact]
        public void Iso8160Ticks()
        {
            StringUtils.ToIso8601(new DateTime(2024, 05, 21, 12, 23, 11, 586)).Should()
                .BeEquivalentTo("2024-05-21T12:23:11.5860000");
        }
    }
}
