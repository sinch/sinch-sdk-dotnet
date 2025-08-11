using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sinch.Numbers;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class NumbersConfigurationTests
    {
        public record NumbersUrlTestCase(
            string TestName,
            string UrlOverride,
            string ExpectedUrl)
        {
            private static readonly NumbersUrlTestCase[] TestCases =
            {
                new("Default Numbers URL", null, "https://numbers.api.sinch.com/"),
                new("Custom override", "https://hello.world", "https://hello.world/")
            };

            public static IEnumerable<object[]> TestCasesData =>
                TestCases.Select(testCase => new object[] { testCase });

            public override string ToString() => TestName;
        }

        [Theory]
        [MemberData(nameof(NumbersUrlTestCase.TestCasesData), MemberType = typeof(NumbersUrlTestCase))]
        public void ResolveNumbersUrl(NumbersUrlTestCase testCase)
        {
            var numbersConfig = new SinchNumbersConfiguration()
            {
                UrlOverride = testCase.UrlOverride,
            };
            numbersConfig.ResolveUrl().ToString().Should().BeEquivalentTo(testCase.ExpectedUrl);
        }
    }
}
