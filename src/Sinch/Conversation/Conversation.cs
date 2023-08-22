using System;
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
    public interface IConversation
    {
        /// <summary>
        ///     To start sending messages you must have a Conversation API
        ///     <see href="https://dashboard.sinch.com/convapi/app">app</see>.
        ///     The app holds information about the channel credentials and registered webhooks
        ///     to which the API delivers callbacks such as message delivery receipts and contact messages.
        ///     If you don't already have an app please follow the instructions in the getting started guide available
        ///     in the <see href="https://dashboard.sinch.com/convapi/getting-started">Sinch Dashboard</see>
        ///     to create one.
        /// </summary>
        IMessages Messages { get; }
    }

    internal class Conversation : IConversation
    {
        internal Conversation(string projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Messages = new Messages.Messages(projectId, baseAddress, loggerFactory?.Create<Messages.Messages>(),
                http);
        }

        /// <summary>
        ///     Access a Messages API.
        /// </summary>
        public IMessages Messages { get; }
    }
}
