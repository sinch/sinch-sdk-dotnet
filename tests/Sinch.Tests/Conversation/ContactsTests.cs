using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;
using Sinch.Conversation.Contacts.Merge;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class ContactsTests : ConversationTestBase
    {
        private const string ContactId001 = "01W4FFL35P4NC4K35CONTACT001";
        private const string ContactId002 = "01W4FFL35P4NC4K35CONTACT002";
        private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";
        private readonly string _contactsUrl = $"https://us.conversation.api.sinch.com/v1/projects/{ProjectId}/contacts";

        [Fact]
        public void ContactMaskTwoFields()
        {
            var contact = new Contact()
            {
                DisplayName = "hola",
                Metadata = null
            };
            contact.GetPropertiesMask().Should().BeEquivalentTo("display_name,metadata");
        }

        [Fact]
        public void ContactMaskAllFields()
        {
            var contact = new Contact()
            {
                DisplayName = "hola",
                Metadata = "aaaa",
                ExternalId = "id",
                ChannelPriority = new List<ConversationChannel>(),
                Email = "mail",
                ChannelIdentities = new List<ChannelIdentity>(),
                Language = ConversationLanguage.Arabic,
                Id = "id",
            };
            contact.GetPropertiesMask().Should()
                .BeEquivalentTo(
                    "display_name,metadata,external_id,channel_priority,email,channel_identities,language,id");
        }

        [Fact]
        public async Task Create_SendsCorrectRequest_ReturnsContact()
        {
            var request = new
            {
                channel_identities = new[] { new { channel = "SMS", identity = "+12015555555" } },
                language = "EN_US",
                display_name = "Marty McFly",
                email = "time.traveler@delorean.com"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, _contactsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(request))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ContactId001,
                    channel_identities = new[] { new { channel = "SMS", identity = "12015555555", app_id = "" } },
                    channel_priority = new string[] { },
                    display_name = "Marty McFly",
                    email = "time.traveler@delorean.com",
                    external_id = "",
                    metadata = "",
                    language = "EN_US"
                }));

            var contact = await Conversation.Contacts.Create(new CreateContactRequest
            {
                ChannelIdentities = [new() { Channel = ConversationChannel.Sms, Identity = "+12015555555" }],
                Language = "EN_US",
                DisplayName = "Marty McFly",
                Email = "time.traveler@delorean.com"
            });

            contact.Should().NotBeNull();
            contact.Id.Should().Be(ContactId001);
            contact.DisplayName.Should().Be("Marty McFly");
            contact.Email.Should().Be("time.traveler@delorean.com");
            contact.Language.Should().Be(ConversationLanguage.EnglishUS);
        }

        [Fact]
        public async Task Get_ReturnsContact()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"{_contactsUrl}/{ContactId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ContactId001,
                    channel_identities = new[] { new { channel = "SMS", identity = "12015555555", app_id = "" } },
                    channel_priority = new string[] { },
                    display_name = "Marty McFly",
                    email = "time.traveler@delorean.com",
                    external_id = "",
                    metadata = "",
                    language = "EN_US"
                }));

            var contact = await Conversation.Contacts.Get(ContactId001);

            contact.Should().NotBeNull();
            contact.Id.Should().Be(ContactId001);
            contact.DisplayName.Should().Be("Marty McFly");
            contact.Language.Should().Be(ConversationLanguage.EnglishUS);
        }

        [Fact]
        public async Task List_ReturnsPagedContacts()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, _contactsUrl)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    contacts = new[]
                    {
                        new { id = ContactId001, display_name = "Marty McFly" },
                        new { id = ContactId002, display_name = "Pika pika" }
                    },
                    next_page_token = "ChowMVc0RkZMMzVQNE5DNEszNUNPTlRBQ1QwMDI="
                }));

            var response = await Conversation.Contacts.List(new ListContactsRequest { PageSize = 2 });

            response.Should().NotBeNull();
            response.Contacts.Should().HaveCount(2);
            response.NextPageToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Update_SendsCorrectRequest_ReturnsUpdatedContact()
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

            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"{_contactsUrl}/{ContactId001}*")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ContactId001,
                    channel_identities = new[]
                    {
                        new { channel = "MESSENGER", identity = "7968425018576406", app_id = AppId },
                        new { channel = "SMS", identity = "12015555555", app_id = "" }
                    },
                    channel_priority = new[] { "MESSENGER" },
                    display_name = "Marty McFly",
                    email = "time.traveler@delorean.com",
                    external_id = "",
                    metadata = "",
                    language = "EN_US"
                }));

            var contact = await Conversation.Contacts.Update(contactToUpdate);

            contact.Should().NotBeNull();
            contact.Id.Should().Be(ContactId001);
            contact.ChannelIdentities.Should().HaveCount(2);
            contact.ChannelPriority.Should().ContainSingle(c => c == ConversationChannel.Messenger);
        }

        [Fact]
        public async Task Delete_SendsCorrectRequest()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"{_contactsUrl}/{ContactId001}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new { }));

            await Conversation.Contacts.Delete(ContactId001);
        }

        [Fact]
        public async Task Merge_SendsCorrectRequest_ReturnsMergedContact()
        {
            var request = new { source_id = ContactId001 };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_contactsUrl}/{ContactId002}:merge")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(request))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ContactId002,
                    channel_identities = new[]
                    {
                        new { channel = "MESSENGER", identity = "7968425018576406", app_id = AppId },
                        new { channel = "MMS", identity = "12016666666", app_id = "" },
                        new { channel = "SMS", identity = "12015555555", app_id = "" }
                    },
                    channel_priority = new[] { "MMS", "MESSENGER" },
                    display_name = "Pika pika",
                    email = "pikachu@poke.mon",
                    external_id = "",
                    metadata = "Some metadata",
                    language = "EN_US"
                }));

            var response = await Conversation.Contacts.MergeContact(ContactId002,
                new MergeContactRequest { SourceId = ContactId001 });

            response.Should().NotBeNull();
            response.Id.Should().Be(ContactId002);
            response.DisplayName.Should().Be("Pika pika");
            response.ChannelIdentities.Should().HaveCount(3);
            response.ChannelPriority.Should().HaveCount(2);
        }

        [Fact]
        public async Task Merge_WithStrategy_SendsStrategyInBody()
        {
            var request = new { source_id = ContactId001, strategy = "MERGE" };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_contactsUrl}/{ContactId002}:merge")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(request))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ContactId002,
                    display_name = "Pika pika"
                }));

            var response = await Conversation.Contacts.MergeContact(ContactId002,
                new MergeContactRequest { SourceId = ContactId001, Strategy = ContactMergeStrategy.Merge });

            response.Should().NotBeNull();
            response.Id.Should().Be(ContactId002);
        }

        [Fact]
        public async Task GetChannelProfile_SendsCorrectRequest_ReturnsProfile()
        {
            var request = new
            {
                app_id = AppId,
                recipient = new { contact_id = ContactId001 },
                channel = "MESSENGER"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"{_contactsUrl}:getChannelProfile")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(JsonConvert.SerializeObject(request))
                .Respond(HttpStatusCode.OK, JsonContent.Create(new { profile_name = "Marty McFly FB" }));

            var channelProfile = await Conversation.Contacts.GetChannelProfile(new GetChannelProfileRequest
            {
                AppId = AppId,
                Recipient = new ContactRecipient { ContactId = ContactId001 },
                Channel = ChannelProfileConversationChannel.Messenger
            });

            channelProfile.Should().NotBeNull();
            channelProfile.ProfileName.Should().Be("Marty McFly FB");
        }
    }
}

