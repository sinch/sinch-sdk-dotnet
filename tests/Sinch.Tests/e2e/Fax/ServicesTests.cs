using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Fax.Emails;
using Sinch.Fax.Faxes;
using Sinch.Fax.Services;
using Xunit;

namespace Sinch.Tests.e2e.Fax
{
    public class ServicesTests : FaxTestBase
    {
        private readonly Service _service = new Service()
        {
            Id = "01GVRB50KEQFFE1SGMPFRNBG6J",
            ProjectId = ProjectId,
            Name = "hello-service",
            IncomingWebhookUrl = "http://service.webhook/handler",
            WebhookContentType = CallbackUrlContentType.ApplicationJson,
            DefaultForProject = true,
            DefaultFrom = "+12015555555",
            NumberOfRetries = 5,
            RetryDelaySeconds = 120,
            ImageConversionMethod = ImageConversionMethod.Halftone,
            SaveOutboundFaxDocuments = true,
            SaveInboundFaxDocuments = true
        };

        [Fact]
        public async Task Create()
        {
            var response = await FaxClient.Services.Create(new CreateServiceRequest()
            {
                Name = "hello-service",
                IncomingWebhookUrl = "http://service.webhook/handler",
                WebhookContentType = CallbackUrlContentType.ApplicationJson,
                DefaultForProject = true,
                DefaultFrom = "+12015555555",
                NumberOfRetries = 5,
                RetryDelaySeconds = 120,
                ImageConversionMethod = ImageConversionMethod.Halftone,
                SaveOutboundFaxDocuments = true,
                SaveInboundFaxDocuments = true
            });
            response.Should().BeEquivalentTo(_service);
        }

        [Fact]
        public async Task Get()
        {
            var response = await FaxClient.Services.Get(_service.Id!);
            response.Should().BeEquivalentTo(_service);
        }

        [Fact]
        public async Task Update()
        {
            var response = await FaxClient.Services.Update(new UpdateServiceRequest()
            {
                Id = _service.Id!,
                Name = "hello-service",
                IncomingWebhookUrl = "http://service.webhook/handler",
                WebhookContentType = CallbackUrlContentType.ApplicationJson,
                DefaultForProject = true,
                DefaultFrom = "+12015555555",
                NumberOfRetries = 5,
                RetryDelaySeconds = 120,
                ImageConversionMethod = ImageConversionMethod.Halftone,
                SaveOutboundFaxDocuments = true,
                SaveInboundFaxDocuments = true
            });
            response.Should().BeEquivalentTo(_service);
        }

        [Fact]
        public async Task Delete()
        {
            var op = () => FaxClient.Services.Delete(_service.Id!);
            await op.Should().NotThrowAsync();
        }

        [Fact]
        public async Task List()
        {
            var response = await FaxClient.Services.List(1, 2);
            response.Should().BeEquivalentTo(new ListServicesResponse()
            {
                Services = new List<Service>()
                {
                    _service, _service
                },
                PageNumber = 1,
                PageSize = 2,
                TotalItems = 2,
                TotalPages = 1,
            });
        }

        [Fact]
        public async Task ListForNumber()
        {
            var response = await FaxClient.Services.ListEmailsForNumber("01HXGS1GE2SXS6HKQDMPYM1JHY", "+12015555554",
                page: 1,
                pageSize: 2);
            response.Should().BeEquivalentTo(new ListEmailsResponse<string>()
            {
                Emails = new List<string>()
                {
                    "email@example.com", "hello@world.com"
                },
                TotalItems = 4,
                TotalPages = 2,
                PageSize = 2,
                PageNumber = 1
            });
        }

        [Fact]
        public async Task ListForNumberAuto()
        {
            var emails = FaxClient.Services.ListEmailsForNumberAuto("01HXGS1GE2SXS6HKQDMPYM1JHY", "+12015555554",
                page: 1,
                pageSize: 2);
            var counter = 0;
            await foreach (var email in emails)
            {
                counter++;
                email.Should().NotBeNull();
            }

            counter.Should().Be(4);
        }

        [Fact]
        public async Task ListNumbers()
        {
            var response = await FaxClient.Services.ListNumbers(_service.Id!, 1, 2);
            response.Should().BeEquivalentTo(new ListNumbersResponse()
            {
                PageNumber = 1,
                TotalPages = 1,
                PageSize = 2,
                TotalItems = 2,
                PhoneNumbers = new List<ServicePhoneNumber>()
                {
                    new ServicePhoneNumber()
                    {
                        ProjectId = ProjectId,
                        PhoneNumber = "+12015555554",
                        ServiceId = _service.Id
                    },
                    new ServicePhoneNumber()
                    {
                        ProjectId = ProjectId,
                        PhoneNumber = "+12015555555",
                        ServiceId = _service.Id
                    }
                }
            });
        }
    }
}
