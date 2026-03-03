using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Capability
{
    private ISinchConversationCapabilities _capability;
    private LookupCapabilityResponse _response;

    [Given(@"the Conversation service ""Capability"" is available")]
    public void GivenTheConversationServiceCapabilityIsAvailable()
    {
        _capability = Utils.SinchConversationClient().Capabilities;
    }

    [When(@"I send a request to query a capability lookup")]
    public async Task WhenISendARequestToQueryACapabilityLookup()
    {
        _response = await _capability.Lookup(new LookupCapabilityRequest
        {
            AppId = "01W4FFL35P4NC4K35CONVAPP001",
            Recipient = new ContactRecipient
            {
                ContactId = "01W4FFL35P4NC4K35CONTACT001"
            }
        });
    }

    [Then(@"the response contains the id of the capability lookup query")]
    public void ThenTheResponseContainsTheIdOfTheCapabilityLookupQuery()
    {
        _response.Should().NotBeNull();
        _response.AppId.Should().Be("01W4FFL35P4NC4K35CONVAPP001");
        _response.RequestId.Should().Be("01W4FFL35P4NC4K35CAPABILITY");
        var contactRecipient = _response.Recipient.Should().BeOfType<ContactRecipient>().Subject;
        contactRecipient.ContactId.Should().Be("01W4FFL35P4NC4K35CONTACT001");
    }
}
