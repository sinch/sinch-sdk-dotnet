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
        // TODO: remove this possibility in 2.0
        public void ShouldSerializeVoiceConfigPlainForBackwardCompatibility()
        {
            var voiceConfig = new VoiceConfiguration()
            {
                AppId = "app id value"
            };
            var jsonString = SerializeAsNumbersClient(new Container()
            {
                VoiceConfiguration = voiceConfig
            });
            Helpers.AssertJsonEqual(Helpers.LoadResources("Numbers/RtcVoiceSerializationExpected.json"), jsonString);
        }

        [Fact]
        // TODO: remove this possibility in 2.0
        public void ShouldDeserializeVoiceConfigPlainForBackwardCompatibility()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));

            obj.VoiceConfiguration.Type.Should().BeEquivalentTo(VoiceApplicationType.Rtc);
#pragma warning disable CS0618 // Type or member is obsolete
            obj.VoiceConfiguration.AppId.Should().BeEquivalentTo("app id value");
            obj.VoiceConfiguration.ScheduledVoiceProvisioning!.AppId.Should().BeEquivalentTo("app id value");
#pragma warning restore CS0618 // Type or member is obsolete
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

            var prov = new ScheduledVoiceEstProvisioning()
            {
                TrunkId = "trunk id value",
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            };
            obj.VoiceConfiguration.Should().BeEquivalentTo(
                new VoiceEstConfiguration()
                {
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                    TrunkId = "trunk id value",
                    ScheduledVoiceProvisioning = prov,
                }
            );
            // cast to expected type to test exclusively 
            obj.VoiceConfiguration.ScheduledVoiceProvisioning.As<ScheduledVoiceEstProvisioning>().Should()
                .BeEquivalentTo(prov);
        }

        [Fact]
        public void ShouldDeserializeVoiceFaxConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/FaxVoiceResponse.json"));

            var scheduledVoiceFaxProvisioning = new ScheduledVoiceFaxProvisioning()
            {
                ServiceId = "service id value",
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            };
            obj.VoiceConfiguration.Should().BeEquivalentTo(
                new VoiceFaxConfiguration()
                {
                    ServiceId = "service id value",
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                    ScheduledVoiceProvisioning = scheduledVoiceFaxProvisioning
                }
            );
            // cast to expected type to test exclusively 
            obj.VoiceConfiguration.ScheduledVoiceProvisioning.As<ScheduledVoiceFaxProvisioning>().Should()
                .BeEquivalentTo(
                    scheduledVoiceFaxProvisioning);
        }

        [Fact]
        public void ShouldDeserializeVoiceRtcConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));
            var expectedProvisioning = new ScheduledVoiceRtcProvisioning()
            {
                AppId = "app id value",
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            };
            var expected = new VoiceRtcConfiguration()
            {
                AppId = "app id value",
                LastUpdatedTime = Helpers.ParseUtc("2024-06-30T07:08:09.100Z"),
                ScheduledVoiceProvisioning = expectedProvisioning
            };
#pragma warning disable CS0618 // Type or member is obsolete
            // backward compatibility for old plain ScheduledVoiceProvisioning and VoiceConfiguration
            (expected as VoiceConfiguration).AppId = "app id value";
            (expected as VoiceConfiguration).ScheduledVoiceProvisioning = new ScheduledVoiceProvisioning()
            {
                AppId = "app id value",
                Type = VoiceApplicationType.Rtc,
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            };
#pragma warning restore CS0618 // Type or member is obsolete
            obj.VoiceConfiguration.Should().BeEquivalentTo(expected);
            // if inheritance in play, to check object correctly with BeEquivalentTo,
            // it should have a cast to desired type to be checked
            obj.VoiceConfiguration.ScheduledVoiceProvisioning.As<ScheduledVoiceRtcProvisioning>().Should()
                .BeEquivalentTo(
                    new ScheduledVoiceRtcProvisioning()
                    {
                        AppId = "app id value",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                    });
        }

        [Fact]
        public void ShouldDeserializeVoiceRtcConfigurationButWithFaxProvisioning()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/RtcVoiceConfigWithFaxProvisioning.json"));
            var prov = new ScheduledVoiceFaxProvisioning()
            {
                ServiceId = "service id value",
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            };
            var expected = new VoiceRtcConfiguration()
            {
                AppId = "app id value",
                LastUpdatedTime = Helpers.ParseUtc("2024-06-30T07:08:09.100Z"),
                ScheduledVoiceProvisioning = prov
            };

            obj.VoiceConfiguration.Should().BeEquivalentTo(expected);
            // cast to expected type to test exclusively 
            obj.VoiceConfiguration.ScheduledVoiceProvisioning.As<ScheduledVoiceFaxProvisioning>().Should()
                .BeEquivalentTo(prov);
        }
    }
}
