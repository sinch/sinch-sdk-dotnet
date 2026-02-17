using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Fax.Services;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class ServicesTests : FaxTestBase
    {
        private const string ServiceId = "FAX_SERVICE_ID";
        private const string PhoneNumber = "+12025550134";
        private const string BaseServicesPath = $"/v3/projects/{ProjectId}/services";

        #region Create Tests

        [Fact]
        public async Task Create_WithValidRequest_ReturnsCreateService()
        {
            var request = new CreateFaxServiceRequest
            {
                Name = "Test Service",
                IncomingWebhookUrl = "https://example.com/fax"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseServicesPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("Test Service")
                .Respond(HttpStatusCode.Created, JsonContent.Create(new
                {
                    id = ServiceId,
                    name = "Test Service",
                    incomingWebhookUrl = "https://example.com/fax",
                    projectId = ProjectId
                }));

            var response = await Fax.Services.Create(request);

            response.Should().NotBeNull();
            response.Id.Should().Be(ServiceId);
            response.Name.Should().Be("Test Service");
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_WithValidServiceId_ReturnsService()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ServiceId,
                    name = "Service Name",
                    projectId = ProjectId
                }));

            var response = await Fax.Services.Get(ServiceId);

            response.Should().NotBeNull();
            response.Id.Should().Be(ServiceId);
            response.Name.Should().Be("Service Name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Get_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.Get(invalidServiceId));

            exception.ParamName.Should().Be("serviceId");
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidRequest_ReturnsUpdatedService()
        {
            var request = new UpdateFaxServiceRequest
            {
                Id = ServiceId,
                Name = "Updated Service"
            };

            HttpMessageHandlerMock
                .When(HttpMethod.Patch, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("Updated Service")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    id = ServiceId,
                    name = "Updated Service",
                    projectId = ProjectId
                }));

            var response = await Fax.Services.Update(request);

            response.Should().NotBeNull();
            response.Id.Should().Be(ServiceId);
            response.Name.Should().Be("Updated Service");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Update_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var request = new UpdateFaxServiceRequest
            {
                Id = invalidServiceId!
            };

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.Update(request));

            exception.ParamName.Should().Be("Id");
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidServiceId_DeletesServiceSuccessfully()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.NoContent);

            await Fax.Services.Delete(ServiceId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Delete_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.Delete(invalidServiceId));

            exception.ParamName.Should().Be("serviceId");
        }

        #endregion

        #region List Tests

        [Fact]
        public async Task List_WithPageAndPageSize_ReturnsPagedResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseServicesPath}?page=2&pageSize=5")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    services = new[] { new { id = ServiceId, name = "Service" } },
                    page = 2,
                    pageSize = 5,
                    totalItems = 12,
                    totalPages = 3
                }));

            var response = await Fax.Services.List(page: 2, pageSize: 5);

            response.Should().NotBeNull();
            response.Page.Should().Be(2);
            response.PageSize.Should().Be(5);
            response.TotalItems.Should().Be(12);
            response.TotalPages.Should().Be(3);
            response.Services.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseServicesPath}";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    services = new[] { new { id = "service-1", name = "First" } },
                    page = 1,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));
            HttpMessageHandlerMock
                .Expect($"{baseUri}?page=2&pageSize=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    services = new[] { new { id = "service-2", name = "Second" } },
                    page = 2,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));

            var services = new List<Service>();
            await foreach (var service in Fax.Services.ListAuto(pageSize: 1))
            {
                services.Add(service);
            }

            services.Should().HaveCount(2);
            services[0].Id.Should().Be("service-1");
            services[1].Id.Should().Be("service-2");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region ListEmailsForNumber Tests

        [Fact]
        public async Task ListEmailsForNumber_WithValidParams_ReturnsEmailsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers/{PhoneNumber}/emails")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test1@example.com", "test2@example.com" },
                    page = 1,
                    pageSize = 20,
                    totalItems = 2,
                    totalPages = 1
                }));

            var response = await Fax.Services.ListEmailsForNumber(ServiceId, PhoneNumber);

            response.Should().NotBeNull();
            response.EmailAddresses.Should().HaveCount(2);
            response.EmailAddresses[0].Should().Be("test1@example.com");
            response.EmailAddresses[1].Should().Be("test2@example.com");
        }

        [Fact]
        public async Task ListEmailsForNumber_WithPageAndPageSize_ReturnsPagedResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get,
                    $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers/{PhoneNumber}/emails?page=3&pageSize=10")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test@example.com" },
                    page = 3,
                    pageSize = 10,
                    totalItems = 21,
                    totalPages = 3
                }));

            var response = await Fax.Services.ListEmailsForNumber(ServiceId, PhoneNumber, page: 3, pageSize: 10);

            response.Should().NotBeNull();
            response.Page.Should().Be(3);
            response.PageSize.Should().Be(10);
            response.TotalItems.Should().Be(21);
            response.TotalPages.Should().Be(3);
            response.EmailAddresses.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListEmailsForNumber_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.ListEmailsForNumber(invalidServiceId, PhoneNumber));

            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListEmailsForNumber_WithNullOrEmptyPhoneNumber_ThrowsException(string invalidPhoneNumber)
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.ListEmailsForNumber(ServiceId, invalidPhoneNumber));

            exception.ParamName.Should().Be("phoneNumber");
        }

        [Fact]
        public async Task ListEmailsForNumberAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers/{PhoneNumber}/emails";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test1@example.com" },
                    page = 1,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));
            HttpMessageHandlerMock
                .Expect($"{baseUri}?page=2&pageSize=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test2@example.com" },
                    page = 2,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));

            var emails = new List<string>();
            await foreach (var email in Fax.Services.ListEmailsForNumberAuto(ServiceId, PhoneNumber, pageSize: 1))
            {
                emails.Add(email);
            }

            emails.Should().HaveCount(2);
            emails[0].Should().Be("test1@example.com");
            emails[1].Should().Be("test2@example.com");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region ListNumbers Tests

        [Fact]
        public async Task ListNumbers_WithValidParams_ReturnsNumbersResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    numbers = new[]
                    {
                        new { phoneNumber = PhoneNumber, projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 1,
                    pageSize = 20,
                    totalItems = 1,
                    totalPages = 1
                }));

            var response = await Fax.Services.ListNumbers(ServiceId);

            response.Should().NotBeNull();
            response.PhoneNumbers.Should().HaveCount(1);
            response.PhoneNumbers[0].PhoneNumber.Should().Be(PhoneNumber);
            response.Page.Should().Be(1);
        }

        [Fact]
        public async Task ListNumbers_WithPageAndPageSize_ReturnsPagedResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers?page=2&pageSize=10")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    numbers = new[]
                    {
                        new { phoneNumber = PhoneNumber, projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 2,
                    pageSize = 10,
                    totalItems = 11,
                    totalPages = 2
                }));

            var response = await Fax.Services.ListNumbers(ServiceId, page: 2, pageSize: 10);

            response.Should().NotBeNull();
            response.Page.Should().Be(2);
            response.PageSize.Should().Be(10);
            response.TotalItems.Should().Be(11);
            response.TotalPages.Should().Be(2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListNumbers_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Services.ListNumbers(invalidServiceId));

            exception.ParamName.Should().Be("serviceId");
        }

        [Fact]
        public async Task ListNumbersAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseServicesPath}/{ServiceId}/numbers";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    numbers = new[]
                    {
                        new { phoneNumber = "+12025550134", projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 1,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));
            HttpMessageHandlerMock
                .Expect($"{baseUri}?page=2&pageSize=1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    numbers = new[]
                    {
                        new { phoneNumber = "+12025550135", projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 2,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));

            var numbers = new List<ServicePhoneNumber>();
            await foreach (var number in Fax.Services.ListNumbersAuto(ServiceId, pageSize: 1))
            {
                numbers.Add(number);
            }

            numbers.Should().HaveCount(2);
            numbers[0].PhoneNumber.Should().Be("+12025550134");
            numbers[1].PhoneNumber.Should().Be("+12025550135");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion
    }
}
