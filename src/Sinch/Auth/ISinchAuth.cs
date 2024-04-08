using System.Threading.Tasks;

namespace Sinch.Auth
{
    public interface ISinchAuth
    {
        /// <summary>
        ///     Get an auth token.
        /// </summary>
        /// <param name="force">Set to `true` to invalidate cached token, if any.</param>
        /// <returns>A string representation of an auth token</returns>
        Task<string> GetAuthToken(bool force = false);

        public string Scheme { get; }
    }
}
