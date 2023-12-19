using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice.Applications.GetNumbers;
using Sinch.Voice.Applications.UnassignNumbers;
using Sinch.Voice.Applications.UpdateNumbers;
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

        [Fact]
        public async Task AssignNumbers()
        {
            var op = () => VoiceClient.Applications.AssignNumbers(new AssignNumbersRequest
            {
                Capability = Capability.Voice,
                Numbers = new List<string>() { "+123" },
                ApplicationKey = "key1"
            });
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UnAssignNumbers()
        {
            var op = () => VoiceClient.Applications.UnassignNumbers(new UnassignNumberRequest()
            {
                Capability = Capability.Voice,
                Number = "+123",
                ApplicationKey = "key1"
            });
            await op.Should().NotThrowAsync();
        }
    }
}
