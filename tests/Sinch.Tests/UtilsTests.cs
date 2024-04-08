using System;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.Core;
using Sinch.Numbers;
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


        class Root
        {
            public DateTime Date { get; set; }

            public string DescLong { get; set; }

            public Types Type { get; set; }

            public List<Types> Types { get; set; }

            public string Null { get; set; }

            public DateTime? NullDate { get; set; }
        }

        [Fact]
        public void QueryString()
        {
            var root = new Root()
            {
                Type = Types.Local,
                Date = new DateTime(2022, 7, 12),
                DescLong = "descri",
                Types = new List<Types>()
                {
                    Types.Local, Types.Mobile
                }
            };
            var str = Utils.ToSnakeCaseQueryString(root);
            str.Should().Be("date=2022-07-12T00%3A00%3A00.0000000&desc_long=descri&type=LOCAL&types=LOCAL&types=MOBILE");
        }
    }
}
