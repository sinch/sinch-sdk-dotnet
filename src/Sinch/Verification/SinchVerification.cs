using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Verification.Start;

namespace Sinch.Verification
{
    public interface ISinchVerification
    {
        Task<VerificationResponseBase> Start();
    }
    public class SinchVerification : ISinchVerification
    {
        
        public Task<VerificationResponseBase> Start()
        {
            throw new System.NotImplementedException();
        }
    }
}
