using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Messages;
using Xunit;

namespace Sinch.Tests.Conversation
{
    public class ContactsTests : ConversationTestBase
    {
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
    }
}
