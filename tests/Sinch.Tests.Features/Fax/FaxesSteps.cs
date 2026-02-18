using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Core;
using Sinch.Fax.Faxes;
using FaxModel = Sinch.Fax.Faxes.Fax;

namespace Sinch.Tests.Features.Fax;

[Binding]
public class FaxesSteps
{
    private ISinchFaxFaxes _faxesApi;
    private readonly List<FaxModel> _faxesList = new();
    private ListFaxResponse _listFaxResponse;
    private List<FaxModel> _sendFaxResponse;
    private FaxModel _fax;
    private ContentResult _contentResult;
    private Func<Task> _deleteContentOp;

    [Given("the Fax service \"Faxes\" is available")]
    public void GivenTheFaxServiceFaxesIsAvailable()
    {
        var faxClient = Utils.SinchFaxClient();
        _faxesApi = faxClient.Faxes;
    }

    [When("I send a fax with a contentUrl only to a single recipient")]
    public async Task WhenISendAFaxWithAContentUrlOnlyToASingleRecipient()
    {
        _sendFaxResponse = new List<FaxModel>
        {
            await _faxesApi.Send(
                "+12015555555",
                new SendFaxRequest
                {
                    ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
                }
            )
        };
    }

    [Then("the response contains a list of fax objects with a single element received from a multipart-form-data request with contentUrl only")]
    public void ThenTheResponseContainsASingleFaxReceivedFromAMultipartFormDataRequestWithContentUrlOnly()
    {
        _sendFaxResponse.Should().HaveCount(1);
        _sendFaxResponse[0].Id.Should().Be("01W4FFL35P4NC4K35URLSINGLE1");
    }

    [When("I send a fax with a contentUrl only to multiple recipients")]
    public async Task WhenISendAFaxWithAContentUrlOnlyToMultipleRecipients()
    {
        _sendFaxResponse = await _faxesApi.Send(
            new List<string> { "+12015555555", "+12016666666" },
            new SendFaxRequest
            {
                ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
            }
        );
    }

    [Then("the response contains a list of fax objects with multiple elements received from a multipart-form-data request with contentUrl only")]
    public void ThenTheResponseContainsMultipleFaxesReceivedFromAMultipartFormDataRequestWithContentUrlOnly()
    {
        _sendFaxResponse.Should().HaveCount(2);
        _sendFaxResponse[0].Id.Should().Be("01W4FFL35P4NC4K35URLMULTI01");
        _sendFaxResponse[1].Id.Should().Be("01W4FFL35P4NC4K35URLMULTI02");
    }

    [When("I send a fax with a contentUrl and a binary file attachment to a single recipient")]
    public async Task WhenISendAFaxWithAContentUrlAndABinaryFileAttachmentToASingleRecipient()
    {
        var fileBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        await using var stream = new MemoryStream(fileBytes);
        using var request = new SendFaxRequest(stream, "sinch-logo.png")
        {
            ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
        };

        _sendFaxResponse = new List<FaxModel>
        {
            await _faxesApi.Send("+12015555555", request)
        };
    }

    [Then("the response contains a list of fax objects with a single element received from a multipart-form-data request")]
    public void ThenTheResponseContainsASingleFaxReceivedFromAMultipartFormDataRequest()
    {
        _sendFaxResponse.Should().HaveCount(1);
        _sendFaxResponse[0].Id.Should().Be("01W4FFL35P4NC4K35BINSINGLE1");
    }

    [When("I send a fax with a contentUrl and a binary file attachment to multiple recipients")]
    public async Task WhenISendAFaxWithAContentUrlAndABinaryFileAttachmentToMultipleRecipients()
    {
        var fileBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        await using var stream = new MemoryStream(fileBytes);
        using var request = new SendFaxRequest(stream, "sinch-logo.png")
        {
            ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
        };

        _sendFaxResponse = await _faxesApi.Send(
            new List<string> { "+12015555555", "+12016666666" },
            request
        );
    }

    [When("I send a fax with a contentUrl and a base64 file encoded to a single recipient")]
    public async Task WhenISendAFaxWithAContentUrlAndABase64FileEncodedToASingleRecipient()
    {
        var request = new SendFaxRequest(new List<Base64File>
        {
            new()
            {
                File = "WSdhIGRlcyBqb3VycywgZmF1dCBwYXMgbSdjaGVyY2hlciAhIEV0IHknYSBkZXMgam91cnMgdG91cyBsZXMgam91cnMgIQ==",
                FileType = FileType.PDF
            },
            new()
            {
                File = "UXVhbmQgbGUgdHJvbGwgcGFybGUsIGwnaG9tbWUgYXZpc8OpIGwnw6ljb3V0ZQ==",
                FileType = FileType.PDF
            }
        })
        {
            ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
        };

        _sendFaxResponse = new List<FaxModel>
        {
            await _faxesApi.Send("+12015555555", request)
        };
    }

    [Then("the response contains a list of fax objects with a single element received from an application-json request")]
    public void ThenTheResponseContainsASingleFaxReceivedFromAnApplicationJsonRequest()
    {
        _sendFaxResponse.Should().HaveCount(1);
        _sendFaxResponse[0].Id.Should().Be("01W4FFL35P4NC4K35B64SINGLE1");
    }

    [When("I send a fax with a contentUrl and a base64 file encoded to multiple recipients")]
    public async Task WhenISendAFaxWithAContentUrlAndABase64FileEncodedToMultipleRecipients()
    {
        var request = new SendFaxRequest(new List<Base64File>
        {
            new()
            {
                File = "WSdhIGRlcyBqb3VycywgZmF1dCBwYXMgbSdjaGVyY2hlciAhIEV0IHknYSBkZXMgam91cnMgdG91cyBsZXMgam91cnMgIQ==",
                FileType = FileType.PDF
            },
            new()
            {
                File = "UXVhbmQgbGUgdHJvbGwgcGFybGUsIGwnaG9tbWUgYXZpc8OpIGwnw6ljb3V0ZQ==",
                FileType = FileType.PDF
            }
        })
        {
            ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
        };

        _sendFaxResponse = await _faxesApi.Send(
            new List<string> { "+12015555555", "+12016666666" },
            request
        );
    }

    [Then("the response contains a list of fax objects with multiple elements received from an application-json request")]
    public void ThenTheResponseContainsMultipleFaxesReceivedFromAnApplicationJsonRequest()
    {
        _sendFaxResponse.Should().HaveCount(2);
        _sendFaxResponse[0].Id.Should().Be("01W4FFL35P4NC4K35B64MULTI01");
        _sendFaxResponse[1].Id.Should().Be("01W4FFL35P4NC4K35B64MULTI02");
    }

    [When("I retrieve a fax")]
    public async Task WhenIRetrieveAFax()
    {
        _fax = await _faxesApi.Get("01W4FFL35P4NC4K35CR3P35M1N1");
    }

    [Then("the response contains a fax object")]
    public void ThenTheResponseContainsAFaxObject()
    {
        _fax.Id.Should().Be("01W4FFL35P4NC4K35CR3P35M1N1");
        _fax.Direction.Should().Be(Direction.Outbound);
        _fax.From.Should().Be("+12014444444");
        _fax.To.Should().Be("+12015555555");
        _fax.NumberOfPages.Should().Be(1);
        _fax.Status.Should().Be(FaxStatus.Completed);
        _fax.HeaderTimeZone.Should().Be("America/New_York");
        _fax.RetryDelaySeconds.Should().Be(60);
        _fax.CallbackUrlContentType.Should().Be(CallbackUrlContentType.MultipartFormData);
        _fax.ProjectId.Should().Be("123coffee-dada-beef-cafe-baadc0de5678");
        _fax.ServiceId.Should().Be("01K1TTENC4TSJ0LLYJ1GGLYJU1Y");
        _fax.Price.Should().NotBeNull();
        _fax.Price!.CurrencyCode.Should().Be("USD");
        _fax.Price.Amount.Should().BeApproximately(0.07f, 0.0001f);
        _fax.MaxRetries.Should().Be(3);
        _fax.CreateTime.Should().Be(DateTimeOffset.Parse("2024-06-06T14:42:42Z").UtcDateTime);
        _fax.CompletedTime.Should().Be(DateTimeOffset.Parse("2024-06-06T14:43:17Z").UtcDateTime);
        _fax.HeaderPageNumbers.Should().BeTrue();
        _fax.ContentUrl.Should().NotBeNull();
        _fax.ContentUrl![0].Should().Be("https://developers.sinch.com/fax/fax.pdf");
        _fax.ImageConversionMethod.Should().Be(ImageConversionMethod.Monochrome);
        _fax.HasFile.Should().BeTrue();
    }

    [When("I send a request to list faxes")]
    public async Task WhenISendARequestToListFaxes()
    {
        _listFaxResponse = await _faxesApi.List(new ListFaxesRequest { PageSize = 2 });
    }

    [Then("the response contains {string} faxes")]
    public void ThenTheResponseContainsFaxes(string expectedAnswer)
    {
        var expectedFaxes = int.Parse(expectedAnswer);
        _listFaxResponse.Faxes.Should().NotBeNull();
        _listFaxResponse.Faxes!.Should().HaveCount(expectedFaxes);
    }

    [When("I send a request to list all the faxes")]
    public async Task WhenISendARequestToListAllTheFaxes()
    {
        _faxesList.Clear();

        var page = 1;
        while (true)
        {
            var response = await _faxesApi.List(new ListFaxesRequest
            {
                Page = page,
                PageSize = 2
            });

            if (response.Faxes != null)
            {
                _faxesList.AddRange(response.Faxes);
            }

            if (response.PageNumber * response.PageSize >= response.TotalItems)
            {
                break;
            }

            page++;
        }
    }

    [Then("the faxes list contains {string} faxes")]
    public void ThenTheFaxesListContainsFaxes(string expectedAnswer)
    {
        var expectedFaxes = int.Parse(expectedAnswer);
        _faxesList.Should().HaveCount(expectedFaxes);
    }

    [When("I send a request to download a fax content as PDF")]
    public async Task WhenISendARequestToDownloadAFaxContentAsPdf()
    {
        _contentResult = await _faxesApi.DownloadContent("01W4FFL35P4NC4K35CR3P35DWLD");
    }

    [Then("the response contains a PDF document")]
    public void ThenTheResponseContainsAPdfDocument()
    {
        _contentResult.Should().NotBeNull();
        _contentResult.FileName.Should().Be("01W4FFL35P4NC4K35CR3P35DWLD.pdf");
    }

    [When("I send a request to delete a fax content on the server")]
    public void WhenISendARequestToDeleteAFaxContentOnTheServer()
    {
        _deleteContentOp = () => _faxesApi.DeleteContent("01W4FFL35P4NC4K35CR3P35DEL0");
    }

    [Then("the response contains no data")]
    public async Task ThenTheResponseContainsNoData()
    {
        await _deleteContentOp.Should().NotThrowAsync();
    }
}
