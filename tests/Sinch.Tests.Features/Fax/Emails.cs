using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Fax.Emails;

namespace Sinch.Tests.Features.Fax;

[Binding]
public class Emails
{
    private ISinchFaxEmails _emailsApi;
    private ListEmailsResponse<string> _listEmailsForNumberResponse;
    private readonly List<string> _emailsForNumber = new();
    private ListEmailsResponse<EmailAddress> _listEmailsResponse;
    private readonly List<EmailAddress> _emailsList = new();
    private EmailAddress _email;
    private Func<Task> _deleteEmailOp;
    private ListNumbersResponse _listNumbersResponse;
    private readonly List<ServicePhoneNumber> _numbersList = new();

    [Given("the Fax service \"Emails\" is available")]
    public void GivenTheFaxServiceEmailsIsAvailable()
    {
        var faxClient = Utils.SinchFaxClient();
        _emailsApi = faxClient.Emails;
    }

    [When("I send a request to list the emails associated to a phone number via the \"Emails\" Service")]
    public async Task WhenISendARequestToListTheEmailsAssociatedToAPhoneNumberViaTheEmailsService()
    {
        _listEmailsForNumberResponse = await _emailsApi.ListForNumber(
            "01W4FFL35P4NC4K35FAXSERVICE",
            "+12014444444"
        );
    }

    [Then("the \"Emails\" Service response contains {string} emails associated to the phone number")]
    public void ThenTheEmailsServiceResponseContainsEmailsAssociatedToThePhoneNumber(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _listEmailsForNumberResponse.Emails.Should().HaveCount(expectedEmails);
    }

    [When("I send a request to list all the emails associated to a phone number via the \"Emails\" Service")]
    public async Task WhenISendARequestToListAllTheEmailsAssociatedToAPhoneNumberViaTheEmailsService()
    {
        await foreach (var email in _emailsApi.ListForNumberAuto(
                           "01W4FFL35P4NC4K35FAXSERVICE",
                           "+12014444444"
                       ))
        {
            _emailsForNumber.Add(email);
        }
    }

    [Then("the emails list from the \"Emails\" Service contains {string} emails associated to a phone number")]
    public void ThenTheEmailsListFromTheEmailsServiceContainsEmailsAssociatedToAPhoneNumber(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _emailsForNumber.Should().HaveCount(expectedEmails);
    }

    [When("I send a request to list the emails associated to the project")]
    public async Task WhenISendARequestToListTheEmailsAssociatedToTheProject()
    {
        _listEmailsResponse = await _emailsApi.List("01W4FFL35P4NC4K35FAXSERVICE");
    }

    [Then("the response contains {string} emails associated to the project")]
    public void ThenTheResponseContainsEmailsAssociatedToTheProject(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _listEmailsResponse.Emails.Should().HaveCount(expectedEmails);
    }

    [When("I send a request to list all the emails associated to the project")]
    public async Task WhenISendARequestToListAllTheEmailsAssociatedToTheProject()
    {
        await foreach (var email in _emailsApi.ListAuto("01W4FFL35P4NC4K35FAXSERVICE"))
        {
            _emailsList.Add(email);
        }
    }

    [Then("the emails list contains {string} emails associated to the project")]
    public void ThenTheEmailsListContainsEmailsAssociatedToTheProject(string expectedAnswer)
    {
        var expectedEmails = int.Parse(expectedAnswer);
        _emailsList.Should().HaveCount(expectedEmails);
    }

    [When("I send a request to add a new email to the project")]
    public async Task WhenISendARequestToAddANewEmailToTheProject()
    {
        _email = await _emailsApi.Add(
            "01W4FFL35P4NC4K35FAXSERVICE",
            new EmailRequest
            {
                Email = "spaceship@galaxy.far.far.away",
                PhoneNumbers = new List<PhoneNumber>
                {
                    new()
                    {
                        Number = "+12016666666",
                        Permissions = PhoneNumberPermission.Both
                    }
                }
            }
        );
    }

    [Then("the response contains the added email")]
    public void ThenTheResponseContainsTheAddedEmail()
    {
        _email.Should().NotBeNull();
        _email.Email.Should().Be("spaceship@galaxy.far.far.away");
        _email.PhoneNumbers.Should().BeEquivalentTo(new List<PhoneNumber>
        {
            new()
            {
                Number = "+12016666666",
                Permissions = PhoneNumberPermission.Both
            }
        });
        _email.ProjectId.Should().Be("123c0ffee-dada-beef-cafe-baadc0de5678");
    }

    [When("I send a request to update the phone numbers associated to an email")]
    public async Task WhenISendARequestToUpdateThePhoneNumbersAssociatedToAnEmail()
    {
        _email = await _emailsApi.Update(
            "01W4FFL35P4NC4K35FAXSERVICE",
            "spaceship@galaxy.far.far.away",
            new UpdateEmailRequest
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new()
                    {
                        Number = "+12016666666",
                        Permissions = PhoneNumberPermission.Send
                    },
                    new()
                    {
                        Number = "+12017777777",
                        Permissions = PhoneNumberPermission.Receive
                    }
                }
            }
        );
    }

    [Then("the response contains the updated email")]
    public void ThenTheResponseContainsTheUpdatedEmail()
    {
        _email.Should().NotBeNull();
        _email.Email.Should().Be("spaceship@galaxy.far.far.away");
        _email.PhoneNumbers.Should().BeEquivalentTo(new List<PhoneNumber>
        {
            new()
            {
                Number = "+12016666666",
                Permissions = PhoneNumberPermission.Send
            },
            new()
            {
                Number = "+12017777777",
                Permissions = PhoneNumberPermission.Receive
            }
        });
        _email.ProjectId.Should().Be("123c0ffee-dada-beef-cafe-baadc0de5678");
    }

    [When("I send a request to delete an email from the project")]
    public void WhenISendARequestToDeleteAnEmailFromTheProject()
    {
        _deleteEmailOp = () => _emailsApi.Delete("01W4FFL35P4NC4K35FAXSERVICE",
            "spaceship@galaxy.far.far.away");
    }

    [Then("the delete email response contains no data")]
    public async Task ThenTheDeleteEmailResponseContainsNoData()
    {
        await _deleteEmailOp.Should().NotThrowAsync();
    }

    [When("I send a request to list the phone numbers associated to an email")]
    public async Task WhenISendARequestToListThePhoneNumbersAssociatedToAnEmail()
    {
        _listNumbersResponse = await _emailsApi.ListNumbers(
            "01W4FFL35P4NC4K35FAXSERVICE",
            "cookie.monster@nom.nom"
        );
    }

    [Then("the response contains {string} phone numbers associated to the email")]
    public void ThenTheResponseContainsPhoneNumbersAssociatedToTheEmail(string expectedAnswer)
    {
        var expectedNumbers = int.Parse(expectedAnswer);
        _listNumbersResponse.PhoneNumbers.Should().HaveCount(expectedNumbers);
    }

    [When("I send a request to list all the phone numbers associated to an email")]
    public async Task WhenISendARequestToListAllThePhoneNumbersAssociatedToAnEmail()
    {
        await foreach (var number in _emailsApi.ListNumbersAuto(
                           "01W4FFL35P4NC4K35FAXSERVICE",
                           "cookie.monster@nom.nom"
                       ))
        {
            _numbersList.Add(number);
        }
    }

    [Then("the phone numbers list contains {string} phone numbers associated to the email")]
    public void ThenThePhoneNumbersListContainsPhoneNumbersAssociatedToTheEmail(string expectedAnswer)
    {
        var expectedNumbers = int.Parse(expectedAnswer);
        _numbersList.Should().HaveCount(expectedNumbers);
    }
}
