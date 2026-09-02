using System.Threading.Tasks;
using Reqnroll;
using Sinch.Numbers;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class EmergencyAddress
    {
        private ISinchNumbers _sinchNumbers;

        [Given(@"the Numbers service is available to handle emergency addresses")]
        public void GivenTheNumbersServiceIsAvailableToHandleEmergencyAddresses()
        {
            _sinchNumbers = Utils.SinchNumbersClient();
        }

        [When(@"I send a request to validate an emergency address")]
        public async Task WhenISendARequestToValidateAnEmergencyAddress()
        {
            // TODO: Support emergency address
        }

        [Then(@"the response contains the corrected address")]
        public void ThenTheResponseContainsTheCorrectedAddress()
        {
            // TODO: Support emergency address
        }

        [When(@"I send a request to validate an approximate emergency address")]
        public async Task WhenISendARequestToValidateAnApproximateEmergencyAddress()
        {
            // TODO: Support emergency address
        }

        [Then(@"the response contains the candidate address")]
        public void ThenTheResponseContainsTheCandidateAddress()
        {
            // TODO: Support emergency address
        }

        [When(@"I send a request to provision an emergency address")]
        public async Task WhenISendARequestToProvisionAnEmergencyAddress()
        {
            // TODO: Support emergency address
        }

        [Then(@"the response contains the provisioned emergency address")]
        public void ThenTheResponseContainsTheProvisionedEmergencyAddress()
        {
            // TODO: Support emergency address
        }

        [When(@"I send a request to deprovision an emergency address")]
        public async Task WhenISendARequestToDeprovisionAnEmergencyAddress()
        {
            // TODO: Support emergency address
        }

        [Then(@"the response indicates successful deprovisioning")]
        public void ThenTheResponseIndicatesSuccessfulDeprovisioning()
        {
            // TODO: Support emergency address
        }

        [When(@"I send a request to get the emergency address for a number")]
        public async Task WhenISendARequestToGetTheEmergencyAddressForANumber()
        {
            // TODO: Support emergency address
        }

        [Then(@"the response contains the provisioned emergency address for the phone number")]
        public void ThenTheResponseContainsTheProvisionedEmergencyAddressForThePhoneNumber()
        {
            // TODO: Support emergency address
        }
    }
}
