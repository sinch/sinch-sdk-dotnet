using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS;
using Sinch.SMS.Batches;
using Sinch.SMS.Batches.Send;
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
            parameters = new
            {
                name = new
                {
                    msisdn = "123",
                    default_value = "irythil"
                }
            },
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
        public async Task SendBatch()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("irythil")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var request = new TextBatchRequest()
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

            response.Should().NotBeNull();
            response.Parameters["name"].Should().BeEquivalentTo(new Dictionary<string, string>
            {
                { "msisdn", "123" },
                { "default_value", "irythil" }
            });
            response.DeliveryReport.Should().Be(DeliveryReport.Full);
            response.Type.Should().Be(SmsType.MtBinary);
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

            var request = new SMS.Batches.List.ListBatchesRequest
            {
                PageSize = 11,
                ClientReference = "havel",
                From = new[] { "123", "456" },
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
                        new { recipient = "a", message_part = "b", body = "c", encoding = "g" },
                        new { recipient = "a", message_part = "b", body = "c", encoding = "g" }
                    }
                }));

            var response = await Sms.Batches.DryRun(new SMS.Batches.DryRun.DryRunRequest
            {
                PerRecipient = false,
                NumberOfRecipients = 144,
                To = new List<string> { "1", "2" },
                Body = "some_body",
                From = "sender",
                Type = SmsType.MtBinary,
                Udh = "ox213",
                DeliveryReport = DeliveryReport.PerRecipient,
                SendAt = DateTime.UtcNow.AddDays(1),
                ExpireAt = DateTime.UtcNow.AddDays(2),
                CallbackUrl = new Uri("https://localhost:2534"),
                FlashMessage = false,
                Parameters = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "name", new Dictionary<string, string>
                        {
                            { "sid", "131321" },
                            { "default", "ag" }
                        }
                    }
                },
                ClientReference = "admin",
                MaxNumberOfMessageParts = 3
            });

            response.Should().NotBeNull();
            response.PerRecipient.Should().HaveCount(2);
            response.NumberOfMessages.Should().Be(50);
            response.NumberOfRecipients.Should().Be(5);
        }

        [Fact]
        public async Task Get()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Get, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Get("01FC66621XXXXX119Z8PMV1QPQ");

            response.Should().NotBeNull();
            response.Udh.Should().Be("udh_");
        }

        [Fact]
        public async Task Update()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Post, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("31231323")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Update("01FC66621XXXXX119Z8PMV1QPQ",
                new SMS.Batches.Update.UpdateBatchRequest
                {
                    Body = null,
                    From = "31231323",
                    CallbackUrl = new Uri("http://localhost:3452"),
                    DeliveryReport = DeliveryReport.Summary,
                    ExpireAt = DateTime.UtcNow.AddDays(3),
                    SendAt = DateTime.Now.AddDays(5),
                    ToAdd = new List<string>
                    {
                        "123",
                        "456"
                    },
                    ToRemove = new List<string>
                    {
                        "987"
                    }
                });

            response.Should().NotBeNull();
            response.Udh.Should().Be("udh_");
        }

        [Fact]
        public async Task Replace()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Put, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("hi, {admin}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Replace(new Batch
            {
                Id = "01FC66621XXXXX119Z8PMV1QPQ",
                To = new List<string> { "15551231234", "15551256344" },
                Parameters = new Dictionary<string, Dictionary<string, string>>(),
                Body = "hi, {admin}"
            });

            response.Should().NotBeNull();
            response.Body.Should().Be("Hi ${name}! How are you?");
        }

        [Fact]
        public async Task Cancel()
        {
            var uri = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/batches/01FC66621XXXXX119Z8PMV1QPQ";
            HttpMessageHandlerMock.When(HttpMethod.Delete, uri)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Batch));

            var response = await Sms.Batches.Cancel("01FC66621XXXXX119Z8PMV1QPQ");

            response.Should().NotBeNull();
            response.Body.Should().Be("Hi ${name}! How are you?");
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
                .WithQueryString("page", "0")
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
            var request = new SMS.Batches.List.ListBatchesRequest();
            var response = Sms.Batches.ListAuto(request);
            await foreach (var batch in response)
            {
                batch.Should().NotBeNull();
            }

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }
    }
}
