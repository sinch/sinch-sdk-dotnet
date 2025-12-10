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
        public void VoiceConfiguration_ShouldBeAbstract()
        {
            typeof(VoiceConfiguration).IsAbstract.Should().BeTrue();
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

            obj.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = new VoiceEstConfiguration()
                {
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                    TrunkId = "trunk id value",
                    ScheduledVoiceProvisioning = new ScheduledVoiceEstProvisioning()
                    {
                        TrunkId = "trunk id value",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                    },
                }
            });
        }

        [Fact]
        public void ShouldDeserializeVoiceFaxConfiguration()
        {
            var obj =
                DeserializeAsNumbersClient<Container>(
                    Helpers.LoadResources("Numbers/FaxVoiceResponse.json"));

            obj.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = new VoiceFaxConfiguration()
                {
                    ServiceId = "service id value",
                    LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z"),
                    ScheduledVoiceProvisioning = new ScheduledVoiceFaxProvisioning()
                    {
                        ServiceId = "service id value",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
                    }
                }
            });
        }

        [Fact]
        public void ShouldDeserializeVoiceRtcConfiguration()
        {
            var container = DeserializeAsNumbersClient<Container>(Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));
            
            var expected = new VoiceRtcConfiguration()
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
            container.Should().BeEquivalentTo(new Container()
            {
                VoiceConfiguration = expected
            });
        }

        [Fact]
        public void ScheduledVoiceProvisioning_ShouldBeAbstract()
        {
            typeof(ScheduledVoiceProvisioning).IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void ScheduledVoiceRtcProvisioning_ShouldDeserializeToConcreteType()
        {
            var obj = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));

            var voiceRtc = (VoiceRtcConfiguration)obj.VoiceConfiguration;

            // Unique value: Explicitly tests the polymorphic deserialization
            voiceRtc.ScheduledVoiceProvisioning.Should().BeOfType<ScheduledVoiceRtcProvisioning>();

            // Use BeEquivalentTo for completeness
            var scheduledRtc = voiceRtc.ScheduledVoiceProvisioning;
            scheduledRtc.Should().BeEquivalentTo(new ScheduledVoiceRtcProvisioning()
            {
                AppId = "app id value",
                Status = ProvisioningStatus.Waiting,
                LastUpdatedTime = Helpers.ParseUtc("2024-07-01T11:58:35.610198Z")
            });
        }

        [Fact]
        public void ScheduledVoiceFaxProvisioning_ShouldDeserializeToConcreteType()
        {
            var container = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/FaxVoiceResponse.json"));

            var voiceFax = (VoiceFaxConfiguration)container.VoiceConfiguration;
            voiceFax.ScheduledVoiceProvisioning.Should().BeOfType<ScheduledVoiceFaxProvisioning>();
        }

        [Fact]
        public void ScheduledVoiceEstProvisioning_ShouldDeserializeToConcreteType()
        {
            var container = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/EstVoiceResponse.json"));

            var voiceEst = (VoiceEstConfiguration)container.VoiceConfiguration;
            voiceEst.ScheduledVoiceProvisioning.Should().BeOfType<ScheduledVoiceEstProvisioning>();
        }

        [Fact]
        public void VoiceRtcConfiguration_ShouldDeserializeToConcreteType()
        {
            var obj = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/RtcVoiceResponse.json"));

            obj.VoiceConfiguration.Should().BeOfType<VoiceRtcConfiguration>();
        }

        [Fact]
        public void VoiceEstConfiguration_ShouldDeserializeToConcreteType()
        {
            var obj = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/EstVoiceResponse.json"));

            obj.VoiceConfiguration.Should().BeOfType<VoiceEstConfiguration>();
        }

        [Fact]
        public void VoiceFaxConfiguration_ShouldDeserializeToConcreteType()
        {
            var obj = DeserializeAsNumbersClient<Container>(
                Helpers.LoadResources("Numbers/FaxVoiceResponse.json"));

            obj.VoiceConfiguration.Should().BeOfType<VoiceFaxConfiguration>();
        }

        [Fact]
        public void VoiceConfigurationConverter_ShouldThrowOnUnknownType()
        {
            var unknownJson = """{"voiceConfiguration": {"type": "UNKNOWN"}}""";

            var act = () => DeserializeAsNumbersClient<Container>(unknownJson);

            act.Should().Throw<JsonException>()
                .WithMessage("*Type tag is invalid*");
        }

        [Fact]
        public void VoiceConfigurationConverter_ShouldThrowOnMissingType()
        {
            var noTypeJson = """{"voiceConfiguration": {"lastUpdatedTime": "2024-07-01T11:58:35.610198Z"}}""";

            var act = () => DeserializeAsNumbersClient<Container>(noTypeJson);

            act.Should().Throw<JsonException>()
                .WithMessage("*Failed to deserialize VoiceConfiguration*");
        }

        [Fact]
        public void VoiceRtcConfiguration_RoundTrip_ShouldPreserveAllProperties()
        {
            var original = new VoiceRtcConfiguration()
            {
                AppId = "test-app-id"
            };

            var json = SerializeAsNumbersClient(new Container { VoiceConfiguration = original });
            var deserialized = DeserializeAsNumbersClient<Container>(json);

            deserialized.VoiceConfiguration.Should().BeOfType<VoiceRtcConfiguration>();
            var rtcConfig = (VoiceRtcConfiguration)deserialized.VoiceConfiguration;
            rtcConfig.AppId.Should().Be("test-app-id");
            rtcConfig.Type.Should().Be(VoiceApplicationType.Rtc);
        }

        [Fact]
        public void VoiceFaxConfiguration_RoundTrip_ShouldPreserveAllProperties()
        {
            var original = new VoiceFaxConfiguration()
            {
                ServiceId = "test-service-id"
            };

            var json = SerializeAsNumbersClient(new Container { VoiceConfiguration = original });
            var deserialized = DeserializeAsNumbersClient<Container>(json);

            deserialized.VoiceConfiguration.Should().BeOfType<VoiceFaxConfiguration>();
            var faxConfig = (VoiceFaxConfiguration)deserialized.VoiceConfiguration;
            faxConfig.ServiceId.Should().Be("test-service-id");
            faxConfig.Type.Should().Be(VoiceApplicationType.Fax);
        }

        [Fact]
        public void VoiceEstConfiguration_RoundTrip_ShouldPreserveAllProperties()
        {
            var original = new VoiceEstConfiguration()
            {
                TrunkId = "test-trunk-id"
            };

            var json = SerializeAsNumbersClient(new Container { VoiceConfiguration = original });
            var deserialized = DeserializeAsNumbersClient<Container>(json);

            deserialized.VoiceConfiguration.Should().BeOfType<VoiceEstConfiguration>();
            var estConfig = (VoiceEstConfiguration)deserialized.VoiceConfiguration;
            estConfig.TrunkId.Should().Be("test-trunk-id");
            estConfig.Type.Should().Be(VoiceApplicationType.Est);
        }

        [Fact]
        public void VoiceConfiguration_Type_ShouldBeSetCorrectlyForEachImplementation()
        {
            var rtc = new VoiceRtcConfiguration();
            rtc.Type.Should().Be(VoiceApplicationType.Rtc);

            var fax = new VoiceFaxConfiguration();
            fax.Type.Should().Be(VoiceApplicationType.Fax);

            var est = new VoiceEstConfiguration();
            est.Type.Should().Be(VoiceApplicationType.Est);
        }

        [Fact]
        public void ScheduledVoiceProvisioning_Type_ShouldBeSetCorrectlyForEachImplementation()
        {
            var rtc = new ScheduledVoiceRtcProvisioning();
            rtc.Type.Should().Be(VoiceApplicationType.Rtc);

            var fax = new ScheduledVoiceFaxProvisioning();
            fax.Type.Should().Be(VoiceApplicationType.Fax);

            var est = new ScheduledVoiceEstProvisioning();
            est.Type.Should().Be(VoiceApplicationType.Est);
        }
    }
}
