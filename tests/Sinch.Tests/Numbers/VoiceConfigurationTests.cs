using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Numbers;
using Sinch.Numbers.VoiceConfigurations;
using Xunit;

namespace Sinch.Tests.Numbers
{
    public class VoiceConfigurationTests : NumberTestBase
    {
        private class Container
        {
            [JsonConverter(typeof(VoiceConfigurationConverter))]
            public VoiceConfiguration VoiceConfiguration { get; set; }
        }

        [Fact]
        public void ShouldSerializeVoiceRtcConfiguration()
        {
            var config = new VoiceRtcConfiguration()
            {
                AppId = "app id value",
            };
            var jsonString = SerializeAsNumbersClient(new Container()
            {
                VoiceConfiguration = config
            });

            Helpers.AssertJsonEqual(Helpers.LoadResources("Numbers/RtcVoiceSerializationExpected.json"), jsonString);
        }

        [Fact]
        public void ShouldSerializeVoiceFaxConfiguration()
        {
            var config = new VoiceFaxConfiguration()
            {
                ServiceId = "service id value",
            };
            var jsonString = SerializeAsNumbersClient(new Container()
            {
                VoiceConfiguration = config
            });

            Helpers.AssertJsonEqual(Helpers.LoadResources("Numbers/FaxVoiceSerializationExpected.json"), jsonString);
        }

        [Fact]
        public void ShouldSerializeVoiceEstConfiguration()
        {
            var config = new VoiceEstConfiguration()
            {
                TrunkId = "trunk id value",
            };
            var jsonString = SerializeAsNumbersClient(new Container()
            {
                VoiceConfiguration = config
            });

            Helpers.AssertJsonEqual(Helpers.LoadResources("Numbers/EstVoiceSerializationExpected.json"), jsonString);
        }

        [Fact]
        public void ShouldDeserializeVoiceEstConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/EstVoiceResponse.json"));
            var config = new VoiceEstConfiguration()
            {
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                TrunkId = "trunk id value",
                ScheduledVoiceProvisioning = new ScheduledVoiceEstProvisioning()
                {
                    TrunkId = "trunk id value",
                    Status = ProvisioningStatus.Waiting,
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                },
            };
            ((VoiceConfiguration)config).ScheduledVoiceProvisioning = config.ScheduledVoiceProvisioning;
            obj.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = config
            });
        }

        [Fact]
        public void ShouldDeserializeVoiceFaxConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/FaxVoiceResponse.json"));
            var config = new VoiceFaxConfiguration()
                {
                    ServiceId = "service id value",
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                    ScheduledVoiceProvisioning = new ScheduledVoiceFaxProvisioning()
                    {
                        ServiceId = "service id value",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                    }
                };
            ((VoiceConfiguration)config).ScheduledVoiceProvisioning = config.ScheduledVoiceProvisioning;
            obj.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = config
            });
        }

        [Fact]
        public void ShouldDeserializeVoiceRtcConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));
            var config = new VoiceRtcConfiguration()
            {
                AppId = "app id value",
                LastUpdatedTime = Helpers.ParseUtc("2024-06-30T07:08:09.100Z"),
                ScheduledVoiceProvisioning = new ScheduledVoiceRtcProvisioning()
                {
                    AppId = "app id value",
                    Status = ProvisioningStatus.Waiting,
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                }
            };
            ((VoiceConfiguration)config).ScheduledVoiceProvisioning = config.ScheduledVoiceProvisioning;
            obj.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = config
            });
        }
    }
}
