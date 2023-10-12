using System.Threading.Tasks;
using Sinch.Core;

namespace Sinch.Verification
{
    public interface ISinchVerification
    {
        Task<AnyOf<SmsResponse, FlashCallResponse, PhoneCallResponse, DataResponse>> Start();
    }
    public class SinchVerification : ISinchVerification
    {
        public Task<AnyOf<SmsResponse, FlashCallResponse, PhoneCallResponse, DataResponse>> Start()
        {
            throw new System.NotImplementedException();
        }
    }
}
