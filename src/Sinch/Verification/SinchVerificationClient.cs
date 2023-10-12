using System;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Verification
{
    public interface ISinchVerificationClient
    {
        
    }

    internal class SinchVerificationClient : ISinchVerificationClient
    {
        private readonly string _appKey;
        private readonly string _appSecret;

        internal SinchVerificationClient(string appKey, string appSecret, Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            
        }
    }
}
