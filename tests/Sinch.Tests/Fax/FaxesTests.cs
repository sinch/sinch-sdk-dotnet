using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Fax.Faxes;
using Xunit;
using FaxModel = Sinch.Fax.Faxes.Fax;

namespace Sinch.Tests.Fax
{
    public class FaxesTests : FaxTestBase
    {
        private const string BaseFaxesPath = $"/v3/projects/{ProjectId}/faxes";

        #region Send Tests

        [Fact]
        public async Task Send_WithContentUrlAndSingleRecipient_ReturnsListWithSingleFaxObject()
        {
            var request = new SendFaxRequest
            {
                To = new List<string> { "+12015555555" },
                ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "01W4FFL35P4NC4K35URLSINGLE1",
                    to = "+12015555555",
                    from = "+12014444444",
                    contentUrl = new[] { "https://developers.sinch.com/fax/fax.pdf" },
                    direction = "OUTBOUND",
                    status = "PENDING",
                    numberOfPages = 1,
                    createTime = "2024-06-06T14:42:42Z",
                    projectId = ProjectId
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(1);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35URLSINGLE1");
            response[0].To.Should().Be("+12015555555");
            response[0].ContentUrl.Should().Contain("https://developers.sinch.com/fax/fax.pdf");
        }

        [Fact]
        public async Task Send_WithContentUrlAndMultipleRecipients_ReturnsListWithMultipleFaxObjects()
        {
            var request = new SendFaxRequest
            {
                To = new List<string> { "+12015555555", "+12016666666" },
                ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35URLMULTI01",
                            to = "+12015555555",
                            from = "+12014444444",
                            contentUrl = new[] { "https://developers.sinch.com/fax/fax.pdf" },
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        },
                        new
                        {
                            id = "01W4FFL35P4NC4K35URLMULTI02",
                            to = "+12016666666",
                            from = "+12014444444",
                            contentUrl = new[] { "https://developers.sinch.com/fax/fax.pdf" },
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        }
                    }
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(2);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35URLMULTI01");
            response[1].Id.Should().Be("01W4FFL35P4NC4K35URLMULTI02");
        }

        [Fact]
        public async Task Send_WithBinaryFileAttachmentAndSingleRecipient_ReturnsListWithSingleFaxObject()
        {
            var fileBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            await using var stream = new MemoryStream(fileBytes);

            using var request = SendFaxRequest.FromStream(stream, "sinch-logo.png");
            request.To = new List<string> { "+12015555555" };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "01W4FFL35P4NC4K35BINSINGLE1",
                    to = "+12015555555",
                    from = "+12014444444",
                    direction = "OUTBOUND",
                    status = "PENDING",
                    numberOfPages = 1,
                    createTime = "2024-06-06T14:42:42Z",
                    projectId = ProjectId,
                    hasFile = true
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(1);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35BINSINGLE1");
            response[0].HasFile.Should().BeTrue();
        }

        [Fact]
        public async Task Send_WithBinaryFileAttachmentAndMultipleRecipients_ReturnsListWithMultipleFaxObjects()
        {
            var fileBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            await using var stream = new MemoryStream(fileBytes);

            using var request = SendFaxRequest.FromStream(stream, "sinch-logo.png");
            request.To = new List<string> { "+12015555555", "+12016666666" };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35BINMULTI01",
                            to = "+12015555555",
                            from = "+12014444444",
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId,
                            hasFile = true
                        },
                        new
                        {
                            id = "01W4FFL35P4NC4K35BINMULTI02",
                            to = "+12016666666",
                            from = "+12014444444",
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId,
                            hasFile = true
                        }
                    }
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(2);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35BINMULTI01");
            response[1].Id.Should().Be("01W4FFL35P4NC4K35BINMULTI02");
        }

        [Fact]
        public async Task Send_WithBase64FileAndSingleRecipient_ReturnsListWithSingleFaxObject()
        {
            var files = new List<Base64File>
            {
                new()
                {
                    File =
                        "WSdhIGRlcyBqb3VycywgZmF1dCBwYXMgbSdjaGVyY2hlciAhIEV0IHknYSBkZXMgam91cnMgdG91cyBsZXMgam91cnMgIQ==",
                    FileType = FileType.PDF
                },

                new()
                {
                    File = "UXVhbmQgbGUgdHJvbGwgcGFybGUsIGwnaG9tbWUgYXZpc8OpIGwnw6ljb3V0ZQ==",
                    FileType = FileType.PDF
                }
            };

            var request = SendFaxRequest.WithFiles(files);
            request.To = new List<string> { "+12015555555" };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = "01W4FFL35P4NC4K35B64SINGLE1",
                    to = "+12015555555",
                    from = "+12014444444",
                    direction = "OUTBOUND",
                    status = "PENDING",
                    numberOfPages = 1,
                    createTime = "2024-06-06T14:42:42Z",
                    projectId = ProjectId
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(1);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35B64SINGLE1");
        }

        [Fact]
        public async Task Send_WithBase64FileAndMultipleRecipients_ReturnsListWithMultipleFaxObjects()
        {
            var files = new List<Base64File>
            {
                new()
                {
                    File = "WSdhIGRlcyBqb3VycywgZmF1dCBwYXMgbSdjaGVyY2hlciAhIEV0IHknYSBkZXMgam91cnMgdG91cyBsZXMgam91cnMgIQ==",
                    FileType = FileType.PDF
                },
                new()
                {
                    File = "UXVhbmQgbGUgdHJvbGwgcGFybGUsIGwnaG9tbWUgYXZpc8OpIGwnw6ljb3V0ZQ==",
                    FileType = FileType.PDF
                }
            };

            var request = SendFaxRequest.WithFiles(files);
            request.To = new List<string> { "+12015555555", "+12016666666" };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseFaxesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35B64MULTI01",
                            to = "+12015555555",
                            from = "+12014444444",
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        },
                        new
                        {
                            id = "01W4FFL35P4NC4K35B64MULTI02",
                            to = "+12016666666",
                            from = "+12014444444",
                            direction = "OUTBOUND",
                            status = "PENDING",
                            numberOfPages = 1,
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        }
                    }
                }));

            var response = await Fax.Faxes.Send(request);

            response.Should().HaveCount(2);
            response[0].Id.Should().Be("01W4FFL35P4NC4K35B64MULTI01");
            response[1].Id.Should().Be("01W4FFL35P4NC4K35B64MULTI02");
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidFaxId_ReturnsFaxObject()
        {
            const string faxId = "01W4FFL35P4NC4K35CR3P35M1N1";

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseFaxesPath}/{faxId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = faxId,
                    direction = "OUTBOUND",
                    from = "+12014444444",
                    to = "+12015555555",
                    numberOfPages = 1,
                    status = "COMPLETED",
                    headerTimeZone = "America/New_York",
                    retryDelaySeconds = 60,
                    callbackUrlContentType = "multipart/form-data",
                    projectId = ProjectId,
                    serviceId = "01K1TTENC4TSJ0LLYJ1GGLYJU1Y",
                    price = new { currencyCode = "USD", amount = 0.07f },
                    maxRetries = 3,
                    createTime = "2024-06-06T14:42:42Z",
                    completedTime = "2024-06-06T14:43:17Z",
                    headerPageNumbers = true,
                    contentUrl = new[] { "https://developers.sinch.com/fax/fax.pdf" },
                    imageConversionMethod = "MONOCHROME",
                    hasFile = true
                }));

            var fax = await Fax.Faxes.Get(faxId);

            fax.Should().NotBeNull();
            fax.Id.Should().Be(faxId);
            fax.Direction.Should().Be(Direction.Outbound);
            fax.From.Should().Be("+12014444444");
            fax.To.Should().Be("+12015555555");
            fax.NumberOfPages.Should().Be(1);
            fax.Status.Should().Be(FaxStatus.Completed);
            fax.HeaderTimeZone.Should().Be("America/New_York");
            fax.RetryDelaySeconds.Should().Be(60);
            fax.CallbackUrlContentType.Should().Be(CallbackUrlContentType.MultipartFormData);
            fax.ProjectId.Should().Be(ProjectId);
            fax.ServiceId.Should().Be("01K1TTENC4TSJ0LLYJ1GGLYJU1Y");
            fax.Price.Should().NotBeNull();
            fax.Price!.CurrencyCode.Should().Be("USD");
            fax.Price.Amount.Should().BeApproximately(0.07f, 0.0001f);
            fax.MaxRetries.Should().Be(3);
            fax.CreateTime.Should().Be(DateTimeOffset.Parse("2024-06-06T14:42:42Z").UtcDateTime);
            fax.CompletedTime.Should().Be(DateTimeOffset.Parse("2024-06-06T14:43:17Z").UtcDateTime);
            fax.HeaderPageNumbers.Should().BeTrue();
            fax.ContentUrl.Should().NotBeNull();
            fax.ContentUrl![0].Should().Be("https://developers.sinch.com/fax/fax.pdf");
            fax.ImageConversionMethod.Should().Be(ImageConversionMethod.Monochrome);
            fax.HasFile.Should().BeTrue();
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithValidRequest_ReturnsFaxesResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseFaxesPath}?pageSize=2")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID01",
                            direction = "OUTBOUND",
                            from = "+12014444444",
                            to = "+12015555555",
                            numberOfPages = 1,
                            status = "COMPLETED",
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        },
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID02",
                            direction = "INBOUND",
                            from = "+12015555555",
                            to = "+12014444444",
                            numberOfPages = 2,
                            status = "COMPLETED",
                            createTime = "2024-06-06T15:42:42Z",
                            projectId = ProjectId
                        }
                    },
                    page = 1,
                    pageSize = 2,
                    totalItems = 5,
                    totalPages = 3
                }));

            var response = await Fax.Faxes.List(new ListFaxesRequest { PageSize = 2 });

            response.Should().NotBeNull();
            response.Faxes.Should().HaveCount(2);
            response.Page.Should().Be(1);
            response.PageSize.Should().Be(2);
            response.TotalItems.Should().Be(5);
            response.TotalPages.Should().Be(3);
            response.Faxes[0].Id.Should().Be("01W4FFL35P4NC4K35FAXID01");
            response.Faxes[1].Id.Should().Be("01W4FFL35P4NC4K35FAXID02");
        }

        [Fact]
        public async Task List_WithPageAndPageSize_ReturnsPagedResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseFaxesPath}?page=2&pageSize=5")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID03",
                            direction = "OUTBOUND",
                            from = "+12014444444",
                            to = "+12015555555",
                            numberOfPages = 1,
                            status = "COMPLETED",
                            createTime = "2024-06-06T16:42:42Z",
                            projectId = ProjectId
                        }
                    },
                    page = 2,
                    pageSize = 5,
                    totalItems = 8,
                    totalPages = 2
                }));

            var response = await Fax.Faxes.List(new ListFaxesRequest { Page = 2, PageSize = 5 });

            response.Should().NotBeNull();
            response.Page.Should().Be(2);
            response.PageSize.Should().Be(5);
            response.TotalItems.Should().Be(8);
            response.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseFaxesPath}";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID01",
                            direction = "OUTBOUND",
                            from = "+12014444444",
                            to = "+12015555555",
                            numberOfPages = 1,
                            status = "COMPLETED",
                            createTime = "2024-06-06T14:42:42Z",
                            projectId = ProjectId
                        }
                    },
                    page = 1,
                    pageSize = 1,
                    totalItems = 3,
                    totalPages = 3
                }));
            HttpMessageHandlerMock
                .Expect($"{baseUri}?page=2&pageSize=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID02",
                            direction = "INBOUND",
                            from = "+12015555555",
                            to = "+12014444444",
                            numberOfPages = 2,
                            status = "COMPLETED",
                            createTime = "2024-06-06T15:42:42Z",
                            projectId = ProjectId
                        }
                    },
                    page = 2,
                    pageSize = 1,
                    totalItems = 3,
                    totalPages = 3
                }));
            HttpMessageHandlerMock
                .Expect($"{baseUri}?page=3&pageSize=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    faxes = new[]
                    {
                        new
                        {
                            id = "01W4FFL35P4NC4K35FAXID03",
                            direction = "OUTBOUND",
                            from = "+12014444444",
                            to = "+12015555555",
                            numberOfPages = 1,
                            status = "PENDING",
                            createTime = "2024-06-06T16:42:42Z",
                            projectId = ProjectId
                        }
                    },
                    page = 3,
                    pageSize = 1,
                    totalItems = 3,
                    totalPages = 3
                }));

            var faxes = new List<FaxModel>();
            await foreach (var fax in Fax.Faxes.ListAuto(new ListFaxesRequest { PageSize = 1 }))
            {
                faxes.Add(fax);
            }

            faxes.Should().HaveCount(3);
            faxes[0].Id.Should().Be("01W4FFL35P4NC4K35FAXID01");
            faxes[1].Id.Should().Be("01W4FFL35P4NC4K35FAXID02");
            faxes[2].Id.Should().Be("01W4FFL35P4NC4K35FAXID03");

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region DownloadContent Tests

        [Fact]
        public async Task DownloadContent_WithValidFaxId_ReturnsContentResult()
        {
            const string faxId = "01W4FFL35P4NC4K35CR3P35DWLD";

            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseFaxesPath}/{faxId}/file")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(req =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(new byte[] { 37, 80, 68, 70 });
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{faxId}.pdf"
                    };
                    return response;
                });

            var result = await Fax.Faxes.DownloadContent(faxId);

            result.Should().NotBeNull();
            result.FileName.Should().Be($"{faxId}.pdf");
        }

        #endregion

        #region DeleteContent Tests

        [Fact]
        public async Task DeleteContent_WithValidFaxId_SendsDeleteRequest()
        {
            const string faxId = "01W4FFL35P4NC4K35CR3P35DEL0";

            HttpMessageHandlerMock
                .Expect(HttpMethod.Delete, $"https://fax.api.sinch.com{BaseFaxesPath}/{faxId}/file")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.NoContent);

            await Fax.Faxes.DeleteContent(faxId);

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion
    }
}
