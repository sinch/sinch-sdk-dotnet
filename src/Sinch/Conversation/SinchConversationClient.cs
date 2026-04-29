using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Contacts;
using Sinch.Conversation.Conversations;
using Sinch.Conversation.EventDestinations;
using Sinch.Conversation.Events;
using Sinch.Conversation.Messages;
using Sinch.Conversation.SinchEvents;
using Sinch.Conversation.Transcoding;
using Sinch.Conversation.TemplatesV2;
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

        /// <inheritdoc cref="ISinchConversationEventDestinations" />
        ISinchConversationEventDestinations EventDestinations { get; }

        /// <inheritdoc cref="IConversationSinchEvents" />
        IConversationSinchEvents SinchEvents { get; }

        /// <inheritdoc cref="ISinchConversationEvents" />
        ISinchConversationEvents Events { get; }

        /// <inheritdoc cref="ISinchConversationTranscoding" />
        ISinchConversationTranscoding Transcoding { get; }

        /// <inheritdoc cref="ISinchConversationCapabilities" />
        ISinchConversationCapabilities Capabilities { get; }

        /// <inheritdoc cref="ISinchConversationTemplates" />
        ISinchConversationTemplates Templates { get; }

        /// <summary>
        ///     For internal use, JsonSerializerOption to be utilized for serialization and deserialization of all Conversation models
        /// </summary>
        internal JsonSerializerOptions JsonSerializerOptions { get; }
    }

    /// <inheritdoc />
    internal sealed class SinchConversationClient : ISinchConversation
    {
        internal static readonly JsonSerializerOptions JsonSerializerOptionsInner =
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

        internal SinchConversationClient(string projectId, Uri conversationBaseAddress, Uri templatesBaseAddress
            , LoggerFactory? loggerFactory, Lazy<IHttp> http)
        {
            JsonSerializerOptions = JsonSerializerOptionsInner;
            Messages = new Messages.Messages(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationMessages>(),
                http);
            Apps = new Apps.Apps(projectId, conversationBaseAddress, loggerFactory?.Create<Apps.Apps>(), http);
            Contacts = new Contacts.Contacts(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationContacts>(), http);
            Conversations = new ConversationsClient(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationConversations>(), http);
            EventDestinations = new EventDestinations.EventDestinations(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationEventDestinations>(), http);
            SinchEvents = new ConversationSinchEvents(JsonSerializerOptionsInner,
                loggerFactory?.Create<IConversationSinchEvents>());
            Events = new Events.Events(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationEvents>(), http);
            Transcoding = new Transcoding.Transcoding(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationTranscoding>(), http);
            Capabilities = new Capabilities(projectId, conversationBaseAddress,
                loggerFactory?.Create<ISinchConversationCapabilities>(), http);
            Templates = new TemplatesV2.Templates(projectId, templatesBaseAddress,
                loggerFactory?.Create<ISinchConversationTemplates>(), http);
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
        public ISinchConversationEventDestinations EventDestinations { get; }

        /// <inheritdoc />
        public IConversationSinchEvents SinchEvents { get; }

        /// <inheritdoc />
        public ISinchConversationEvents Events { get; }

        /// <inheritdoc />
        public ISinchConversationTranscoding Transcoding { get; }

        /// <inheritdoc />
        public ISinchConversationCapabilities Capabilities { get; }

        /// <inheritdoc />
        public ISinchConversationTemplates Templates { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
