using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.SMS.Groups.Create;
using Xunit;

namespace Sinch.Tests.e2e.Sms
{
    public class GroupTests : TestBase
    {
        [Fact]
        public async Task Create()
        {
            var response = await SinchClientMockStudio.Sms.Groups.Create(new CreateGroupRequest
            {
                Name = "KillerRabbit",
                Members = new List<string>()
                {
                    "+48 737532793"
                },
            });
            response.Name.Should().Be("KillerRabbit");
        }

        [Fact]
        public async Task Get()
        {
            var response = await SinchClientMockStudio.Sms.Groups.Get("01GJFY2CEJQ0Y20704G8G506N9");
            response.Name.Should().Be("WithChildGroups");
            response.ChildGroups.First().Should().Be("01GJFY2B2M6NXH1GS96QGG0K2K");
        }

        [Fact]
        public async Task Update()
        {
            var response = await SinchClientMockStudio.Sms.Groups.Update(new SMS.Groups.Update.UpdateGroupRequest
            {
                GroupId = "01GJFY2CEJQ0Y20704G8G506N9",
                Name = "KillerRabbit222",
            });
            response.Name.Should().Be("KillerRabbit222");
        }

        [Fact]
        public async Task Replace()
        {
            var response = await SinchClientMockStudio.Sms.Groups.Replace(new SMS.Groups.Replace.ReplaceGroupRequest
            {
                GroupId = "01GJFY2CEJQ0Y20704G8G506N9",
                Members = new List<string>()
                {
                    "48111111111"
                },
            });
            response.Size.Should().Be(1);
        }

        [Fact]
        public async Task Delete()
        {
            Func<Task> response = () => SinchClientMockStudio.Sms.Groups.Delete("01GJFY2CEJQ0Y20704G8G506N9");
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ListMemebers()
        {
            var response = await SinchClientMockStudio.Sms.Groups.ListMembers("01GJFY2CEJQ0Y20704G8G506N9");
            response.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateWithChild()
        {
            var response = await SinchClientMockStudio.Sms.Groups.Create(new CreateGroupRequest()
            {
                Name = "WithChildGroups",
                Members = new List<string>()
                {
                    "+48 737532793"
                },
                ChildGroups = new List<string>()
                {
                    "01GJFY2B2M6NXH1GS96QGG0K2K"
                }
            });
            response.Size.Should().Be(1);
        }

        [Fact]
        public async Task List()
        {
            var response = await SinchClientMockStudio.Sms.Groups.List(new SMS.Groups.List.ListGroupsRequest
            {
                Page = 2,
            });
            response.Count.Should().Be(56);
        }
    }
}
