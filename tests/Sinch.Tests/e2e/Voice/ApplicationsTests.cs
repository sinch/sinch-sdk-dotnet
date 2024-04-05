using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Applications;
using Sinch.Voice.Applications.GetNumbers;
using Sinch.Voice.Applications.QueryNumber;
using Sinch.Voice.Applications.UnassignNumbers;
using Sinch.Voice.Applications.UpdateCallbackUrls;
using Sinch.Voice.Applications.UpdateNumbers;
using Xunit;
using NumberItem = Sinch.Voice.Applications.GetNumbers.NumberItem;

namespace Sinch.Tests.e2e.Voice
{
    public class ApplicationsTests : VoiceTestBase
    {
        [Fact]
        public async Task GetNumbers()
        {
            var response = await VoiceClient.Applications.GetNumbers();
            response.Numbers.Should().BeEquivalentTo(new List<NumberItem>
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
                Numbers = new List<string> { "+123" },
                ApplicationKey = "key1"
            });
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UnAssignNumber()
        {
            var op = () => VoiceClient.Applications.UnassignNumber(new UnassignNumberRequest
            {
                Capability = Capability.Voice,
                Number = "+123",
                ApplicationKey = "key1"
            });
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetCallbackUrls()
        {
            var response = await VoiceClient.Applications.GetCallbackUrls("appkey1");
            response.Should().BeEquivalentTo(new Callbacks
            {
                Url = new CallbackUrls
                {
                    Primary = "http://primarycallback.com",
                    Fallback = "http://fallbackcallback.com"
                }
            });
        }

        [Fact]
        public async Task UpdateCallbackUrls()
        {
            var op = () => VoiceClient.Applications.UpdateCallbackUrls(new UpdateCallbackUrlsRequest
            {
                ApplicationKey = "appkey1",
                Url = new CallbackUrls
                {
                    Primary = "http://primarycallback.com",
                    Fallback = "http://fallbackcallback.com"
                }
            });
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task QueryNumber()
        {
            var response = await VoiceClient.Applications.QueryNumber("+431");
            response.Should().BeEquivalentTo(new QueryNumberResponse
            {
                Method = "numberItem",
                NumberItem = new Sinch.Voice.Applications.QueryNumber.NumberItem
                {
                    CountryId = "SE",
                    NumberType = NumberType.Mobile,
                    NormalizedNumber = "+14151112223333",
                    Restricted = true,
                    Rate = new Rate
                    {
                        Amount = 0.368M,
                        CurrencyId = "USD"
                    }
                }
            });
        }
    }
}
