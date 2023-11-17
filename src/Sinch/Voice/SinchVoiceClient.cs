using System;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Voice
{
    public interface ISinchVoiceClient
    {
        
    }
    
    internal class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            
        }
    }
}
