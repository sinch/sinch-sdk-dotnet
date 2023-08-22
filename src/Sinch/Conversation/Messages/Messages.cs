using System;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Messages
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
    public interface IMessages
    {
        Task Send();

        Task Get();

        Task Delete();

        Task List();
    }

    /// <inheritdoc />
    internal class Messages : IMessages
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<Messages> _logger;
        private readonly string _projectId;

        public Messages(string projectId, Uri baseAddress, ILoggerAdapter<Messages> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        public Task Get()
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task List()
        {
            throw new NotImplementedException();
        }

        public Task Send()
        {
            throw new NotImplementedException();
        }
    }
}
