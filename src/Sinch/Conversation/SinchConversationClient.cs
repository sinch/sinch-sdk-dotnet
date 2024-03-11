using System;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Conversations;
using Sinch.Conversation.Messages;
using Sinch.Conversation.TemplatesV2;
using Sinch.Conversation.Webhooks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation
{
    /// <summary>
    ///     Send and receive messages globally over SMS, RCS, WhatsApp, Viber Business,
    ///     Facebook messenger and other popular channels using the Sinch Conversation API.<br /><br />
    ///     The Conversation API endpoint uses built-in transcoding to give you the power of conversation across all
    ///     supported channels and, if required, full control over channel specific features.
    /// </summary>
    public interface ISinchConversation
    {
        /// <inheritdoc cref="ISinchConversationMessages" />
        ISinchConversationMessages Messages { get; }

        /// <inheritdoc cref="ISinchConversationApps" />
        ISinchConversationApps Apps { get; }

        /// <inheritdoc cref="ISinchConversationContacts" />
        ISinchConversationContacts Contacts { get; }

        /// <inheritdoc cref="ISinchConversationConversations" />
        ISinchConversationConversations Conversations { get; }

        /// <inheritdoc cref="ISinchConversationWebhooks" />
        ISinchConversationWebhooks Webhooks { get; }

        /// <inheritdoc cref="ISinchConversationTemplatesV2" />
        ISinchConversationTemplatesV2 TemplatesV2 { get; }
    }

    /// <inheritdoc />
    internal class SinchConversationClient : ISinchConversation
    {
        internal SinchConversationClient(string projectId, Uri conversationBaseAddress, Uri templatesBaseAddress
            , LoggerFactory loggerFactory, IHttp http)
        {
            Messages = new Messages.Messages(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationMessages>(),
                http);
            Apps = new Apps.Apps(projectId, conversationBaseAddress, loggerFactory?.Create<Apps.Apps>(), http);
            Contacts = new Contacts.Contacts(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationContacts>(), http);
            Conversations = new ConversationsClient(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationConversations>(), http);
            Webhooks = new Webhooks.Webhooks(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationWebhooks>(), http);
            TemplatesV2 = new TemplatesV2.TemplatesV2(projectId, templatesBaseAddress,
                loggerFactory?.Create<ISinchConversationTemplatesV2>(), http);
        }

        /// <inheritdoc />
        public ISinchConversationMessages Messages { get; }

        /// <inheritdoc />
        public ISinchConversationApps Apps { get; }

        /// <inheritdoc />
        public ISinchConversationContacts Contacts { get; }

        /// <inheritdoc />
        public ISinchConversationConversations Conversations { get; }

        public ISinchConversationWebhooks Webhooks { get; }

        public ISinchConversationTemplatesV2 TemplatesV2 { get; }
    }
}
