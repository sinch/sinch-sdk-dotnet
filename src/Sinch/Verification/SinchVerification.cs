using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Verification.Start;

namespace Sinch.Verification
{
    public interface ISinchVerification
    {
        Task<IVerificationResponse> Start();
    }
    public class SinchVerification : ISinchVerification
    {
        
        public Task<IVerificationResponse> Start()
        {
            throw new System.NotImplementedException();
        }
    }
}
