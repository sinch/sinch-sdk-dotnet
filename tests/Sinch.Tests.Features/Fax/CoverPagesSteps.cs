using System.Threading.Tasks;
using Reqnroll;

namespace Sinch.Tests.Features.Fax;

[Binding]
public class CoverPagesSteps
{
    // TODO: Implement Fax Cover Pages test steps

    [Given(@"the Fax service ""CoverPages"" is available")]
    public void GivenTheFaxServiceCoverPagesIsAvailable()
    {
        // TODO: Initialize cover pages client
    }

    [When(@"I send a request to retrieve a Fax Cover Page")]
    public async Task WhenISendARequestToRetrieveAFaxCoverPage()
    {
        // TODO: Implement get cover page
    }

    [Then(@"the response contains the Fax Cover Page details")]
    public void ThenTheResponseContainsTheFaxCoverPageDetails()
    {
        // TODO: Verify cover page details
    }

    [When(@"I send a request to list the Fax Cover Pages")]
    public async Task WhenISendARequestToListTheFaxCoverPages()
    {
        // TODO: Implement list cover pages
    }

    [Then(@"the Fax Cover Pages list contains ""(.*)"" Fax Cover Pages")]
    public void ThenTheFaxCoverPagesListContainsFaxCoverPages(int count)
    {
        // TODO: Verify cover pages count
    }

    [When(@"I send a request to list all the Fax Cover Pages")]
    public async Task WhenISendARequestToListAllTheFaxCoverPages()
    {
        // TODO: Implement list all cover pages
    }

    [When(@"I iterate manually over the Fax Cover Pages pages")]
    public async Task WhenIIterateManuallyOverTheFaxCoverPagesPages()
    {
        // TODO: Implement manual iteration
    }

    [Then(@"the Fax Cover Pages iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheFaxCoverPagesIterationResultContainsTheDataFromPages(int count)
    {
        // TODO: Verify iteration results
    }

    [When(@"I send a request to add a new Fax Cover Page to a service")]
    public async Task WhenISendARequestToAddANewFaxCoverPageToAService()
    {
        // TODO: Implement add cover page
    }

    [Then(@"the Fax Cover Page is created")]
    public void ThenTheFaxCoverPageIsCreated()
    {
        // TODO: Verify cover page creation
    }

    [When(@"I send a request to delete a Fax Cover Page")]
    public async Task WhenISendARequestToDeleteAFaxCoverPage()
    {
        // TODO: Implement delete cover page
    }

    [When(@"I send a request to remove a Fax Cover Page")]
    public async Task WhenISendARequestToRemoveAFaxCoverPage()
    {
        // TODO: Implement remove cover page
    }

    [Then(@"the delete Fax Cover Page response contains no data")]
    public void ThenTheDeleteFaxCoverPageResponseContainsNoData()
    {
        // TODO: Verify delete response
    }

    [Then(@"the response contains {string} Fax Cover Pages")]
    public void ThenTheResponseContainsFaxCoverPages(string count)
    {
        // TODO: Verify cover pages count
    }
}
