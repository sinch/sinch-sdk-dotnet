using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;

namespace Sinch.Tests.Features.Conversation;

[Binding]
public class Contacts
{
    private const string ContactId001 = "01W4FFL35P4NC4K35CONTACT001";
    private const string ContactId002 = "01W4FFL35P4NC4K35CONTACT002";
    private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";

    private ISinchConversationContacts _contacts;
    private Contact _contact;
    private ListContactsResponse _listContactsResponse;
    private List<Contact> _allContacts;
    private int _totalContactsPages;
    private ChannelProfile _channelProfile;
    private bool _deleteCompleted;
    private ListIdentityConflictsResponse _listConflictsResponse;
    private List<IdentityConflict> _allConflicts;
    private int _totalConflictsPages;

    [Given(@"the Conversation service ""Contacts"" is available")]
    public void GivenTheConversationServiceContactsIsAvailable()
    {
        _contacts = Utils.SinchConversationClient().Contacts;
    }

    [When(@"I send a request to create a contact")]
    public async Task WhenISendARequestToCreateAContact()
    {
        _contact = await _contacts.Create(new CreateContactRequest
        {
            ChannelIdentities = new List<ChannelIdentity>
            {
                new() { Channel = ConversationChannel.Sms, Identity = "+12015555555" }
            },
            Language = "EN_US",
            DisplayName = "Marty McFly",
            Email = "time.traveler@delorean.com"
        });
    }

    [Then(@"the contact is created")]
    public void ThenTheContactIsCreated()
    {
        _contact.Should().NotBeNull();
        _contact.Id.Should().Be(ContactId001);
        _contact.DisplayName.Should().Be("Marty McFly");
        _contact.Email.Should().Be("time.traveler@delorean.com");
        _contact.Language.Should().Be(ConversationLanguage.EnglishUS);
        _contact.ChannelIdentities.Should().HaveCount(1);
        _contact.ChannelIdentities![0].Channel.Should().Be(ConversationChannel.Sms);
        _contact.ChannelIdentities![0].Identity.Should().Be("12015555555");
    }

    [When(@"I send a request to list the existing contacts")]
    public async Task WhenISendARequestToListTheExistingContacts()
    {
        _listContactsResponse = await _contacts.List(new ListContactsRequest { PageSize = 2 });
    }

    [Then(@"the response contains ""(.*)"" contacts")]
    public void ThenTheResponseContainsContacts(int count)
    {
        _listContactsResponse.Contacts.Should().HaveCount(count);
        _listContactsResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the contacts")]
    public async Task WhenISendARequestToListAllTheContacts()
    {
        _allContacts = new List<Contact>();
        await foreach (var contact in _contacts.ListAuto(new ListContactsRequest { PageSize = 2 }))
        {
            _allContacts.Add(contact);
        }
    }

    [Then(@"the contacts list contains ""(.*)"" contacts")]
    public void ThenTheContactsListContainsContacts(int count)
    {
        _allContacts.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the contacts pages")]
    public async Task WhenIIterateManuallyOverTheContactsPages()
    {
        _allContacts = new List<Contact>();
        _totalContactsPages = 0;
        ListContactsResponse response = null;
        while (true)
        {
            response = await _contacts.List(new ListContactsRequest
            {
                PageSize = 2,
                PageToken = response?.NextPageToken
            });
            if (response.Contacts == null || response.Contacts.Count == 0) break;
            _allContacts.AddRange(response.Contacts);
            _totalContactsPages++;
            if (string.IsNullOrEmpty(response.NextPageToken)) break;
        }
    }

    [Then(@"the contacts iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheContactsIterationResultContainsTheDataFromPages(int count)
    {
        _totalContactsPages.Should().Be(count);
    }

    [When(@"I send a request to retrieve a contact")]
    public async Task WhenISendARequestToRetrieveAContact()
    {
        _contact = await _contacts.Get(ContactId001);
    }

    [Then(@"the response contains the contact details")]
    public void ThenTheResponseContainsTheContactDetails()
    {
        _contact.Should().NotBeNull();
        _contact.Id.Should().Be(ContactId001);
        _contact.DisplayName.Should().Be("Marty McFly");
        _contact.Email.Should().Be("time.traveler@delorean.com");
        _contact.Language.Should().Be(ConversationLanguage.EnglishUS);
        _contact.ChannelIdentities.Should().HaveCount(1);
        _contact.ChannelIdentities![0].Channel.Should().Be(ConversationChannel.Sms);
        _contact.ChannelIdentities![0].Identity.Should().Be("12015555555");
    }

    [When(@"I send a request to update a contact")]
    public async Task WhenISendARequestToUpdateAContact()
    {
        var contactToUpdate = new Contact
        {
            Id = ContactId001,
            ChannelIdentities = new List<ChannelIdentity>
            {
                new() { Channel = ConversationChannel.Messenger, Identity = "7968425018576406", AppId = AppId },
                new() { Channel = ConversationChannel.Sms, Identity = "12015555555" }
            },
            ChannelPriority = new List<ConversationChannel> { ConversationChannel.Messenger }
        };
        _contact = await _contacts.Update(contactToUpdate);
    }

    [Then(@"the response contains the contact details with updated data")]
    public void ThenTheResponseContainsTheContactDetailsWithUpdatedData()
    {
        _contact.Should().NotBeNull();
        _contact.Id.Should().Be(ContactId001);
        _contact.DisplayName.Should().Be("Marty McFly");
        _contact.ChannelIdentities.Should().HaveCount(2);
        _contact.ChannelIdentities!.Should().Contain(ci =>
            ci.Channel == ConversationChannel.Messenger && ci.Identity == "7968425018576406");
        _contact.ChannelIdentities!.Should().Contain(ci =>
            ci.Channel == ConversationChannel.Sms && ci.Identity == "12015555555");
        _contact.ChannelPriority.Should().ContainSingle(c => c == ConversationChannel.Messenger);
    }

    [When(@"I send a request to delete a contact")]
    public async Task WhenISendARequestToDeleteAContact()
    {
        await _contacts.Delete(ContactId001);
        _deleteCompleted = true;
    }

    [Then(@"the delete contact response contains no data")]
    public void ThenTheDeleteContactResponseContainsNoData()
    {
        _deleteCompleted.Should().BeTrue();
    }

    [When(@"I send a request to merge a source contact to a destination contact")]
    public async Task WhenISendARequestToMergeASourceContactToADestinationContact()
    {
        _contact = await _contacts.Merge(ContactId002, ContactId001);
    }

    [Then(@"the response contains data from the destination contact and from the source contact")]
    public void ThenTheResponseContainsDataFromTheDestinationContactAndFromTheSourceContact()
    {
        _contact.Should().NotBeNull();
        _contact.Id.Should().Be(ContactId002);
        _contact.DisplayName.Should().Be("Pika pika");
        _contact.Email.Should().Be("pikachu@poke.mon");
        _contact.Metadata.Should().Be("Some metadata");
        _contact.ChannelIdentities.Should().HaveCount(3);
        _contact.ChannelIdentities!.Should().Contain(ci =>
            ci.Channel == ConversationChannel.Messenger && ci.Identity == "7968425018576406");
        _contact.ChannelIdentities!.Should().Contain(ci =>
            ci.Channel == ConversationChannel.Mms && ci.Identity == "12016666666");
        _contact.ChannelIdentities!.Should().Contain(ci =>
            ci.Channel == ConversationChannel.Sms && ci.Identity == "12015555555");
        _contact.ChannelPriority.Should().HaveCount(2);
        _contact.ChannelPriority![0].Should().Be(ConversationChannel.Mms);
        _contact.ChannelPriority![1].Should().Be(ConversationChannel.Messenger);
    }

    [When(@"I send a request to get the channel profile of a contact ID")]
    public async Task WhenISendARequestToGetTheChannelProfileOfAContactID()
    {
        _channelProfile = await _contacts.GetChannelProfile(new GetChannelProfileRequest
        {
            AppId = AppId,
            Recipient = new ContactRecipient { ContactId = ContactId001 },
            Channel = ChannelProfileConversationChannel.Messenger
        });
    }

    [Then(@"the response contains the profile of the contact on the requested channel")]
    public void ThenTheResponseContainsTheProfileOfTheContactOnTheRequestedChannel()
    {
        _channelProfile.Should().NotBeNull();
        _channelProfile.ProfileName.Should().Be("Marty McFly FB");
    }

    [When(@"I send a request to list the existing identity conflicts")]
    public async Task WhenISendARequestToListTheExistingIdentityConflicts()
    {
        _listConflictsResponse = await _contacts.ListIdentityConflicts(new ListIdentityConflictsRequest { PageSize = 2 });
    }

    [Then(@"the response contains ""(.*)"" identity conflicts")]
    public void ThenTheResponseContainsIdentityConflicts(int count)
    {
        _listConflictsResponse.Conflicts.Should().HaveCount(count);
        _listConflictsResponse.NextPageToken.Should().NotBeNullOrEmpty();
    }

    [When(@"I send a request to list all the identity conflicts")]
    public async Task WhenISendARequestToListAllTheIdentityConflicts()
    {
        _allConflicts = new List<IdentityConflict>();
        await foreach (var conflict in _contacts.ListIdentityConflictsAuto(new ListIdentityConflictsRequest { PageSize = 2 }))
        {
            _allConflicts.Add(conflict);
        }
    }

    [Then(@"the identity conflicts list contains ""(.*)"" identity conflicts")]
    public void ThenTheIdentityConflictsListContainsIdentityConflicts(int count)
    {
        _allConflicts.Should().HaveCount(count);
    }

    [When(@"I iterate manually over the identity conflicts pages")]
    public async Task WhenIIterateManuallyOverTheIdentityConflictsPages()
    {
        _allConflicts = new List<IdentityConflict>();
        _totalConflictsPages = 0;
        ListIdentityConflictsResponse response = null;
        while (true)
        {
            response = await _contacts.ListIdentityConflicts(new ListIdentityConflictsRequest
            {
                PageSize = 2,
                PageToken = response?.NextPageToken
            });
            if (response.Conflicts == null || response.Conflicts.Count == 0) break;
            _allConflicts.AddRange(response.Conflicts);
            _totalConflictsPages++;
            if (string.IsNullOrEmpty(response.NextPageToken)) break;
        }
    }

    [Then(@"the identity conflicts iteration result contains the data from ""(.*)"" pages")]
    public void ThenTheIdentityConflictsIterationResultContainsTheDataFromPages(int count)
    {
        _totalConflictsPages.Should().Be(count);
    }
}
