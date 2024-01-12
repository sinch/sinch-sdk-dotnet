using Sinch.Core;
using Sinch.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Faxes
{
    public class FaxClient
    {
        private readonly string projectId;
        private readonly Uri uri;
        private readonly LoggerFactory loggerFactory;
        private readonly Http httpSnakeCase;

        internal FaxClient(string projectId, Uri uri, Logger.LoggerFactory _loggerFactory, Core.Http httpSnakeCase) {
            this.projectId = projectId;
            this.uri = uri;
            loggerFactory = _loggerFactory;
            this.httpSnakeCase = httpSnakeCase;
            Faxes = new Faxes(projectId, uri, loggerFactory?.Create<Faxes>(), httpSnakeCase);
           // Services = new Services(projectId, uri, loggerFactory?.Create<Services>(), httpSnakeCase);
        }
        public Faxes Faxes { get; }
     //   public Services Services { get; }
    }
}
