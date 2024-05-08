using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests
{
    public class StringUtilsTests
    {
        [Theory]
        [InlineData("Name", "name")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("DataBroker", "dataBroker")]
        [InlineData("HelloWorldAgain", "helloWorldAgain")]
        [InlineData("test", "test")]
        public void ToCamelCase(string input, string output)
        {
            StringUtils.PascalToCamelCase(input).Should().BeEquivalentTo(output);
        }
    }
}
