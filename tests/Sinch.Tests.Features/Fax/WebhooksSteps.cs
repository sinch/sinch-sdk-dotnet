using System.Threading.Tasks;
using Reqnroll;

namespace Sinch.Tests.Features.Fax;

[Binding]
public class WebhooksSteps
{
    // TODO: Implement Fax Webhooks test steps

    [Given(@"the Fax Webhooks handler is available")]
    public void GivenTheFaxWebhooksHandlerIsAvailable()
    {
        // TODO: Initialize webhooks handler
    }

    [When(@"I send a request to trigger the INCOMING_FAX event with the ""(.*)"" content type")]
    public async Task WhenISendARequestToTriggerTheIncomingFaxEventWithTheContentType(string contentType)
    {
        // TODO: Implement incoming fax event trigger
    }

    [Then(@"the event describes an INCOMING_FAX event with the (.*) content type")]
    public void ThenTheEventDescribesAnIncomingFaxEventWithTheContentType(string contentType)
    {
        // TODO: Verify incoming fax event
    }
}
