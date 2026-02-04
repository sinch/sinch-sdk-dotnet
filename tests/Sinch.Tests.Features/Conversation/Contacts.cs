using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;

namespace Sinch.Tests.Features.Conversation
{
    [Binding]
    public class Contacts
    {
        private const string ContactId001 = "01W4FFL35P4NC4K35CONTACT001";
        private const string ContactId002 = "01W4FFL35P4NC4K35CONTACT002";
        private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";

        private ISinchConversationContacts _contacts;
        private Contact _contact;
        private IEnumerable<Contact> _contactsPage;
        private List<Contact> _contactsList;
        private int _pagesIteration;
        private ChannelProfile _channelProfile;
        private bool _deleteCompleted;

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
                    new()
                    {
                        Channel = ConversationChannel.Sms,
                        Identity = "+12015555555"
                    }
                },
                Language = "EN_US",
                DisplayName = "Marty McFly",
                Email = "time.traveler@delorean.com"
            });
        }

        [Then(@"the contact is created")]
        public void ThenTheContactIsCreated()
        {
            _contact.Id.Should().Be(ContactId001);
        }

        [When(@"I send a request to list the existing contacts")]
        public async Task WhenISendARequestToListTheExistingContacts()
        {
            var response = await _contacts.List(new ListContactsRequest { PageSize = 2 });
            _contactsPage = response.Contacts ?? [];
        }

        [Then(@"the response contains ""{int}"" contacts")]
        public void ThenTheResponseContainsContacts(int expectedCount)
        {
            _contactsPage.Count().Should().Be(expectedCount);
        }

        [When(@"I send a request to list all the contacts")]
        public async Task WhenISendARequestToListAllTheContacts()
        {
            _contactsList = [];
            await foreach (var contact in _contacts.ListAuto(new ListContactsRequest { PageSize = 2 }))
            {
                _contactsList.Add(contact);
            }
        }

        [When(@"I iterate manually over the contacts pages")]
        public async Task WhenIIterateManuallyOverTheContactsPages()
        {
            _contactsList = [];
            var response = await _contacts.List(new ListContactsRequest { PageSize = 2 });
            _contactsList.AddRange(response.Contacts ?? []);
            _pagesIteration = 1;

            while (!string.IsNullOrEmpty(response.NextPageToken))
            {
                response = await _contacts.List(new ListContactsRequest { PageSize = 2, PageToken = response.NextPageToken });
                _contactsList.AddRange(response.Contacts ?? []);
                _pagesIteration++;
            }
        }

        [Then(@"the contacts list contains ""{int}"" contacts")]
        public void ThenTheContactsListContainsContacts(int expectedCount)
        {
            _contactsList.Count().Should().Be(expectedCount);
        }

        [Then(@"the contacts iteration result contains the data from ""{int}"" pages")]
        public void ThenTheContactsIterationResultContainsDataFromPages(int expectedPages)
        {
            _pagesIteration.Should().Be(expectedPages);
        }

        [When(@"I send a request to retrieve a contact")]
        public async Task WhenISendARequestToRetrieveAContact()
        {
            _contact = await _contacts.Get(ContactId001);
        }

        [Then(@"the response contains the contact details")]
        public void ThenTheResponseContainsTheContactDetails()
        {
            AssertCommonContactFields(_contact);
            _contact.Id.Should().Be(ContactId001);
            _contact.Email.Should().Be("time.traveler@delorean.com");
            _contact.ChannelIdentities.Should().BeEquivalentTo(new List<ChannelIdentity>
            {
                new()
                {
                    Channel = ConversationChannel.Sms,
                    Identity = "12015555555",
                    AppId = string.Empty
                }
            }, options => options.WithStrictOrdering());
            _contact.ChannelPriority.Should().BeEmpty();
        }

        [When(@"I send a request to update a contact")]
        public async Task WhenISendARequestToUpdateAContact()
        {
            var contactToUpdate = new Contact
            {
                Id = ContactId001,
                ChannelIdentities = new List<ChannelIdentity>
                {
                    new()
                    {
                        Channel = ConversationChannel.Messenger,
                        Identity = "7968425018576406",
                        AppId = AppId
                    },
                    new()
                    {
                        Channel = ConversationChannel.Sms,
                        Identity = "12015555555"
                    }
                },
                ChannelPriority = new List<ConversationChannel> { ConversationChannel.Messenger }
            };
            _contact = await _contacts.Update(contactToUpdate);
        }

        [Then(@"the response contains the contact details with updated data")]
        public void ThenTheResponseContainsTheContactDetailsWithUpdatedData()
        {
            AssertCommonContactFields(_contact);
            _contact.Id.Should().Be(ContactId001);
            _contact.Email.Should().Be("time.traveler@delorean.com");
            _contact.ChannelIdentities.Should().BeEquivalentTo(new List<ChannelIdentity>
            {
                new()
                {
                    Channel = ConversationChannel.Messenger,
                    Identity = "7968425018576406",
                    AppId = AppId
                },
                new()
                {
                    Channel = ConversationChannel.Sms,
                    Identity = "12015555555",
                    AppId = string.Empty
                }
            }, options => options.WithStrictOrdering());
            _contact.ChannelPriority.Should().BeEquivalentTo(new List<ConversationChannel>
            {
                ConversationChannel.Messenger
            }, options => options.WithStrictOrdering());
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
        public void ThenTheResponseContainsDataFromTheDestinationAndSourceContact()
        {
            AssertCommonContactFields(_contact);
            _contact.Id.Should().Be(ContactId002);
            _contact.ChannelIdentities.Should().HaveCount(3);
            _contact.ChannelPriority.Should().HaveCount(2);
            _contact.Email.Should().Be("pikachu@poke.mon");
        }

        [When(@"I send a request to get the channel profile of a contact ID")]
        public async Task WhenISendARequestToGetTheChannelProfileOfAContactId()
        {
            _channelProfile = await _contacts.GetChannelProfile(new GetChannelProfileRequest
            {
                AppId = AppId,
                Channel = ChannelProfileConversationChannel.Messenger,
                Recipient = new ContactRecipient
                {
                    ContactId = ContactId001
                }
            });
        }

        [Then(@"the response contains the profile of the contact on the requested channel")]
        public void ThenTheResponseContainsTheProfileOfTheContactOnTheRequestedChannel()
        {
            _channelProfile.ProfileName.Should().Be("Marty McFly FB");
        }
        
        [When(@"I send a request to list the existing identity conflicts")]
        public void WhenISendARequestToListTheExistingIdentityConflicts()
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }

        [Then(@"the response contains ""(.*)"" identity conflicts")]
        public void ThenTheResponseContainsIdentityConflicts(int count)
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }

        [When(@"I send a request to list all the identity conflicts")]
        public void WhenISendARequestToListAllTheIdentityConflicts()
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }

        [Then(@"the identity conflicts list contains ""(.*)"" identity conflicts")]
        public void ThenTheIdentityConflictsListContainsIdentityConflicts(int count)
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }

        [When(@"I iterate manually over the identity conflicts pages")]
        public void WhenIIterateManuallyOverTheIdentityConflictsPages()
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }

        [Then(@"the identity conflicts iteration result contains the data from ""(.*)"" pages")]
        public void ThenTheIdentityConflictsIterationResultContainsDataFromPages(int count)
        {
            // TODO: Identity conflicts are not supported in the .NET SDK
        }
        
        private void AssertCommonContactFields(Contact contact)
        {
            contact.Language.Should().Be(ConversationLanguage.EnglishUS);
        }
    }
}
