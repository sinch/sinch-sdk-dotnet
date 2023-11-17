using System;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Callout;

namespace Sinch.Voice
{
    public interface ISinchVoiceClient
    {
        ICallout Callout { get; }
    }
    
    internal class SinchVoiceClient : ISinchVoiceClient
    {
        public SinchVoiceClient(Uri baseAddress, LoggerFactory loggerFactory,
            IHttp http)
        {
            
        }
        
        public ICallout Callout { get; }
    }
}
