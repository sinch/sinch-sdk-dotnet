using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Conversation.Contact.Create;
using Sinch.Conversation.Messages;
using Xunit;
using Contact = Sinch.Conversation.Contact.Contact;

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
                Language = "EN_US",
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
                Language = "EN_US",
                Metadata = "no"
            });
        }
    }
}
