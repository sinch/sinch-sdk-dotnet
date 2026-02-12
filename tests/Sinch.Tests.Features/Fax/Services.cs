using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Fax.Services;
using Sinch.Fax.Emails;
using Sinch.Fax.Faxes;

namespace Sinch.Tests.Features.Fax;

[Binding]
public class Services
{
    private ISinchFaxServices _servicesApi;
    private Service _createServiceResponse;
    private ListFaxServicesResponse _listFaxResponse;
    private readonly List<Service> _servicesList = new();
    private Service _service;
    private ListNumbersResponse _listNumbersResponse;
    private readonly List<ServicePhoneNumber> _numbersList = new();
    private ListEmailsResponse<string> _listEmailsResponse;
    private readonly List<string> _emailsList = new();

    [Given(@"the Fax service ""Services"" is available")]
    public void GivenTheFaxServiceIsAvailable()
    {
        var faxClient = Utils.SinchFaxClient();
        _servicesApi = faxClient.Services;
    }

    [When(@"I send a request to create a new service")]
    public async Task WhenISendARequestToCreateANewService()
    {
        _createServiceResponse = await _servicesApi.Create(
            new CreateFaxServiceRequest
            {
                Name = "Fax service for e2e tests",
                IncomingWebhookUrl = "https://my-callback-server.com/fax",
                WebhookContentType = CallbackUrlContentType.ApplicationJson,
                DefaultForProject = false,
                DefaultFrom = "+12014444444",
                NumberOfRetries = 2,
                RetryDelaySeconds = 30,
                ImageConversionMethod = ImageConversionMethod.Monochrome,
                SaveInboundFaxDocuments = true,
                SaveOutboundFaxDocuments = true
            }
        );
    }

    [Then(@"the service is created")]
    public void ThenTheServiceIsCreated()
    {
        _createServiceResponse.Id.Should().Be("01W4FFL35P4NC4K35FAXSERVICE");
    }

    [When(@"I send a request to list the existing services")]
    public async Task WhenISendARequestToListTheExistingServices()
    {
        _listFaxResponse = await _servicesApi.List(pageSize: 2);
    }

    [Then("the response contains {string} services")]
    public void ThenTheResponseContainsServices(string expectedAnswer)
    {
        var expectedServices = int.Parse(expectedAnswer);
        _listFaxResponse.Services.Should().HaveCount(expectedServices);
    }

    [When(@"I send a request to list all the services")]
    public async Task WhenISendARequestToListAllTheServices()
    {
        await foreach (var service in _servicesApi.ListAuto(pageSize: 2))
        {
            _servicesList.Add(service);
        }
    }

    [Then("the services list contains {string} services")]
    public void ThenTheServicesListContainsServices(string expectedAnswer)
    {
        var expectedServices = int.Parse(expectedAnswer);
        _servicesList.Should().HaveCount(expectedServices);
    }

    [When(@"I send a request to retrieve a service")]
    public async Task WhenISendARequestToRetrieveAService()
    {
        _service = await _servicesApi.Get("01W4FFL35P4NC4K35FAXSERVICE");
    }

    [Then(@"the response contains a service object")]
    public void ThenTheResponseContainsAServiceObject()
    {
        _service.Should().NotBeNull();
        _service.Id.Should().Be("01W4FFL35P4NC4K35FAXSERVICE");
        _service.WebhookContentType.Should().Be(CallbackUrlContentType.ApplicationJson);
        _service.DefaultFrom.Should().Be("+12014444444");
        _service.NumberOfRetries.Should().Be(2);
        _service.RetryDelaySeconds.Should().Be(30);
        _service.ImageConversionMethod.Should().Be(ImageConversionMethod.Monochrome);
        _service.ProjectId.Should().Be("123coffee-dada-beef-cafe-baadc0de5678");
        _service.DefaultForProject.Should().BeFalse();
        _service.SaveInboundFaxDocuments.Should().BeTrue();
        _service.SaveOutboundFaxDocuments.Should().BeTrue();
        _service.Name.Should().Be("Fax service for e2e tests");
        _service.IncomingWebhookUrl.Should().Be("https://my-callback-server.com/fax");
    }

    [When(@"I send a request to update a service")]
    public async Task WhenISendARequestToUpdateAService()
    {
        _service = await _servicesApi.Update(
            new UpdateFaxServiceRequest
            {
                Id = "01W4FFL35P4NC4K35FAXSERVICE",
                Name = "Updated Fax service name",
                WebhookContentType = CallbackUrlContentType.MultipartFormData,
                DefaultForProject = true,
                NumberOfRetries = 3,
                RetryDelaySeconds = 60,
                ImageConversionMethod = ImageConversionMethod.Halftone,
                SaveOutboundFaxDocuments = false,
                SaveInboundFaxDocuments = false
            }
        );
    }

    [Then(@"the response contains a service with updated parameters")]
    public void ThenTheResponseContainsAServiceWithUpdatedParameters()
    {
        _service.Should().NotBeNull();
        _service.Id.Should().Be("01W4FFL35P4NC4K35FAXSERVICE");
        _service.WebhookContentType.Should().Be(CallbackUrlContentType.MultipartFormData);
        _service.NumberOfRetries.Should().Be(3);
        _service.RetryDelaySeconds.Should().Be(60);
        _service.ImageConversionMethod.Should().Be(ImageConversionMethod.Halftone);
        _service.DefaultForProject.Should().BeTrue();
        _service.SaveInboundFaxDocuments.Should().BeFalse();
        _service.SaveOutboundFaxDocuments.Should().BeFalse();
        _service.Name.Should().Be("Updated Fax service name");
    }

    [When(@"I send a request to remove a service")]
    public async Task WhenISendARequestToRemoveAService()
    {
        await _servicesApi.Delete("01W4FFL35P4NC4K35FAXSERVICE");
    }

    [Then(@"the delete service response contains no data")]
    public void ThenTheDeleteServiceResponseContainsNoData()
    {
        // Delete operation returns Task with no data, successful if no exception is thrown
    }

    [When(@"I send a request to list the numbers associated to a fax service")]
    public async Task WhenISendARequestToListTheNumbersAssociatedToAFaxService()
    {
        _listNumbersResponse = await _servicesApi.ListNumbers("01W4FFL35P4NC4K35FAXSERVICE", pageSize: 20);
    }

    [Then("the response contains {string} numbers associated to the fax service")]
    public void ThenTheResponseContainsNumbersAssociatedToTheFaxService(string expectedAnswer)
    {
        var expectedNumbers = int.Parse(expectedAnswer);
        _listNumbersResponse.PhoneNumbers.Should().HaveCount(expectedNumbers);
    }

    [When(@"I send a request to list all the numbers associated to a fax service")]
    public async Task WhenISendARequestToListAllTheNumbersAssociatedToAFaxService()
    {
        await foreach (var number in _servicesApi.ListNumbersAuto("01W4FFL35P4NC4K35FAXSERVICE", pageSize: 20))
        {
            _numbersList.Add(number);
        }
    }

    [Then("the phone numbers list contains {string} numbers associated to the fax service")]
    public void ThenThePhoneNumbersListContainsNumbersAssociatedToTheFaxService(string expectedAnswer)
    {
        var expectedNumbers = int.Parse(expectedAnswer);
        _numbersList.Should().HaveCount(expectedNumbers);
    }

    [When(@"I send a request to list the emails associated to a phone number")]
    public async Task WhenISendARequestToListTheEmailsAssociatedToAPhoneNumber()
    {
        _listEmailsResponse = await _servicesApi.ListEmailsForNumber("01W4FFL35P4NC4K35FAXSERVICE", "+12014444444");
    }

    [Then("the response contains {string} emails associated to the phone number")]
    public void ThenTheResponseContainsEmailsAssociatedToThePhoneNumber(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _listEmailsResponse.Emails.Should().HaveCount(expectedEmails);
    }

    [When(@"I send a request to list all the emails associated to a phone number")]
    public async Task WhenISendARequestToListAllTheEmailsAssociatedToAPhoneNumber()
    {
        await foreach (var email in _servicesApi.ListEmailsForNumberAuto("01W4FFL35P4NC4K35FAXSERVICE", "+12014444444"))
        {
            _emailsList.Add(email);
        }
    }

    [Then("the emails list contains {string} emails associated to a phone number")]
    public void ThenTheEmailsListContainsEmailsAssociatedToAPhoneNumber(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _emailsList.Should().HaveCount(expectedEmails);
    }
}
