using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
                .WithPartialContent("irythil")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var request = new SendTextBatchRequest()
            {
                Body = "Hi ${name}! How are you?",
                DeliveryReport = DeliveryReport.Full,
                To = new List<string> { "15551231234", "15551256344" },
                From = "15551231234",
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "name", new Dictionary<string, string>
                        {
                            { "msisdn", "123" },
                            { "default_value", "irythil" }
                        }
                    }
                },
                SendAt = DateTime.Parse("2019-08-24T14:15:22Z"),
                ExpireAt = DateTime.Parse("2019-08-24T14:15:22Z"),
                CallbackUrl = new Uri("http://localhost:1202"),
                ClientReference = "string",
                FeedbackEnabled = false,
                FlashMessage = false,
                TruncateConcat = true,
                MaxNumberOfMessageParts = 1,
                FromTon = 6,
                FromNpi = 18
            };

            var response = await Sms.Batches.Send(request);

            var textBatch = response.Should().BeOfType<BinaryBatch>().Which;

            textBatch.DeliveryReport.Should().Be(DeliveryReport.Full);
            textBatch.Type.Should().Be(SmsType.MtBinary);
        }

        [Fact]
        public async Task SendMediaBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(new
                {
                    body = new
                    {
                        url = "http://hello.world",
                        message = "HI"
                    },
                    to = new[] { "123", "456" },
                    strict_validation = true,
                    type = "mt_media",
                    feedback_enabled = false,
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "1",
                    strict_validation = true,
                    type = "mt_media",
                    body = new
                    {
                        url = "http://hello.world",
                        message = "HI"
                    },
                    to = new[] { "123", "456" },
                }));

            var request = new SendMediaBatchRequest()
            {
                Body = new MediaBody()
                {
                    Message = "HI",
                    Url = new Uri("http://hello.world")
                },
                To = new List<string>()
                {
                    "123", "456",
                },
                StrictValidation = true,
                FeedbackEnabled = false,
            };

            var response = await Sms.Batches.Send(request);

            var mediaBatch = response.Should().BeOfType<MediaBatch>().Which;

            mediaBatch.Id.Should().Be("1");
            mediaBatch.StrictValidation.Should().BeTrue();
        }

        [Fact]
        public async Task SendBinaryBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(new
                {
                    body = "Holla!",
                    to = new[] { "123", "456" },
                    type = "mt_binary",
                    feedback_enabled = false,
                    flash_message = true,
                    truncate_concat = false,
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "1",
                    type = "mt_binary",
                    flash_message = true,
                    body = "Holla!",
                    to = new[] { "123", "456" }
                }));

            var request = new SendBinaryBatchRequest()
            {
                Body = "Holla!",
                To = new List<string>()
                {
                    "123", "456",
                },
                FlashMessage = true,
                TruncateConcat = false,
                FeedbackEnabled = false,
            };

            var response = await Sms.Batches.Send(request);

            var mediaBatch = response.Should().BeOfType<BinaryBatch>().Which;

            mediaBatch.Id.Should().Be("1");
            mediaBatch.FlashMessage.Should().BeTrue();
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
                    CallbackUrl = new Uri("https://localhost:2534"),
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
                .WithJson(Helpers.LoadResources("Sms/Batch/UpdateTextBatch.json"))
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
                    Body = "Hi ${name} ({an identifier}) ! How are you?",
                    DeliveryReport = DeliveryReport.None,
                    SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                    ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                    CallbackUrl = new Uri("https://calback.yes"),
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
                        ["an identifier"] = new Dictionary<string, string>
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
                    ["an identifier"] = new Dictionary<string, string>
                    {
                        ["15551231234"] = "an identifier value for 15551231234",
                        ["15551256344"] = "an identifier value for 15551256344"
                    }
                },
                Body = "Hi ${name} ({an identifier}) ! How are you?",
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2019-08-24T14:19:22Z"),
                ExpireAt = Helpers.ParseUtc("2019-08-24T14:21:22Z"),
                CallbackUrl = new Uri("https://callback.yes"),
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
