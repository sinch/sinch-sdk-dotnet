using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Xunit;

namespace Sinch.Tests.e2e.Conversation
{
    public class CapabilitiesTests : TestBase
    {
        [Fact]
        public async Task Lookup()
        {
            var response = await SinchClientMockServer.Conversation.Capabilities.Lookup(new LookupCapabilityRequest()
            {
                AppId = "string",
                Recipient = new ContactRecipient()
                {
                    ContactId = "contact_id"
                }
            });
            response.Should().BeEquivalentTo(new LookupCapabilityResponse()
            {
                AppId = "string",
                Recipient = new ContactRecipient()
                {
                    ContactId = "contact_id"
                },
                RequestId = "string"
            });
        }
    }
}
