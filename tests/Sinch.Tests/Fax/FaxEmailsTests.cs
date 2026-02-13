using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Fax.Emails;
using Xunit;

namespace Sinch.Tests.Fax
{
    public class FaxEmailsTests : FaxTestBase
    {
        private const string ServiceId = "FAX_SERVICE_ID";
        private const string PhoneNumber = "+12025550134";
        private const string Email = "test_email@sinch.com";
        private const string BaseEmailsPath = $"/v3/projects/{ProjectId}/services/{ServiceId}/emails";

        #region List Tests

        [Fact]
        public async Task List_WithValidServiceId_ReturnsEmailsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseEmailsPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[]
                    {
                        new
                        {
                            email = "test1@example.com",
                            phoneNumbers = new[]
                            {
                                new { number = "+12025550134", permissions = "both" }
                            },
                            projectId = ProjectId
                        }
                    },
                    page = 1,
                    pageSize = 20,
                    totalItems = 1,
                    totalPages = 1
                }));

            var response = await Fax.Emails.List(ServiceId);

            response.Should().NotBeNull();
            response.Emails.Should().HaveCount(1);
            response.Emails[0].Email.Should().Be("test1@example.com");
            response.Page.Should().Be(1);
            response.PageSize.Should().Be(20);
            response.TotalItems.Should().Be(1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task List_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            ArgumentException exception;

            if (invalidServiceId == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.List(invalidServiceId));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.List(invalidServiceId));
            }

            exception.ParamName.Should().Be("serviceId");
        }

        [Fact]
        public async Task List_WithPageAndPageSize_IncludesQueryParameters()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseEmailsPath}?page=2&pageSize=5")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[]
                    {
                        new
                        {
                            email = "test@example.com",
                            phoneNumbers = new object[] { },
                            projectId = ProjectId
                        }
                    },
                    page = 2,
                    pageSize = 5,
                    totalItems = 15,
                    totalPages = 3
                }));

            var response = await Fax.Emails.List(ServiceId, page: 2, pageSize: 5);

            response.Should().NotBeNull();
            response.Page.Should().Be(2);
            response.PageSize.Should().Be(5);
            response.TotalItems.Should().Be(15);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task ListAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseEmailsPath}";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[]
                    {
                        new { email = "test1@example.com", phoneNumbers = new object[] { }, projectId = ProjectId }
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
                    emails = new[]
                    {
                        new { email = "test2@example.com", phoneNumbers = new object[] { }, projectId = ProjectId }
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
                    emails = new[]
                    {
                        new { email = "test3@example.com", phoneNumbers = new object[] { }, projectId = ProjectId }
                    },
                    page = 3,
                    pageSize = 1,
                    totalItems = 3,
                    totalPages = 3
                }));

            var emails = new List<EmailAddress>();
            await foreach (var email in Fax.Emails.ListAuto(ServiceId, pageSize: 1))
            {
                emails.Add(email);
            }

            emails.Should().HaveCount(3);
            emails[0].Email.Should().Be("test1@example.com");
            emails[1].Email.Should().Be("test2@example.com");
            emails[2].Email.Should().Be("test3@example.com");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region Add Tests

        [Fact]
        public async Task Add_WithValidRequest_SendsPostRequest()
        {
            var phoneNumbers = new List<NumberWithPermissions>
            {
                new() { Number = PhoneNumber }
            };
            var emailRequest = new EmailRequest { Email = Email, PhoneNumbers = phoneNumbers };

            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://fax.api.sinch.com{BaseEmailsPath}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent(Email)
                .Respond(HttpStatusCode.Created, JsonContent.Create(new
                {
                    email = Email,
                    phoneNumbers = new[]
                    {
                        new { number = PhoneNumber, permissions = "both" }
                    },
                    projectId = ProjectId
                }));

            var response = await Fax.Emails.Add(ServiceId, emailRequest);

            response.Should().NotBeNull();
            response.Email.Should().Be(Email);
            response.PhoneNumbers.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Add_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var emailRequest = new EmailRequest
            {
                Email = Email,
                PhoneNumbers = new List<NumberWithPermissions> { new() { Number = PhoneNumber } }
            };

            ArgumentException exception;

            if (invalidServiceId == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Add(invalidServiceId, emailRequest));

            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Add(invalidServiceId, emailRequest));
            }

            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Add_WithNullOrEmptyEmail_ThrowsException(string invalidEmail)
        {
            var emailRequest = new EmailRequest
            {
                Email = invalidEmail,
                PhoneNumbers = [new() { Number = PhoneNumber }]
            };

            ArgumentException exception;

            if (invalidEmail == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Add(ServiceId, emailRequest));

            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Add(ServiceId, emailRequest));
            }

            exception.ParamName.Should().Contain("Email");
        }

        [Fact]
        public async Task Add_WithEmptyPhoneNumbers_ThrowsInvalidOperationException()
        {
            var emailRequest = new EmailRequest { Email = Email, PhoneNumbers = [] };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await Fax.Emails.Add(ServiceId, emailRequest));

            exception.Message.Should().Contain("Phone numbers list should have at least one record");
        }

        [Fact]
        public async Task Add_WithNullEmailRequest_ThrowsArgumentNullException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Emails.Add(ServiceId, null!));

            exception.ParamName.Should().Be("emailRequest");
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidRequest_SendsPutRequest()
        {
            var phoneNumbers = new List<NumberWithPermissions>
            {
                new() { Number = "+12025550135" }
            };
            var updateRequest = new UpdateEmailRequest { PhoneNumbers = phoneNumbers };

            HttpMessageHandlerMock
                .When(HttpMethod.Put, $"https://fax.api.sinch.com{BaseEmailsPath}/{Email}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    email = Email,
                    phoneNumbers = new[]
                    {
                        new { number = "+12025550135", permissions = "both" }
                    },
                    projectId = ProjectId
                }));

            var response = await Fax.Emails.Update(ServiceId, Email, updateRequest);

            response.Should().NotBeNull();
            response.Email.Should().Be(Email);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Update_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            var updateRequest = new UpdateEmailRequest
            {
                PhoneNumbers = new List<NumberWithPermissions> { new() { Number = PhoneNumber } }
            };

            ArgumentException exception;

            if (invalidServiceId == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Update(invalidServiceId, Email, updateRequest));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Update(invalidServiceId, Email, updateRequest));
            }

            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Update_WithNullOrEmptyEmail_ThrowsException(string invalidEmail)
        {
            var updateRequest = new UpdateEmailRequest
            {
                PhoneNumbers = new List<NumberWithPermissions> { new() { Number = PhoneNumber } }
            };

            ArgumentException exception;

            if (invalidEmail == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Update(ServiceId, invalidEmail, updateRequest));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Update(ServiceId, invalidEmail, updateRequest));
            }

            exception.ParamName.Should().Be("email");
        }

        [Fact]
        public async Task Update_WithEmptyPhoneNumbers_ThrowsInvalidOperationException()
        {
            var updateRequest = new UpdateEmailRequest { PhoneNumbers = [] };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await Fax.Emails.Update(ServiceId, Email, updateRequest));

            exception.Message.Should().Contain("Phone numbers list should have at least one record");
        }

        [Fact]
        public async Task Update_WithNullUpdateRequest_ThrowsArgumentNullException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Emails.Update(ServiceId, Email, null!));

            exception.ParamName.Should().Be("updateRequest");
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidParams_SendsDeleteRequest()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Delete, $"https://fax.api.sinch.com{BaseEmailsPath}/{Email}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.NoContent);

            await Fax.Emails.Delete(ServiceId, Email);

            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Delete_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            ArgumentException exception;

            if (invalidServiceId == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Delete(invalidServiceId, Email));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Delete(invalidServiceId, Email));
            }

            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Delete_WithNullOrEmptyEmail_ThrowsException(string invalidEmail)
        {
            ArgumentException exception;

            if (invalidEmail == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.Delete(ServiceId, invalidEmail));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.Delete(ServiceId, invalidEmail));
            }

            exception.ParamName.Should().Be("email");
        }

        #endregion

        #region ListNumbers Tests

        [Fact]
        public async Task ListNumbers_WithValidParams_ReturnsNumbersResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseEmailsPath}/{Email}/numbers")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    phoneNumbers = new[]
                    {
                        new { phoneNumber = PhoneNumber, projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 1,
                    pageSize = 20,
                    totalItems = 1,
                    totalPages = 1
                }));

            var response = await Fax.Emails.ListNumbers(ServiceId, Email);

            response.Should().NotBeNull();
            response.PhoneNumbers.Should().HaveCount(1);
            response.Page.Should().Be(1);
        }

        [Fact]
        public async Task ListNumbers_WithPageAndPageSize_IncludesQueryParameters()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com{BaseEmailsPath}/{Email}/numbers?page=3&pageSize=10")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    phoneNumbers = new[]
                    {
                        new { phoneNumber = PhoneNumber, projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 3,
                    pageSize = 10,
                    totalItems = 25,
                    totalPages = 3
                }));

            var response = await Fax.Emails.ListNumbers(ServiceId, Email, page: 3, pageSize: 10);

            response.Should().NotBeNull();
            response.Page.Should().Be(3);
            response.PageSize.Should().Be(10);
            response.TotalItems.Should().Be(25);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListNumbers_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            ArgumentException exception;

            if (invalidServiceId == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.ListNumbers(invalidServiceId, Email));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.ListNumbers(invalidServiceId, Email));
            }

            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListNumbers_WithNullOrEmptyEmail_ThrowsException(string invalidEmail)
        {
            ArgumentException exception;

            if (invalidEmail == null)
            {
                exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await Fax.Emails.ListNumbers(ServiceId, invalidEmail));
            }
            else
            {
                exception = await Assert.ThrowsAsync<ArgumentException>(
                    async () => await Fax.Emails.ListNumbers(ServiceId, invalidEmail));
            }

            exception.ParamName.Should().Be("email");
        }

        [Fact]
        public async Task ListNumbersAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com{BaseEmailsPath}/{Email}/numbers";
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get, baseUri)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    phoneNumbers = new[]
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
                    phoneNumbers = new[]
                    {
                        new { phoneNumber = "+12025550135", projectId = ProjectId, serviceId = ServiceId }
                    },
                    page = 2,
                    pageSize = 1,
                    totalItems = 2,
                    totalPages = 2
                }));

            var numbers = new List<ServicePhoneNumber>();
            await foreach (var number in Fax.Emails.ListNumbersAuto(ServiceId, Email, pageSize: 1))
            {
                numbers.Add(number);
            }

            numbers.Should().HaveCount(2);
            numbers[0].PhoneNumber.Should().Be("+12025550134");
            numbers[1].PhoneNumber.Should().Be("+12025550135");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion

        #region ListEmailsForNumber Tests

        [Fact]
        public async Task ListForNumber_WithValidParams_DelegatesAndReturnsResponse()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com/v3/projects/{ProjectId}/services/{ServiceId}/numbers/{PhoneNumber}/emails")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test1@example.com", "test2@example.com" },
                    page = 1,
                    pageSize = 20,
                    totalItems = 2,
                    totalPages = 1
                }));

            var response = await Fax.Emails.ListForNumber(ServiceId, PhoneNumber);

            response.Should().NotBeNull();
            response.Emails.Should().HaveCount(2);
            response.Emails[0].Should().Be("test1@example.com");
            response.Emails[1].Should().Be("test2@example.com");
        }

        [Fact]
        public async Task ListForNumber_WithPageAndPageSize_IncludesQueryParameters()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://fax.api.sinch.com/v3/projects/{ProjectId}/services/{ServiceId}/numbers/{PhoneNumber}/emails?page=1&pageSize=5")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    emails = new[] { "test1@example.com", "test2@example.com" },
                    page = 1,
                    pageSize = 5,
                    totalItems = 8,
                    totalPages = 2
                }));

            var response = await Fax.Emails.ListForNumber(ServiceId, PhoneNumber, page: 1, pageSize: 5);

            response.Should().NotBeNull();
            response.Page.Should().Be(1);
            response.PageSize.Should().Be(5);
            response.TotalItems.Should().Be(8);
            response.Emails.Should().HaveCount(2);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListForNumber_WithNullOrEmptyServiceId_ThrowsException(string invalidServiceId)
        {
            // Both null and empty throw ArgumentNullException via ExceptionUtils.CheckEmptyString
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Emails.ListForNumber(invalidServiceId, PhoneNumber));
            exception.ParamName.Should().Be("serviceId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ListForNumber_WithNullOrEmptyPhoneNumber_ThrowsException(string invalidPhoneNumber)
        {
            // Both null and empty throw ArgumentNullException via ExceptionUtils.CheckEmptyString
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await Fax.Emails.ListForNumber(ServiceId, invalidPhoneNumber));
            exception.ParamName.Should().Be("phoneNumber");
        }

        [Fact]
        public async Task ListForNumberAuto_WithMultiplePages_IteratesThroughAllPages()
        {
            var baseUri = $"https://fax.api.sinch.com/v3/projects/{ProjectId}/services/{ServiceId}/numbers/{PhoneNumber}/emails";
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
            await foreach (var email in Fax.Emails.ListForNumberAuto(ServiceId, PhoneNumber, pageSize: 1))
            {
                emails.Add(email);
            }

            emails.Should().HaveCount(2);
            emails[0].Should().Be("test1@example.com");
            emails[1].Should().Be("test2@example.com");
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        #endregion
    }
}
