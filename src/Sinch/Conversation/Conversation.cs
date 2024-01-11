using System;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Messages;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation
{
    /// <summary>
    ///     Send and receive messages globally over SMS, RCS, WhatsApp, Viber Business,
    ///     Facebook messenger and other popular channels using the Sinch Conversation API.<br/><br/>
    /// 
    ///     The Conversation API endpoint uses built-in transcoding to give you the power of conversation across all
    ///     supported channels and, if required, full control over channel specific features.
    /// </summary>
    public interface ISinchConversation
    {
        /// <inheritdoc cref="ISinchConversationMessages" />
        ISinchConversationMessages Messages { get; }

        /// <inheritdoc cref="ISinchConversationApps" />
        ISinchConversationApps Apps { get; }
    }

    /// <inheritdoc />
    internal class Conversation : ISinchConversation
    {
        internal Conversation(string projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Messages = new Messages.Messages(projectId, baseAddress, loggerFactory?.Create<Messages.Messages>(),
                http);
            Apps = new Apps.Apps(projectId, baseAddress, loggerFactory?.Create<Apps.Apps>(), http);
        }

        /// <inheritdoc />
        public ISinchConversationMessages Messages { get; }

        /// <inheritdoc />
        public ISinchConversationApps Apps { get; }
    }
}
