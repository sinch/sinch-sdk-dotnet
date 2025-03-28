using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS;
using Sinch.SMS.Batches;
using Sinch.SMS.Batches.DryRun;
using Sinch.SMS.Batches.List;
using Sinch.SMS.Batches.Send;
using Sinch.SMS.Batches.Update;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class BatchesTests : SmsTestBase
    {
        private BinaryBatch _binaryBatchResponse = new BinaryBatch
        {
            Id = "01FC66621XXXXX119Z8PMV1QPQ",
            To = new List<string>
            {
                "+15551231234",
                "+15551256344"
            },
            From = "+15551231234",
            Canceled = false,
            Body = "Hi ${name} (${an_identifier}) ! How are you?",
            DeliveryReport = DeliveryReport.None,
            SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
            ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
            CallbackUrl = "https://nickelback.com",
            ClientReference = "myReference",
            FeedbackEnabled = false,
            FlashMessage = true,
            TruncateConcat = true,
            MaxNumberOfMessageParts = 1,
            FromTon = 6,
            FromNpi = 18,
            Udh = "foo udh"
        };

        private static readonly object Batch = new
        {
            id = "01FC66621XXXXX119Z8PMV1QPQ",
            to = new[] { "15551231234", "15551256344" },
            from = "15551231234",
            canceled = false,
            body = "Hi ${name}! How are you?",
            type = "mt_binary",
            created_at = "2019-08-24T14:15:22Z",
            modified_at = "2019-08-24T14:15:22Z",
            delivery_report = "full",
            send_at = "2019-08-24T14:15:22Z",
            expire_at = "2019-08-24T14:15:22Z",
            callback_url = "string",
            client_reference = "string",
            feedback_enabled = false,
            flash_message = false,
            truncate_concat = true,
            max_number_of_message_parts = 1,
            from_ton = 6,
            from_npi = 18,
            udh = "udh_"
        };

        [Fact]
        public async Task SendTextBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/SendTextBatchRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/TextResponse.json"));

            var response = await Sms.Batches.Send(new SendTextBatchRequest
            {
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Body = "Hi ${name} (${an_identifier}) ! How are you?",
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                CallbackUrl = "https://my.callback.com",
                ClientReference = "myReference",
                FeedbackEnabled = false,
                FlashMessage = true,
                TruncateConcat = true,
                MaxNumberOfMessageParts = 1,
                FromTon = 6,
                FromNpi = 18,
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    ["name"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "name value for 15551231234",
                        ["15551256344"] = "name value for 15551256344",
                        ["default"] = "default value"
                    },
                    ["an_identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                }
            });

            response.Should().BeOfType<TextBatch>().Which.Should().BeEquivalentTo(new TextBatch
            {
                Id = "01FC66621XXXXX119Z8PMV1QPQ",
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Canceled = false,
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    ["name"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "name value for 15551231234",
                        ["15551256344"] = "name value for 15551256344",
                        ["default"] = "default value"
                    },
                    ["an_identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                },
                Body = "Hi ${name} (${an_identifier}) ! How are you?",
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                CallbackUrl = "https://callback.yes",
                ClientReference = "myReference",
                FeedbackEnabled = false,
                FlashMessage = true,
                TruncateConcat = true,
                MaxNumberOfMessageParts = 1,
                FromTon = 6,
                FromNpi = 18
            });
        }

        [Fact]
        public async Task SendMediaBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/SendMediaBatchRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/MediaResponse.json"));

            var response = await Sms.Batches.Send(new SendMediaBatchRequest
            {
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Body = new MediaBody
                {
                    Message = "Hi ${name} (${an_identifier}) ! How are you?",
                    Url = "https://en.wikipedia.org/wiki/Sinch_(company)#/media/File:Sinch_LockUp_RGB.png"
                },
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    ["name"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "name value for 15551231234",
                        ["15551256344"] = "name value for 15551256344",
                        ["default"] = "default value"
                    },
                    ["an_identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                },
                DeliveryReport = DeliveryReport.Summary,
                SendAt = Helpers.ParseUtc("2019-08-24T14:16:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:17:22Z"),
                CallbackUrl = "https://my.callback.com",
                ClientReference = "client reference",
                FeedbackEnabled = false,
                StrictValidation = true
            });

            response.Should().BeOfType<MediaBatch>().Which.Should().BeEquivalentTo(new MediaBatch()
            {
                Id = "01FC66621XXXXX119Z8PMV1QPQ",
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Canceled = false,
                Body = new MediaBody
                {
                    Message = "Hi ${name} (${an_identifier}) ! How are you?",
                    Url = "https://en.wikipedia.org/wiki/Sinch_(company)#/media/File:Sinch_LockUp_RGB.png",
                    Subject = "subject field"
                },
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    ["name"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "name value for 15551231234",
                        ["15551256344"] = "name value for 15551256344",
                        ["default"] = "default value"
                    },
                    ["an_identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                },
                DeliveryReport = DeliveryReport.Summary,
                SendAt = Helpers.ParseUtc("2019-08-24T14:16:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:17:22Z"),
                CallbackUrl = "https://callback.my",
                ClientReference = "client reference",
                FeedbackEnabled = false,
                StrictValidation = true
            });
        }

        [Fact]
        public async Task SendBinaryBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/SendBinaryBatchRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/BinaryResponse.json"));

            var response = await Sms.Batches.Send(new SendBinaryBatchRequest
            {
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Body = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hi ${name} (${an_identifier}) ! How are you?")),
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                CallbackUrl = "https://my.callback.com",
                ClientReference = "myReference",
                FeedbackEnabled = false,
                FlashMessage = true,
                TruncateConcat = true,
                MaxNumberOfMessageParts = 1,
                FromTon = 6,
                FromNpi = 18,
                Udh = Convert.ToHexString(Encoding.UTF8.GetBytes("foo udh"))
            });

            response.Should().BeOfType<BinaryBatch>().Which.Should().BeEquivalentTo(_binaryBatchResponse);
        }

        [Fact]
        public async Task List()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches";
            uri += "?page=3";
            uri += "&page_size=11";
            uri += "&from=123,456";
            uri += "&start_date=2023-08-16T03:14:17.0000000Z";
            uri += "&end_date=2023-08-16T03:14:17.0000000Z";
            uri += "&client_reference=havel";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 3,
                    count = 123,
                    page_size = 11,
                    batches = new[]
                    {
                        Batch
                    }
                }));

            var request = new ListBatchesRequest
            {
                PageSize = 11,
                ClientReference = "havel",
                From = new List<string>() { "123", "456" },
                Page = 3,
                StartDate = new DateTime(2023, 8, 16, 3, 14, 17, DateTimeKind.Utc),
                EndDate = new DateTime(2023, 8, 16, 3, 14, 17, DateTimeKind.Utc)
            };
            var response = await Sms.Batches.List(request);

            response.Should().NotBeNull();
            response.Page.Should().Be(3);
            response.Count.Should().Be(123);
            response.PageSize.Should().Be(11);
            response.Batches.Should().HaveCount(1);
        }

        [Fact]
        public async Task DryRun()
        {
            var uri =
                $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/dry_run?per_recipient=false&number_of_recipients=144";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("mt_binary")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    number_of_recipients = 5,
                    number_of_messages = 50,
                    per_recipient = new[]
                    {
                        new { recipient = "a", body = "c", encoding = "UTF-8", number_of_parts = 1 },
                        new { recipient = "a", body = "c", encoding = "UTF-8", number_of_parts = 3 }
                    }
                }));

            var response = await Sms.Batches.DryRun(new DryRunRequest
            {
                PerRecipient = false,
                NumberOfRecipients = 144,
                BatchRequest = new SendBinaryBatchRequest()
                {
                    To = new List<string> { "1", "2" },
                    Body = "some_body",
                    From = "sender",
                    Udh = "ox213",
                    DeliveryReport = DeliveryReport.PerRecipient,
                    SendAt = DateTime.UtcNow.AddDays(1),
                    ExpireAt = DateTime.UtcNow.AddDays(2),
                    CallbackUrl = "https://localhost:2534",
                    FlashMessage = false,
                    ClientReference = "admin",
                    MaxNumberOfMessageParts = 3
                }
            });

            response.Should().NotBeNull();
            response.PerRecipient.Should().HaveCount(2);
            response.NumberOfMessages.Should().Be(50);
            response.NumberOfRecipients.Should().Be(5);

            response.PerRecipient!.First().Should().BeEquivalentTo(new PerRecipient()
            {
                Body = "c",
                Recipient = "a",
                NumberOfParts = 1,
                Encoding = "UTF-8"
            });
        }

        [Fact]
        public async Task Get()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Get, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Get("01FC66621XXXXX119Z8PMV1QPQ");

            var binaryBatch = response.Should().BeOfType<BinaryBatch>().Which;
            binaryBatch.Udh.Should().Be("udh_");
        }

        [Fact]
        public async Task UpdateText()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/UpdateTextRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/TextResponse.json"));

            var response = await Sms.Batches.Update("01FC66621XXXXX119Z8PMV1QPQ",
                new UpdateTextBatchRequest
                {
                    ToAdd = new List<string>
                    {
                        "+15551231234",
                        "+15551256344"
                    },
                    ToRemove = new List<string>
                    {
                        "+15550001111",
                        "+15550002222"
                    },
                    From = "+15551231234",
                    Body = "Hi ${name} (${an_identifier}) ! How are you?",
                    DeliveryReport = DeliveryReport.None,
                    SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                    ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                    CallbackUrl = "https://calback.yes",
                    ClientReference = "mock-client-ref-123",
                    FeedbackEnabled = true,
                    Parameters = new Dictionary<string, Dictionary<string, string>>
                    {
                        ["name"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "name value for 15551231234",
                            ["15551256344"] = "name value for 15551256344",
                            ["default"] = "default value"
                        },
                        ["an_identifier"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "an identifier value for 15551231234",
                            ["15551256344"] = "an identifier value for 15551256344"
                        }
                    },
                    MaxNumberOfMessageParts = 5,
                    FromTon = 1,
                    FromNpi = 2,
                    TruncateConcat = true,
                    FlashMessage = true
                });

            response.Should().BeOfType<TextBatch>().Which.Should().BeEquivalentTo(new TextBatch
            {
                Id = "01FC66621XXXXX119Z8PMV1QPQ",
                To = new List<string>
                {
                    "+15551231234",
                    "+15551256344"
                },
                From = "+15551231234",
                Canceled = false,
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    ["name"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "name value for 15551231234",
                        ["15551256344"] = "name value for 15551256344",
                        ["default"] = "default value"
                    },
                    ["an_identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                },
                Body = "Hi ${name} (${an_identifier}) ! How are you?",
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                CallbackUrl = "https://callback.yes",
                ClientReference = "myReference",
                FeedbackEnabled = false,
                FlashMessage = true,
                TruncateConcat = true,
                MaxNumberOfMessageParts = 1,
                FromTon = 6,
                FromNpi = 18
            });
        }

        [Fact]
        public async Task UpdateBinary()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/UpdateBinaryRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/BinaryResponse.json"));

            var response = await Sms.Batches.Update("01FC66621XXXXX119Z8PMV1QPQ",
                new UpdateBinaryBatchRequest
                {
                    ToAdd = new List<string>
                    {
                        "+15551231234",
                        "+15987365412"
                    },
                    ToRemove = new List<string>
                    {
                        "+0123456789",
                        "+9876543210"
                    },
                    From = "+15551231234",
                    DeliveryReport = DeliveryReport.Full,
                    SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                    ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                    CallbackUrl = "https://callback.yes",
                    ClientReference = "a client reference",
                    FeedbackEnabled = true,
                    Body = "Hi ${name} (${an_identifier}) ! How are you?",
                    Udh = "foo udh",
                    FromTon = 3,
                    FromNpi = 10
                });

            response.Should().BeOfType<BinaryBatch>().Which.Should().BeEquivalentTo(_binaryBatchResponse);
        }

        [Fact]
        public async Task UpdateMedia()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Sms/Batch/UpdateMediaRequest.json"))
                .Respond("application/json", Helpers.LoadResources("Sms/Batch/MediaResponse.json"));

            var response = await Sms.Batches.Update("01FC66621XXXXX119Z8PMV1QPQ",
                new UpdateMediaBatchRequest()
                {
                    ToAdd = new List<string>
                    {
                        "+15551231234",
                        "+15987365412"
                    },
                    ToRemove = new List<string>
                    {
                        "+0123456789",
                        "+9876543210"
                    },
                    From = "+15551231234",
                    Body = new MediaBody
                    {
                        Message = "Hi ${name} (${an_identifier}) ! How are you?",
                        Url = "https://en.wikipedia.org/wiki/Sinch_(company)#/media/File:Sinch_LockUp_RGB.png"
                    },
                    Parameters = new Dictionary<string, Dictionary<string, string>>
                    {
                        ["name"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "name value for 15551231234",
                            ["15551256344"] = "name value for 15551256344",
                            ["default"] = "default value"
                        },
                        ["an_identifier"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "an identifier value for 15551231234",
                            ["15551256344"] = "an identifier value for 15551256344"
                        }
                    },
                    DeliveryReport = DeliveryReport.Summary,
                    SendAt = Helpers.ParseUtc("2019-08-24T14:16:22Z"),
                    ExpireAt = Helpers.ParseUtc("2019-08-24T14:17:22Z"),
                    CallbackUrl = "https://calback.my",
                    ClientReference = "a client reference",
                    FeedbackEnabled = true,
                    StrictValidation = true
                });

            response.Should().BeOfType<MediaBatch>().Which.Should().BeEquivalentTo(
                new MediaBatch
                {
                    Id = "01FC66621XXXXX119Z8PMV1QPQ",
                    To = new List<string>
                    {
                        "+15551231234",
                        "+15551256344"
                    },
                    From = "+15551231234",
                    Canceled = false,
                    Body = new MediaBody
                    {
                        Message = "Hi ${name} (${an_identifier}) ! How are you?",
                        Url = "https://en.wikipedia.org/wiki/Sinch_(company)#/media/File:Sinch_LockUp_RGB.png",
                        Subject = "subject field"
                    },
                    Parameters = new Dictionary<string, Dictionary<string, string>>
                    {
                        ["name"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "name value for 15551231234",
                            ["15551256344"] = "name value for 15551256344",
                            ["default"] = "default value"
                        },
                        ["an_identifier"] = new Dictionary<string, string>
                        {
                            ["15551231234"] = "an identifier value for 15551231234",
                            ["15551256344"] = "an identifier value for 15551256344"
                        }
                    },
                    DeliveryReport = DeliveryReport.Summary,
                    SendAt = Helpers.ParseUtc("2019-08-24T14:16:22Z"),
                    ExpireAt = Helpers.ParseUtc("2019-08-24T14:17:22Z"),
                    CallbackUrl = "https://callback.my",
                    ClientReference = "client reference",
                    FeedbackEnabled = false,
                    StrictValidation = true
                });
        }

        [Fact]
        public async Task Replace()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Put, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("hi, {admin}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Replace("01FC66621XXXXX119Z8PMV1QPQ", new SendBinaryBatchRequest()
            {
                To = new List<string> { "15551231234", "15551256344" },
                Body = "hi, {admin}"
            });

            var binaryBatch = response.Should().BeOfType<BinaryBatch>().Which;
            binaryBatch.Body.Should().Be("Hi ${name}! How are you?");
        }

        [Fact]
        public async Task Cancel()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Delete, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Cancel("01FC66621XXXXX119Z8PMV1QPQ");

            response.Should().BeOfType<BinaryBatch>().Which.Body.Should().Be("Hi ${name}! How are you?");
        }

        [Fact]
        public async Task SendDeliveryFeedback()
        {
            var uri =
                $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ/delivery_feedback";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("[\"1234\",\"456\"]")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            Func<Task> response = () => Sms.Batches.SendDeliveryFeedback("01FC66621XXXXX119Z8PMV1QPQ", new[]
            {
                "1234",
                "456"
            });

            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ListAuto()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, uri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 0,
                    count = 30,
                    page_size = 10,
                    batches = new[]
                    {
                        Batch
                    }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, uri)
                .WithQueryString("page", "1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 1,
                    count = 30,
                    page_size = 10,
                    batches = new[]
                    {
                        Batch
                    }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, uri)
                .WithQueryString("page", "2")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 2,
                    count = 30,
                    page_size = 10,
                    batches = new[]
                    {
                        Batch
                    }
                }));
            var request = new ListBatchesRequest();
            var response = Sms.Batches.ListAuto(request);
            await foreach (var batch in response)
            {
                batch.Should().NotBeNull();
            }

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }
    }
}
