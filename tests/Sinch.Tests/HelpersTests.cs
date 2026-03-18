using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Sinch.Tests
{
    public class HelpersTests
    {
        [Fact]
        public void AssertJsonEqual_ShouldPass_WhenTypesMatch()
        {
            const string expected = """{"amount": 100}""";
            const string actual = """{"amount": 100}""";

            var act = () => Helpers.AssertJsonEqual(expected, actual);

            act.Should().NotThrow();
        }

        [Fact]
        public void AssertJsonEqual_ShouldFail_WhenNumericAndStringValuesAreCompared()
        {
            const string expected = """{"amount": 100}""";
            const string actual = """{"amount": "100"}""";

            var act = () => Helpers.AssertJsonEqual(expected, actual);

            act.Should().Throw<XunitException>();
        }
    }
}
