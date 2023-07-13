using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void LastPage()
        {
            Utils.IsLastPage(0, 10, 2).Should().BeTrue();
            Utils.IsLastPage(0, 10, 9).Should().BeTrue();
            Utils.IsLastPage(4, 1, 5).Should().BeTrue();
        }

        [Fact]
        public void NotLastPage()
        {
            Utils.IsLastPage(0, 10, 12).Should().BeFalse();
            Utils.IsLastPage(0, 10, 11).Should().BeFalse();
            Utils.IsLastPage(4, 1, 6).Should().BeFalse();
        }
    }
}
