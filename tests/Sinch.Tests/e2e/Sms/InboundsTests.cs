﻿using System.Threading.Tasks;
using FluentAssertions;
using Sinch.SMS.Inbounds.List;
using Xunit;

namespace Sinch.Tests.e2e.Sms
{
    public class InboundsTests : TestBase
    {
        [Fact]
        public async Task List()
        {
            var response = await SinchClient.Sms.Inbounds.List(new Request
            {
                Page = 0,
            });
            response.PageSize.Should().Be(0);
        }
    }
}
