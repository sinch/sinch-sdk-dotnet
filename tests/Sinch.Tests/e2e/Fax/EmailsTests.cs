using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Fax.Emails;
using Xunit;

namespace Sinch.Tests.e2e.Fax
{
    public class EmailsTests : FaxTestBase
    {
        [Fact]
        public async Task Add()
        {
            var response =
                await FaxClient.Emails.Add("email.example@hello.world", new[] { "+12015555555", "+12015555554" });

            response.Should().BeEquivalentTo(new EmailAddress()
            {
                Email = "email.example@hello.world",
                PhoneNumbers = new List<string>() { "+12015555555", "+12015555554" },
                ProjectId = ProjectId
            });
        }

        [Fact]
        public async Task Delete()
        {
            var response =
                () => FaxClient.Emails.Delete("hello@world.com");

            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Update()
        {
            var response = await FaxClient.Emails.Update("hello@world.com", new[] { "+12015555554" });

            response.Should().BeEquivalentTo(new EmailAddress()
            {
                Email = "hello@world.com",
                ProjectId = ProjectId,
                PhoneNumbers = new List<string>()
                {
                    "+12015555554"
                }
            });
        }

        [Fact]
        public async Task ListEmailsPlain()
        {
            var response = await FaxClient.Emails.ListForNumber("01HXGS1GE2SXS6HKQDMPYM1JHY", "+12015555554", pageNumber: 1,
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
        public async Task ListEmailsPlainAuto()
        {
            var emails = FaxClient.Emails.ListForNumberAuto("01HXGS1GE2SXS6HKQDMPYM1JHY", "+12015555554", pageNumber: 1,
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
        public async Task ListEmails()
        {
            var emails = await FaxClient.Emails.List();

            emails.Should().BeEquivalentTo(new ListEmailsResponse<EmailAddress>()
            {
                PageNumber = 1,
                TotalItems = 2,
                PageSize = 20,
                TotalPages = 1,
                Emails = new List<EmailAddress>()
                {
                    new EmailAddress()
                    {
                        PhoneNumbers = new List<string>()
                        {
                            "+12015555554",
                            "+12015555555"
                        },
                        ProjectId = ProjectId,
                        Email = "example@mail.com"
                    },
                    new EmailAddress()
                    {
                        PhoneNumbers = new List<string>()
                        {
                            "+12015555553",
                            "+12015555552"
                        },
                        ProjectId = ProjectId,
                        Email = "example2@mail.com"
                    },
                }
            });
        }

        [Fact]
        public async Task ListNumbers()
        {
            var response = await FaxClient.Emails.ListNumbers("example@mail.com", pageNumber: 1, pageSize: 1);
            response.Should().BeEquivalentTo(new ListNumbersResponse()
            {
                PhoneNumbers = new List<ServicePhoneNumber>()
                {
                    new ServicePhoneNumber()
                    {
                        ProjectId = ProjectId,
                        PhoneNumber = "+12015555554",
                        ServiceId = "01HXGS1GE2SXS6HKQDMPYM1JHY"
                    }
                },
                TotalItems = 2,
                TotalPages = 2,
                PageSize = 1,
                PageNumber = 1
            });
        }

        [Fact]
        public async Task ListNumbersAuto()
        {
            var response = FaxClient.Emails.ListNumbersAuto("example@mail.com", 1, 1);
            var counter = 0;
            await foreach (var number in response)
            {
                counter++;
                number.Should().NotBeNull();
            }

            counter.Should().Be(2);
        }
    }
}
