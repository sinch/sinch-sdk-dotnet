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
    }
}
