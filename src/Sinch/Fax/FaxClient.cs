using System;
using Sinch.Core;
using Sinch.Fax.Faxes;
using Sinch.Logger;

namespace Sinch.Fax
{
    /// <summary>
    ///     Our Fax API offers collision avoidance features, business integrations,
    ///     and a pay-as-you-go model suited for large scalability, designed with you, the developer, in mind.
    /// </summary>
    public interface ISinchFax
    {
        /// <inheritdoc cref="ISinchFaxFaxes" />
        public ISinchFaxFaxes Faxes { get; }
    }

    internal class Fax : ISinchFax
    {
        internal Fax(string projectId, Uri baseAddress, LoggerFactory? loggerFactory, IHttp http)
        {
            Faxes = new FaxesClient(projectId, baseAddress, loggerFactory?.Create<ISinchFaxFaxes>(), http);
        }

        public ISinchFaxFaxes Faxes { get; }
    }
}
