using System;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Conversations;
using Sinch.Conversation.Events;
using Sinch.Conversation.Messages;
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

        /// <inheritdoc cref="ISinchConversationEvents" />
        ISinchConversationEvents Events { get; }
    }

    /// <inheritdoc />
    internal class SinchConversationClient : ISinchConversation
    {
        internal SinchConversationClient(string projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Messages = new Messages.Messages(projectId, baseAddress,
                loggerFactory?.Create<ISinchConversationMessages>(),
                http);
            Apps = new Apps.Apps(projectId, baseAddress, loggerFactory?.Create<Apps.Apps>(), http);
            Contacts = new Contacts.Contacts(projectId, baseAddress,
                loggerFactory?.Create<ISinchConversationContacts>(), http);
            Conversations = new ConversationsClient(projectId, baseAddress,
                loggerFactory?.Create<ISinchConversationConversations>(), http);
            Webhooks = new Webhooks.Webhooks(projectId, baseAddress,
                loggerFactory?.Create<ISinchConversationWebhooks>(), http);
            Events = new Events.Events(projectId, baseAddress, loggerFactory?.Create<ISinchConversationEvents>(), http);
        }

        /// <inheritdoc />
        public ISinchConversationMessages Messages { get; }

        /// <inheritdoc />
        public ISinchConversationApps Apps { get; }

        /// <inheritdoc />
        public ISinchConversationContacts Contacts { get; }

        /// <inheritdoc />
        public ISinchConversationConversations Conversations { get; }

        /// <inheritdoc />
        public ISinchConversationWebhooks Webhooks { get; }

        /// <inheritdoc />
        public ISinchConversationEvents Events { get; }
    }
}
