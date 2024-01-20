using Sinch.Core;
using Sinch.Logger;
using System;

namespace Sinch.Faxes
{
    public class FaxClient
    {
        private readonly string _projectId;
        private readonly Uri _uri;
        private readonly LoggerFactory _loggerFactory;
        private readonly Http _httpClient;

        internal FaxClient(string projectId, Uri uri, Logger.LoggerFactory _loggerFactory, Core.Http httpClient)
        {
            this._projectId = projectId;
            this._uri = uri;
            this._loggerFactory = _loggerFactory;
            this._httpClient = httpClient;
            Faxes = new Faxes(_projectId, _uri, this._loggerFactory?.Create<Faxes>(), _httpClient);
            // Services = new Services(projectId, uri, loggerFactory?.Create<Services>(), httpSnakeCase);
        }
        public Faxes Faxes { get; }
        //   public Services Services { get; }
    }
}
