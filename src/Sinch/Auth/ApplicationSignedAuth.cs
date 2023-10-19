using System;
using System.Threading.Tasks;

namespace Sinch.Auth
{
    public class ApplicationSignedAuth : IAuth
    {
        public Task<string> GetAuthValue(bool force = false)
        {

            // TODO: implement application signed auth
            throw new NotImplementedException();
        }

        public string Scheme { get; }
    }
}
