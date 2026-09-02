using System.Threading.Tasks;
using Reqnroll;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Consents
{
    [Given(@"the Conversation service ""Consents"" is available")]
    public void GivenTheConversationServiceConsentsIsAvailable()
    {
        // TODO
    }

    [When(@"I send a request to list the existing Consent Identities")]
    public async Task WhenISendARequestToListTheExistingConsentIdentities()
    {
        // TODO
    }

    [When(@"I send a request to list all the Consent Identities")]
    public async Task WhenISendARequestToListAllTheConsentIdentities()
    {
        // TODO
    }

    [When(@"I iterate manually over the Consent Identities pages")]
    public async Task WhenIIterateManuallyOverTheConsentIdentitiesPages()
    {
        // TODO
    }

    [When(@"I send a request to list the Audit Records associated with an identity")]
    public async Task WhenISendARequestToListTheAuditRecordsAssociatedWithAnIdentity()
    {
        // TODO
    }

    [Then(@"the response contains ""(.*)"" Consent Identities")]
    public void ThenTheResponseContainsConsentIdentities(string count)
    {
        // TODO
    }

    [Then(@"the Consent Identities list contains ""(.*)"" Consent Identities")]
    public void ThenTheConsentIdentitiesListContainsConsentIdentities(string count)
    {
        // TODO
    }

    [Then(@"the Consent Identities iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheConsentIdentitiesIterationResultContainsTheDataFromPages(string count)
    {
        // TODO
    }

    [Then(@"the response contains list of the Audit Records associated with an identity")]
    public void ThenTheResponseContainsListOfTheAuditRecordsAssociatedWithAnIdentity()
    {
        // TODO
    }
}
