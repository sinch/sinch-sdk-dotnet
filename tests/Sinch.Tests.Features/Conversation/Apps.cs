using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Conversation;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Apps.Create;
using Sinch.Conversation.Apps.Credentials;
using Sinch.Conversation.Apps.Update;
using Sinch.Conversation.Common;

namespace Sinch.Tests.Features.Conversation
{
    [Binding]
    public class Apps
    {
        private const string AppId = "01W4FFL35P4NC4K35CONVAPP001";
        private ISinchConversationApps _apps;
        private App _app;
        private List<App> _appsList;
        private bool _deleteCompleted;

        [Given(@"the Conversation service ""Apps"" is available")]
        public void GivenTheConversationServiceAppsIsAvailable()
        {
            _apps = Utils.SinchConversationClient().Apps;
        }

        [When(@"I send a request to create an app")]
        public async Task WhenISendARequestToCreateAnApp()
        {
            _app = await _apps.Create(new CreateAppRequest
            {
                DisplayName = "E2E Conversation App",
                ChannelCredentials =
                [
                    new ConversationChannelCredentials(new StaticBearerCredentials
                    {
                        ClaimedIdentity = "SpaceMonkeySquadron",
                        Token = "00112233445566778899aabbccddeeff"
                    })
                    {
                        Channel = ConversationChannel.Sms
                    }
                ]
            });
        }

        [Then(@"the conversation app is created")]
        public void ThenTheConversationAppIsCreated()
        {
            AssertCommonAppFields(_app);
            _app.ChannelCredentials![0].State!.Status.Should().Be(ChannelIntegrationStatus.Pending);
        }

        [When(@"I send a request to list all the apps")]
        public async Task WhenISendARequestToListAllTheApps()
        {
            _appsList = (await _apps.List()).ToList();
        }

        [Then(@"the apps list contains {int} apps")]
        public void ThenTheAppsListContainsApps(int count)
        {
            _appsList.Should().HaveCount(count);

            var app1 = _appsList[0];
            AssertCommonAppFields(app1);
            app1.DisplayName.Should().Be("E2E Conversation App");
            app1.ChannelCredentials![0].State!.Status.Should().Be(ChannelIntegrationStatus.Active);

            var app2 = _appsList[1];
            app2.Id.Should().Be("01W4FFL35P4NC4K35CONVAPP002");
        }

        [When(@"I send a request to retrieve an app")]
        public async Task WhenISendARequestToRetrieveAnApp()
        {
            _app = await _apps.Get(AppId);
        }

        [Then(@"the response contains the app details")]
        public void ThenTheResponseContainsTheAppDetails()
        {
            AssertCommonAppFields(_app);
            _app.DisplayName.Should().Be("E2E Conversation App");
            _app.ChannelCredentials![0].State!.Status.Should().Be(ChannelIntegrationStatus.Active);
        }

        [When(@"I send a request to update an app")]
        public async Task WhenISendARequestToUpdateAnApp()
        {
            _app = await _apps.Update(AppId, new UpdateAppRequest
            {
                DisplayName = "Updated name"
            });
        }

        [Then(@"the response contains the app details with updated properties")]
        public void ThenTheResponseContainsTheAppDetailsWithUpdatedProperties()
        {
            AssertCommonAppFields(_app);
            _app.DisplayName.Should().Be("Updated name");
        }

        [When(@"I send a request to delete an app")]
        public async Task WhenISendARequestToDeleteAnApp()
        {
            await _apps.Delete(AppId);
            _deleteCompleted = true;
        }

        [Then(@"the delete app response contains no data")]
        public void ThenTheDeleteAppResponseContainsNoData()
        {
            _deleteCompleted.Should().BeTrue();
        }
        
        private void AssertCommonAppFields(App app)
        {
            app.Id.Should().Be(AppId);
            app.ChannelCredentials.Should().HaveCount(1);
            app.ConversationMetadataReportView.Should().Be(ConversationMetadataReportView.None);
            app.ProcessingMode.Should().Be(ProcessingMode.Conversation);
        }
    }
}
