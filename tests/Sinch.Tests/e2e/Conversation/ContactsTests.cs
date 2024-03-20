using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;
using Sinch.Conversation.Messages;
using Xunit;
using Contact = Sinch.Conversation.Contacts.Contact;

namespace Sinch.Tests.e2e.Conversation
{
    public class ContactsTests : TestBase
    {
        [Fact]
        public async Task GetContact()
        {
            var response = await SinchClientMockServer.Conversation.Contacts.Get("123ABC");

            response.Should().BeEquivalentTo(new Contact
            {
                ChannelIdentities = new List<ChannelIdentity>()
                {
                    new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Telegram,
                        Identity = "@hola",
                        AppId = string.Empty,
                    }
                },
                ChannelPriority = new List<ConversationChannel>()
                {
                    ConversationChannel.WhatsApp
                },
                DisplayName = "New Contact",
                Email = "new.contact@email.com",
                ExternalId = "yes",
                Id = "01HKWT3XRVH6RP17S8KSBC4PYR",
                Language = ConversationLanguage.EnglishUS,
                Metadata = "no"
            });
        }

        [Fact]
        public async Task CreateContact()
        {
            var response = await SinchClientMockServer.Conversation.Contacts.Create(new CreateContactRequest()
            {
                ChannelIdentities = new List<ChannelIdentity>()
                {
                    new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Sms,
                        Identity = "+49123123123",
                    },
                    new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Viber,
                        Identity = "/ret",
                        AppId = "15"
                    }
                },
                Language = "ZH_CN",
                ChannelPriority = new List<ConversationChannel>()
                {
                    ConversationChannel.Viber,
                    ConversationChannel.Sms
                },
                Email = "oi@mail.org",
                DisplayName = "one",
                Metadata = "rogue",
                ExternalId = "plan"
            });

            response.Should().BeEquivalentTo(new Contact
            {
                ChannelIdentities = new List<ChannelIdentity>()
                {
                    new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Telegram,
                        Identity = "@hola",
                        AppId = string.Empty,
                    }
                },
                ChannelPriority = new List<ConversationChannel>()
                {
                    ConversationChannel.WhatsApp
                },
                DisplayName = "New Contact",
                Email = "new.contact@email.com",
                ExternalId = "yes",
                Id = "01HKWT3XRVH6RP17S8KSBC4PYR",
                Language = ConversationLanguage.EnglishUS,
                Metadata = "no"
            });
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockServer.Conversation.Contacts.List(new ListContactsRequest()
            {
                Channel = ConversationChannel.Instagram,
                ExternalId = "@nice",
                Identity = "nice",
                PageSize = 10,
                PageToken = "tin",
            });

            response.Contacts.Should().HaveCount(2);
            response.NextPageToken.Should().BeEquivalentTo("next");
        }

        [Fact]
        public async Task ListAuto()
        {
            var response = SinchClientMockServer.Conversation.Contacts.ListAuto(new ListContactsRequest()
            {
                Channel = ConversationChannel.Instagram,
                ExternalId = "@nice",
                Identity = "nice",
                PageSize = 10,
                PageToken = "tin",
            });
            var counter = 0;
            await foreach (var contact in response)
            {
                contact.Should().NotBeNull();
                counter++;
            }

            counter.Should().Be(4);
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => SinchClientMockServer.Conversation.Contacts.Delete("123ABC");
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetChannelProfile()
        {
            var response = await SinchClientMockServer.Conversation.Contacts.GetChannelProfile(
                new GetChannelProfileRequest()
                {
                    AppId = "123",
                    Channel = ChannelProfileConversationChannel.Line,
                    Recipient = new Identified()
                    {
                        IdentifiedBy = new IdentifiedBy()
                        {
                            ChannelIdentities = new List<ChannelIdentity>()
                            {
                                new ChannelIdentity()
                                {
                                    Identity = "a",
                                    Channel = ConversationChannel.Rcs,
                                },
                                new ChannelIdentity()
                                {
                                    Identity = "b",
                                    Channel = ConversationChannel.Messenger,
                                }
                            }
                        }
                    }
                });
            response.Should().BeEquivalentTo(new ChannelProfile()
            {
                ProfileName = "beast"
            });
        }

        [Fact]
        public async Task Update()
        {
            var contact = new Contact()
            {
                Id = "123ABC",
                DisplayName = "Unknown",
                ExternalId = "",
                Email = "new.contact@email.com",
                Metadata = "",
                Language = ConversationLanguage.EnglishUS,
                ChannelPriority = new List<ConversationChannel>()
                {
                    ConversationChannel.Telegram
                },
                ChannelIdentities = new List<ChannelIdentity>()
                {
                    new ChannelIdentity()
                    {
                        Channel = ConversationChannel.Telegram,
                        Identity = "@ora",
                        AppId = "",
                    }
                }
            };
            var response = await SinchClientMockServer.Conversation.Contacts.Update(contact);
            response.Should().BeEquivalentTo(contact);
        }


        [Fact]
        public async Task Merge()
        {
            var response = await SinchClientMockServer.Conversation.Contacts.Merge("123ABC", "456EDF");
            response.Should().NotBeNull();
        }
    }
}
