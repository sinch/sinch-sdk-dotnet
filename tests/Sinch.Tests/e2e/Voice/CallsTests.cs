using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Calls;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Calls.Instructions;
using Sinch.Voice.Calls.Manage;
using Sinch.Voice.Calls.Update;
using Xunit;

namespace Sinch.Tests.e2e.Voice
{
    public class CallsTests : VoiceTestBase
    {
        [Fact]
        public async Task GetCall()
        {
            var response = await VoiceClient.Calls.Get("123");
            response.Should().BeEquivalentTo(new Call
            {
                From = "456",
                To = "987",
                Domain = CallDomain.Pstn,
                CallId = "123",
                Duration = 60,
                Status = CallStatus.ONGOING,
                Result = CallResult.Busy,
                Reason = CallResultReason.Cancel,
                Timestamp = DateTime.Parse("2019-08-24T14:15:22Z").ToUniversalTime(),
                Custom = new object(),
                UserRate = "39",
                Debit = "138"
            });
        }

        [Fact]
        public async Task UpdateCall()
        {
            await VoiceClient.Calls.Update(new UpdateCallRequest
            {
                CallId = "123",
                Action = new Hangup(),
                Instructions = new List<IInstruction>
                {
                    new Say
                    {
                        Text = "Hello!",
                        Locale = "en-US"
                    },
                    new StopRecording()
                }
            });
        }

        [Fact]
        public async Task ManageWithCallLeg()
        {
            await VoiceClient.Calls.ManageWithCallLeg("123", CallLeg.Both, new ManageWithCallLegRequest
            {
                Action = new ConnectSip
                {
                    Destination = new ConnectSipDestination
                    {
                        Endpoint = "sip_endpoint"
                    },
                    MaxDuration = 120,
                    Cli = "string",
                    Transport = Transport.UDP,
                    SuppressCallbacks = true,
                    CallHeaders = new List<CallHeader>
                    {
                        new()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    },
                    Moh = MohClass.Music1
                },
                Instructions = new List<IInstruction>
                {
                    new Answer(),
                    new SendDtmf
                    {
                        Value = "ww1234#w#"
                    },
                    new PlayFiles
                    {
                        Ids = new List<List<string>>
                        {
                            new() { "#tts[]" },
                            new() { "Welcome", "id.com" }
                        },
                        Locale = "es-ES"
                    }
                }
            });
        }
    }
}
