using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Voice.Callout
{
    /// <summary>
    ///     A callout is a call made to a phone number or app using the API.<br/><br/>
    ///     Makes a call out to a phone number. The types of callouts currently supported are conference callouts,
    ///     text-to-speech callouts, and custom callouts. The custom callout is the most flexible,
    ///     but text-to-speech and conference callouts are more convenient.
    /// </summary>
    public interface ICallout
    {
        /// <summary>
        ///     The text-to-speech callout calls a phone number and
        ///     plays a synthesized text messages or pre-recorded sound files.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Tts(TtsCalloutRequest request, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///     The conference callout calls a phone number or a user.
        ///     When the call is answered, it's connected to a conference room.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Conference(ConferenceCalloutRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     The custom callout, the server initiates a call from the servers that can be controlled
        ///     by specifying how the call should progress at each call event.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CalloutResponse> Custom(CustomCalloutRequest request, CancellationToken cancellationToken = default);
    }

    public class Callout
    {
        
    }
}
