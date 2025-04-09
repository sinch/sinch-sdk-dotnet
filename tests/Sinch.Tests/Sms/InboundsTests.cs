using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS.Inbounds;
using Sinch.SMS.Inbounds.List;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class InboundsTests : SmsTestBase
    {
        private static object Inbound()
        {
            return new
            {
                type = "mo_binary",
                id = "in-bound",
                from = "123",
                to = "456",
                body = "some_body",
                client_reference = "ccc",
                operator_id = "op_id",
                send_at = "2019-08-24T14:15:22Z",
                received_at = "2019-08-24T14:15:22Z",
                udh = "hey"
            };
        }


        [Fact]
        public async Task GetBinary()
        {
            const string inboundId = "in-bound";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/inbounds/{inboundId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(Inbound()));

            var response = await Sms.Inbounds.Get(inboundId);

            response.Should().NotBeNull();
            response.Should().BeOfType<BinaryInbound>().Which.Should().BeEquivalentTo(new BinaryInbound()
            {
                Id = "in-bound",
                Body = "some_body",
                SendAt = new DateTime(2019, 8, 24, 14, 15, 22),
                ReceivedAt = new DateTime(2019, 8, 24, 14, 15, 22),
                From = "123",
                To = "456",
                Udh = "hey",
                ClientReference = "ccc",
                OperatorId = "op_id"
            });
        }

        [Fact]
        public async Task GetSms()
        {
            const string inboundId = "in-bound";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/inbounds/{inboundId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    type = "mo_text",
                    id = inboundId,
                    from = "+4800000",
                    to = "+48000001",
                    body = "hello-world",
                    received_at = "2019-08-24T14:15:22Z",
                    udh = "hey"
                }));

            var response = await Sms.Inbounds.Get(inboundId);

            response.Should().NotBeNull();
            response.Should().BeOfType<SmsInbound>().Which.Should().BeEquivalentTo(new SmsInbound()
            {
                Id = inboundId,
                Body = "hello-world",
                ReceivedAt = new DateTime(2019, 8, 24, 14, 15, 22),
                From = "+4800000",
                To = "+48000001",
            });
        }

        [Fact]
        public async Task GetMediaInbound()
        {
            var json = Helpers.LoadResources("Sms/Inbounds/InboundMedia.json");
            const string inboundId = "media-inbound";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/inbounds/{inboundId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", json);

            var response = await Sms.Inbounds.Get(inboundId);

            response.Should().NotBeNull();
            response.Should().BeOfType<MediaInbound>().Which.Should().BeEquivalentTo(new MediaInbound
            {
                ClientReference = "a client reference",
                From = "+11203494390",
                Id = "01FC66621XXXXX119Z8PMV1QPA",
                OperatorId = "35000",
                ReceivedAt = Helpers.ParseUtc("2019-08-24T14:17:22Z"),
                SentAt = Helpers.ParseUtc("2019-08-24T14:15:22Z"),
                To = "11203453453",
                Body = new MmsMoBody
                {
                    Subject = "mmy subject",
                    Message = "my message",
                    Media = new List<MmsMedia>
                    {
                        new MmsMedia
                        {
                            Url = "https://foo.url",
                            ContentType = "content/type",
                            Status = new MmsMedia.StatusEnum("a status"),
                            Code = 1234
                        }
                    }
                }
            });
        }

        [Fact]
        public async Task List()
        {
            var url = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/inbounds";
            url += "?page=3";
            url += "&to=%2B123,%2B456";
            url += "&start_date=2019-08-24T14%3A15%3A22.5420000Z";
            url += "&end_date=2019-08-24T14%3A15%3A22.5420000Z";
            url += "&client_reference=icr";
            HttpMessageHandlerMock
                .When(HttpMethod.Get, url)
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 3,
                    page_size = 14,
                    count = 2,
                    inbounds = new[]
                    {
                        Inbound()
                    }
                }));
            var date = new DateTime(2019, 8, 24, 14, 15, 22, 542, DateTimeKind.Utc);
            var request = new ListInboundsRequest
            {
                Page = 3,
                PageSize = 0,
                To = new List<string>() { "+123", "+456" },
                StartDate = date,
                EndDate = date,
                ClientReference = "icr"
            };

            var response = await Sms.Inbounds.List(request);

            response.Should().NotBeNull();
            response.Page.Should().Be(3);
            response.PageSize.Should().Be(14);
            response.Count.Should().Be(2);
            response.Inbounds.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListAuto()
        {
            var url = $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/inbounds";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, url)
                .WithQueryString("page", "0")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 0,
                    page_size = 1,
                    count = 3,
                    inbounds = new[]
                    {
                        Inbound()
                    }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, url)
                .WithQueryString("page", "1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 1,
                    page_size = 1,
                    count = 3,
                    inbounds = new[]
                    {
                        Inbound()
                    }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, url)
                .WithQueryString("page", "2")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 2,
                    page_size = 1,
                    count = 3,
                    inbounds = new[]
                    {
                        Inbound()
                    }
                }));
            var request = new ListInboundsRequest
            {
                Page = 0
            };

            var response = Sms.Inbounds.ListAuto(request);
            var list = new List<IInbound>();
            await foreach (var inbound in response)
            {
                inbound.Should().NotBeNull();
                list.Add(inbound);
            }

            list.Should().HaveCount(3);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }
    }
}
