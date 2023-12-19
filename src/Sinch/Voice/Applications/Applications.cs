using System.Threading;
using System.Threading.Tasks;
using Sinch.Voice.Applications.GetCallbackUrls;
using Sinch.Voice.Applications.GetNumbers;
using Sinch.Voice.Applications.QueryNumber;
using Sinch.Voice.Applications.UnassignNumbers;
using Sinch.Voice.Applications.UpdateCallbackUrls;
using Sinch.Voice.Applications.UpdateNumbers;

namespace Sinch.Voice.Applications
{
    /// <summary>
    ///     You can use the API to manage features of applications in your project.
    /// </summary>
    public interface ISinchVoiceApplications
    {
        /// <summary>
        ///     Get information about your numbers. It returns a list of numbers that you own, as well as their capability (voice
        ///     or SMS). For the ones that are assigned to an app, it returns the application key of the app.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetNumbersResponse> GetNumbers(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Assign a number or a list of numbers to an application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AssignNumbers(UpdateNumbersRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Un-assign a number from an application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UnassignNumbers(UnassignNumbersRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns any callback URLs configured for the specified application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CallbackUrls> GetCallbackUrls(string applicationKey,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update the configured callback URLs for the specified application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CallbackUrls> UpdateCallbackUrls(UpdateCallbackUrlsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns information about the requested number.
        /// </summary>
        /// <param name="number">The phone number you want to query.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryNumberResponse> QueryNumber(string number, CancellationToken cancellationToken = default);
    }
}
