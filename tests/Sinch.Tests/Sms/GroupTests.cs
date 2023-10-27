using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.SMS.Groups;
using Sinch.SMS.Groups.List;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class GroupTests : SmsTestBase
    {
        private readonly object _group = new
        {
            id = "g1",
            name = "grouping",
            size = 4,
            created_at = "2019-08-24T14:15:22Z",
            modified_at = "2019-08-24T14:15:22Z",
            child_groups = new[] { "g3", "01FC66621XXXXX119Z8PMV1AHY" },
            auto_update = new
            {
                to = "125123123",
                add = "fadas",
                remove = "fadsfds"
            }
        };

        [Fact]
        public async Task Get()
        {
            const string groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var response = await Sms.Groups.Get(groupId);

            response.Should().NotBeNull();
            response.ChildGroups.Should().HaveCount(2);
        }

        [Fact]
        public async Task List()
        {
            HttpMessageHandlerMock.When(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups?page=1&page_size=2")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 1,
                    page_size = 2,
                    count = 3,
                    groups = new[] { _group, _group }
                }));

            var response = await Sms.Groups.List(new Request
            {
                Page = 1,
                PageSize = 2
            });

            response.Groups.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListAuto()
        {
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups")
                .WithQueryString("page", "0")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 0,
                    page_size = 1,
                    count = 3,
                    groups = new[] { _group, _group }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups")
                .WithQueryString("page", "1")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 1,
                    page_size = 1,
                    count = 3,
                    groups = new[] { _group, _group }
                }));
            HttpMessageHandlerMock
                .Expect(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups")
                .WithQueryString("page", "2")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    page = 2,
                    page_size = 1,
                    count = 3,
                    groups = new[] { _group, _group }
                }));
            var groups = Sms.Groups.ListAuto(new Request());
            var list = new List<Group>();
            await foreach (var group in groups)
            {
                group.Should().NotBeNull();
                list.Add(group);
            }

            list.Should().HaveCount(6);
            HttpMessageHandlerMock.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task Create()
        {
            HttpMessageHandlerMock.When(HttpMethod.Post,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("name_g")
                .WithPartialContent("ann")
                .WithPartialContent("child_groups")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var response = await Sms.Groups.Create(new SMS.Groups.Create.Request
            {
                Name = "name_g",
                Members = new List<string>() { "1", "2" },
                ChildGroups = new List<string>() { "3", "4" },
                AutoUpdate = new AutoUpdateEdit
                {
                    To = ":123",
                    Add = new AutoUpdateAdd
                    {
                        FirstWord = "biba",
                        SecondWord = "obba"
                    },
                    Remove = new AutoUpdateRemove
                    {
                        FirstWord = "ann",
                        SecondWord = "bloom"
                    }
                }
            });

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Update()
        {
            var groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Post,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(new
                {
                    name = "name_g",
                    auto_update = new
                    {
                        to = ":123",
                        add = new
                        {
                            first_word = "biba",
                            second_word = "obba"
                        },
                        remove = new
                        {
                            first_word = "ann",
                            second_word = "bloom"
                        }
                    }
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var response = await Sms.Groups.Update(new SMS.Groups.Update.Request
            {
                GroupId = "g1",
                Name = "name_g",
                AutoUpdate = new AutoUpdateEdit
                {
                    To = ":123",
                    Add = new AutoUpdateAdd
                    {
                        FirstWord = "biba",
                        SecondWord = "obba"
                    },
                    Remove = new AutoUpdateRemove
                    {
                        FirstWord = "ann",
                        SecondWord = "bloom"
                    }
                }
            });

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateRemoveName()
        {
            var groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Post,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("ann")
                .WithPartialContent("\"name\":null")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var request = new SMS.Groups.Update.Request
            {
                GroupId = "g1",
                Name = null,
                AutoUpdate = new AutoUpdateEdit
                {
                    To = ":123",
                    Add = new AutoUpdateAdd
                    {
                        FirstWord = "biba",
                        SecondWord = "obba"
                    },
                    Remove = new AutoUpdateRemove
                    {
                        FirstWord = "ann",
                        SecondWord = "bloom"
                    }
                }
            };
            var response = await Sms.Groups.Update(request);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task NoNameInProps()
        {
            var groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Post,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("ann")
                .WithPartialContent("\"name\":\"\"")
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var request = new SMS.Groups.Update.Request
            {
                GroupId = "g1",
                Name = string.Empty,
                AutoUpdate = new AutoUpdateEdit
                {
                    To = ":123",
                    Add = new AutoUpdateAdd
                    {
                        FirstWord = "biba",
                        SecondWord = "obba"
                    },
                    Remove = new AutoUpdateRemove
                    {
                        FirstWord = "ann",
                        SecondWord = "bloom"
                    }
                }
            };
            Func<Task<Group>> response = () => Sms.Groups.Update(request);

            await response.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Replace()
        {
            const string groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Put,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithPartialContent("\"name\":\"b\"")
                .WithPartialContent("\"members\":[\"123\",\"456\"]")
                .With(x => !x.Content!.ReadAsStringAsync().Result.Contains("group_id"))
                .Respond(HttpStatusCode.OK, JsonContent.Create(_group));

            var request = new SMS.Groups.Replace.Request
            {
                GroupId = "g1",
                Members = new List<string> { "123", "456" },
                Name = "b"
            };
            var response = await Sms.Groups.Replace(request);

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete()
        {
            var groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Delete,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK);

            Func<Task> response = () => Sms.Groups.Delete(groupId);

            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ListMembers()
        {
            var groupId = "g1";
            HttpMessageHandlerMock.When(HttpMethod.Get,
                    $"https://zt.us.sms.api.sinch.com/xms/v1/{ProjectId}/groups/{groupId}/members")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(new[]
                {
                    "123",
                    "456"
                }));

            var response = await Sms.Groups.ListMembers(groupId);

            response.Should().HaveCount(2);
        }
    }
}
