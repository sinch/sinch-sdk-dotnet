using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice;
using Sinch.Voice.Calls;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Calls
    {
        private ISinchVoiceClient _sinchVoiceClient;
        private Call _callInformation;

        [Given(@"the Voice service ""(.*)"" is available")]
        public void GivenTheVoiceServiceIsAvailable(string calls)
        {
            _sinchVoiceClient = new SinchClient(new SinchClientConfiguration()
            {
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "appKey",
                    AppSecret = "BeIukql3pTKJ8RGL5zo0DA==",
                    VoiceUrlOverride = "http://localhost:3019",
                }
            }).Voice;
        }

        [When(@"I send a request to get a call's information")]
        public async Task WhenISendARequestToGetACallsInformation()
        {
            _callInformation = await _sinchVoiceClient.Calls.Get("1ce0ffee-ca11-ca11-ca11-abcdef000003");
        }

        [Then(@"the response contains the information about the call")]
        public void ThenTheResponseContainsTheInformationAboutTheCall()
        {
            _callInformation.Should().BeEquivalentTo(new Call()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000003",
            });
        }
    }
}
