using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS.Groups;
using Sinch.SMS.Groups.Create;
using Sinch.SMS.Groups.List;
using Sinch.SMS.Groups.Replace;
using Sinch.SMS.Groups.Update;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class Groups
    {
        private ISinchSmsGroups _groups;
        private Group _group;
        private ListGroupsResponse _listGroupsResponse;
        private List<Group> _listGroups;
        private int _totalPages;
        private Group _updatedGroup;
        private Group _replaceGroup;
        private Func<Task> _deleteOp;
        private IEnumerable<string> _memberList;

        [Given(@"the SMS service ""Groups"" is available")]
        public void GivenTheSmsServiceIsAvailable()
        {
            _groups = Utils.SinchClient.Sms.Groups;
        }


        [When(@"I send a request to create an SMS group")]
        public async Task WhenISendARequestToCreateAnSmsGroup()
        {
            _group = await _groups.Create(new CreateGroupRequest()
            {
                Name = "Group master",
                Members = ["+12017778888", "+12018887777"],
                ChildGroups = ["01W4FFL35P4NC4K35SUBGROUP1"]
            });
        }

        [Then(@"the response contains the SMS group details")]
        public void ThenTheResponseContainsTheSmsGroupDetails()
        {
            _group.Should().BeEquivalentTo(new Group
            {
                Id = "01W4FFL35P4NC4K35SMSGROUP1",
                Name = "Group master",
                Size = 2,
                CreatedAt = Helpers.ParseUtc("2024-06-06T08:59:22.156Z"),
                ModifiedAt = Helpers.ParseUtc("2024-06-06T08:59:22.156Z"),
                ChildGroups = ["01W4FFL35P4NC4K35SUBGROUP1"],
            });
        }

        [When(@"I send a request to retrieve an SMS group")]
        public async Task WhenISendARequestToRetrieveAnSmsGroup()
        {
            _group = await _groups.Get("01W4FFL35P4NC4K35SMSGROUP1");
        }

        [When(@"I send a request to list the existing SMS groups")]
        public async Task WhenISendARequestToListTheExistingSmsGroups()
        {
            _listGroupsResponse = await _groups.List(new ListGroupsRequest()
            {
                PageSize = 2
            });
        }

        [Then(@"the response contains ""(.*)"" SMS groups")]
        public void ThenTheResponseContainsSmsGroups(int count)
        {
            _listGroupsResponse.Groups.Should().HaveCount(count);
        }

        [When(@"I send a request to list all the SMS groups")]
        public async Task WhenISendARequestToListAllTheSmsGroups()
        {
            _listGroups = new List<Group>();
            await foreach (var g in _groups.ListAuto(new ListGroupsRequest()
            {
                PageSize = 2
            }))
            {
                _listGroups.Add(g);
            }
        }

        [Then(@"the SMS groups list contains ""(.*)"" SMS groups")]
        public void ThenTheSmsGroupsListContainsSmsGroups(int count)
        {
            _listGroups.Should().HaveCount(count);
        }

        [When(@"I iterate manually over the SMS groups pages")]
        public async Task WhenIIterateManuallyOverTheSmsGroupsPages()
        {
            // TODO(DEVEXP-992): implement iterator
            _listGroups = new List<Group>();
            _totalPages = 0;
            ListGroupsResponse listResponse = null;
            while (true)
            {
                listResponse = await _groups.List(new ListGroupsRequest()
                {
                    PageSize = 2,
                    Page = listResponse?.Page + 1
                });
                if (!listResponse.Groups.Any()) break;
                _listGroups.AddRange(listResponse.Groups);
                _totalPages++;
            }
        }

        [Then(@"the SMS groups iteration result contains the data from ""(.*)"" pages")]
        public void ThenTheSmsGroupsIterationResultContainsTheDataFromPages(int count)
        {
            _totalPages.Should().Be(count);
        }

        [When(@"I send a request to update an SMS group")]
        public async Task WhenISendARequestToUpdateAnSmsGroup()
        {
            _updatedGroup = await _groups.Update(new UpdateGroupRequest
            {
                GroupId = "01W4FFL35P4NC4K35SMSGROUP1",
                Name = "Updated group name",
                Add = new List<string>
                {
                    "+12017771111",
                    "+12017772222"
                },
                Remove = new List<string>
                {
                    "+12017773333",
                    "+12017774444"
                },
                AddFromGroup = "01W4FFL35P4NC4K35SMSGROUP2",
                RemoveFromGroup = "01W4FFL35P4NC4K35SMSGROUP3"
            });
        }

        [Then(@"the response contains the updated SMS group details")]
        public void ThenTheResponseContainsTheUpdatedSmsGroupDetails()
        {
            _updatedGroup.Should().BeEquivalentTo(new Group
            {
                Id = "01W4FFL35P4NC4K35SMSGROUP1",
                Name = "Updated group name",
                Size = 6,
                CreatedAt = Helpers.ParseUtc("2024-06-06T08:59:22.156Z"),
                ModifiedAt = Helpers.ParseUtc("2024-06-06T09:19:58.147Z"),
                ChildGroups = ["01W4FFL35P4NC4K35SUBGROUP1"]
            });
        }

        [When(@"I send a request to update an SMS group to remove its name")]
        public async Task WhenISendARequestToUpdateAnSmsGroupToRemoveItsName()
        {
            _updatedGroup = await _groups.Update(new UpdateGroupRequest()
            {
                GroupId = "1W4FFL35P4NC4K35SMSGROUP2",
                Name = null
            });
        }

        [Then(@"the response contains the updated SMS group details where the name has been removed")]
        public void ThenTheResponseContainsTheUpdatedSmsGroupDetailsWhereTheNameHasBeenRemoved()
        {
            _updatedGroup.Id.Should().Be("01W4FFL35P4NC4K35SMSGROUP2");
            _updatedGroup.Name.Should().BeNull();
        }

        [When(@"I send a request to replace an SMS group")]
        public async Task WhenISendARequestToReplaceAnSmsGroup()
        {
            _replaceGroup = await _groups.Replace(new ReplaceGroupRequest()
            {
                GroupId = "01W4FFL35P4NC4K35SMSGROUP1",
                Name = "Replacement group",
                Members = ["+12018881111", "+12018882222", "+12018883333"]
            });
        }

        [Then(@"the response contains the replaced SMS group details")]
        public void ThenTheResponseContainsTheReplacedSmsGroupDetails()
        {
            _replaceGroup.Should().BeEquivalentTo(new Group
            {
                Id = "01W4FFL35P4NC4K35SMSGROUP1",
                Name = "Replacement group",
                Size = 3,
                CreatedAt = Helpers.ParseUtc("2024-06-06T08:59:22.156Z"),
                ModifiedAt = Helpers.ParseUtc("2024-08-21T09:39:36.679Z"),
                ChildGroups = new HashSet<string> { "01W4FFL35P4NC4K35SUBGROUP1" }
            });
        }

        [When(@"I send a request to delete an SMS group")]
        public void WhenISendARequestToDeleteAnSmsGroup()
        {
            _deleteOp = () => _groups.Delete("01W4FFL35P4NC4K35SMSGROUP1");
        }

        [Then(@"the delete SMS group response contains no data")]
        public async Task ThenTheDeleteSmsGroupResponseContainsNoData()
        {
            await _deleteOp.Should().NotThrowAsync();
        }

        [When(@"I send a request to list the members of an SMS group")]
        public async Task WhenISendARequestToListTheMembersOfAnSmsGroup()
        {
            _memberList = await _groups.ListMembers("01W4FFL35P4NC4K35SMSGROUP1");
        }

        [Then(@"the response contains the phone numbers of the SMS group")]
        public void ThenTheResponseContainsThePhoneNumbersOfTheSmsGroup()
        {
            _memberList.Should().ContainInOrder(new List<string>()
            {
                "12018881111",
                "12018882222",
                "12018883333"
            });
        }
    }
}
