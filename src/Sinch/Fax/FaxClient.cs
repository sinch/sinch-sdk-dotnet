using System;
using Sinch.Core;
using Sinch.Fax.Faxes;
using Sinch.Logger;

namespace Sinch.Fax
{
    public interface ISinchFaxClient
    {
        public ISinchFaxFaxes Faxes { get; }
    }

    public class FaxClient : ISinchFaxClient
    {
        internal FaxClient(string projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Faxes = new FaxesClient(projectId, baseAddress, loggerFactory?.Create<ISinchFaxFaxes>(), http);
        }

        public ISinchFaxFaxes Faxes { get; }
    }
}
