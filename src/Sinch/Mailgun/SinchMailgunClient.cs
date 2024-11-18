using System;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Mailgun
{
    /// <summary>
    ///     The Mailgun API is part of the Sinch family and enables you to send, track, and receive email effortlessly. 
    /// </summary>
    public interface ISinchMailgunClient
    {
        // TBD: add domains
    }

    /// <inheritdoc />
    internal class SinchMailgunClient : ISinchMailgunClient
    {
        public SinchMailgunClient(Uri baseUrl, Http http, LoggerFactory? loggerFactory = null)
        {
            // TBD: implement domains 
        }
    }
}
