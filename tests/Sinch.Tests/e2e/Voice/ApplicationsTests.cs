using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice.Applications.GetNumbers;
using Xunit;

namespace Sinch.Tests.e2e.Voice
{
    public class ApplicationsTests : VoiceTestBase
    {
        [Fact]
        public async Task GetNumbers()
        {
            var response = await VoiceClient.Applications.GetNumbers();
            response.Numbers.Should().BeEquivalentTo(new List<NumberItem>()
            {
                new()
                {
                    Number = "+48128128",
                    Applicationkey = "991",
                    Capability = Capability.Voice
                },
                new()
                {
                    Number = "+8800",
                    Applicationkey = null,
                    Capability = Capability.Sms
                }
            });
        }
    }
}
