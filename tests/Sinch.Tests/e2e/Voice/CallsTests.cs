using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice.Calls;
using Xunit;

namespace Sinch.Tests.e2e.Voice
{
    public class CallsTests : VoiceTestBase
    {
        [Fact]
        public async Task GetCall()
        {
            var response = await VoiceClient.Calls.Get("123");
            response.Should().BeEquivalentTo(new Call()
            {
                From = "456",
                To = "987",
                Domain = CallDomain.Pstn,
                CallId = "123",
                Duration = 60,
                Status = CallStatus.ONGOING,
                Result = CallResult.Busy,
                Reason = CallResultReason.Cancel,
                Timestamp = DateTime.Parse("2019-08-24T14:15:22Z").ToUniversalTime(),
                Custom = new object(),
                UserRate = "39",
                Debit = "138",
            });
        }
    }
}
