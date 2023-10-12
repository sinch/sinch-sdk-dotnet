using System.Threading.Tasks;

namespace Sinch.Auth
{
    public interface IAuth
    {
        /// <summary>
        /// Fetches a token, which is cached by ITokenManager, from OAuth endpoint.
        /// </summary>
        /// <param name="force">Set as true to drop cached token and fetch a fresh one.</param>
        /// <returns>OAuth bearer token.</returns>
        Task<string> GetToken(bool force = false);

        public string Scheme { get; }
    }
}
