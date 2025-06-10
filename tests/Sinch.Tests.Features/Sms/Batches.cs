using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.SMS;
using Sinch.SMS.Batches;
using Sinch.SMS.Batches.DryRun;
using Sinch.SMS.Batches.List;
using Sinch.SMS.Batches.Send;
using Sinch.SMS.Batches.Update;

namespace Sinch.Tests.Features.Sms
{
    [Binding]
    public class Batches
    {

        private ISinchSmsBatches _sinchSmsBatches;
        private IBatch _sendBatchResponse;
        private DryRunResponse _dryRunResponse;
        private ListBatchesResponse _listBatchesResponse;
        private List<IBatch> _batchList;
        private IBatch _batch;
        private int _totalPages;
        private Func<Task> _deliveryFeedbackOp;

        [Given(@"the SMS service ""Batches"" is available")]
        public void GivenTheSmsServiceIsAvailable(string batches)
        {
            _sinchSmsBatches = Utils.SinchClient.Sms.Batches;
        }

        [When(@"I send a request to send a text message")]
        public async Task WhenISendARequestToSendATextMessage()
        {
            _sendBatchResponse = await _sinchSmsBatches.Send(new SendTextBatchRequest()
            {
                Body = "SMS body message",
                To = ["+12017777777"],
                From = "+12015555555",
                SendAt = Helpers.ParseUtc("2024-06-06T09:25:00Z"),
                DeliveryReport = DeliveryReport.Full,
                FeedbackEnabled = true
            });
        }

        [Then(@"the response contains the text SMS details")]
        public void ThenTheResponseContainsTheTextSmsDetails()
        {
            _sendBatchResponse.As<TextBatch>().Should().BeEquivalentTo(new TextBatch
            {
                To = ["12017777777"],
                From = "12015555555",
                DeliveryReport = DeliveryReport.Full,
                SendAt = Helpers.ParseUtc("2024-06-06T09:25:00Z"),
                ExpireAt = Helpers.ParseUtc("2024-06-09T09:25:00Z"),
                FeedbackEnabled = true,
                Id = "01W4FFL35P4NC4K35SMSBATCH1",
                Canceled = false,
                Body = "SMS body message",
                FlashMessage = false,
                //TODO: DEVEXP-982 add created_at, modified_at fields 
            });
        }

        [When(@"I send a request to send a text message with multiple parameters")]
        public async Task WhenISendARequestToSendATextMessageWithMultipleParameters()
        {
            _sendBatchResponse = await _sinchSmsBatches.Send(new SendTextBatchRequest()
            {
                Body = "Hello ${name}! Get 20% off with this discount code ${code}",
                To = ["+12017777777", "+12018888888"],
                From = "+12015555555",
                Parameters = new()
                {
                    ["name"] = new()
                    {
                        ["+12017777777"] = "John",
                        ["+12018888888"] = "Paul",
                        ["default"] = "there"
                    },
                    ["code"] = new()
                    {
                        ["+12017777777"] = "HALLOWEEN20 \ud83c\udf83",
                    }
                },
                DeliveryReport = DeliveryReport.Full,
            });
        }

        [Then(@"the response contains the text SMS details with multiple parameters")]
        public void ThenTheResponseContainsTheTextSmsDetailsWithMultipleParameters()
        {
            _sendBatchResponse.As<TextBatch>().Should().BeEquivalentTo(new TextBatch
            {
                To = ["12017777777", "12018888888"],
                From = "12015555555",
                DeliveryReport = DeliveryReport.Full,
                ExpireAt = Helpers.ParseUtc("2024-06-06T09:22:14.304Z"),
                Id = "01W4FFL35P4NC4K35SMSBATCH2",
                Canceled = false,
                Body = "Hello ${name}! Get 20% off with this discount code ${code}",
                Parameters = new()
                {
                    ["name"] = new()
                    {
                        ["+12017777777"] = "John",
                        ["+12018888888"] = "Paul",
                        ["default"] = "there"
                    },
                    ["code"] = new()
                    {
                        ["+12017777777"] = "HALLOWEEN20 \ud83c\udf83",
                    }
                },
                FlashMessage = false,
                //TODO: DEVEXP-982 add created_at, modified_at fields 
            });
        }

        [When(@"I send a request to perform a dry run of a batch")]
        public async Task WhenISendARequestToPerformADryRunOfABatch()
        {
            _dryRunResponse = await _sinchSmsBatches.DryRun(new DryRunRequest()
            {
                BatchRequest = new SendTextBatchRequest()
                {
                    Body = "Hello ${name}!",
                    To = ["+12017777777", "+12018888888", "+12019999999"],
                    From = "+12015555555",
                    Parameters = new()
                    {
                        ["name"] = new()
                        {
                            ["+12017777777"] = "John",
                            ["+12018888888"] = "Paul",
                            ["default"] = "there"
                        },
                        ["code"] = new()
                        {
                            ["+12017777777"] = "HALLOWEEN20 \ud83c\udf83",
                        }
                    },
                    DeliveryReport = DeliveryReport.None,
                }
            });
        }

        [Then(@"the response contains the calculated bodies and number of parts for all messages in the batch")]
        public void ThenTheResponseContainsTheCalculatedBodiesAndNumberOfPartsForAllMessagesInTheBatch()
        {
            _dryRunResponse.Should().BeEquivalentTo(new DryRunResponse
            {
                NumberOfRecipients = 3,
                NumberOfMessages = 3,
                PerRecipient = new List<PerRecipient>()
                {
                    new PerRecipient()
                    {
                        Body = "Hello John!",
                        Encoding = "text",
                        NumberOfParts = 1,
                        Recipient = "12017777777"
                    },
                    new PerRecipient
                    {
                        Body = "Hello there!",
                        Encoding = "text",
                        NumberOfParts = 1,
                        Recipient = "12019999999"
                    },
                    new PerRecipient
                    {
                        Body = "Hello there!",
                        Encoding = "text",
                        NumberOfParts = 1,
                        Recipient = "12018888888"
                    }
                }
            });
        }

        [When(@"I send a request to list the SMS batches")]
        public async Task WhenISendARequestToListTheSmsBatches()
        {
            _listBatchesResponse = await _sinchSmsBatches.List(new ListBatchesRequest()
            {
                PageSize = 2
            });
        }

        [Then(@"the response contains ""(.*)"" SMS batches")]
        public void ThenTheResponseContainsSmsBatches(int count)
        {
            _listBatchesResponse.Batches.Should().HaveCount(count);
        }

        [When(@"I send a request to list all the SMS batches")]
        public async Task WhenISendARequestToListAllTheSmsBatches()
        {
            _batchList = new List<IBatch>();
            await foreach (var batch in _sinchSmsBatches.ListAuto(new ListBatchesRequest()
            {
                PageSize = 2
            }))
            {
                _batchList.Add(batch);
            }
        }

        [Then(@"the SMS batches list contains ""(.*)"" SMS batches")]
        public void ThenTheSmsBatchesListContainsSmsBatches(int count)
        {
            _batchList.Should().HaveCount(count);
        }

        [When(@"I iterate manually over the SMS batches pages")]
        public async Task WhenIIterateManuallyOverTheSmsBatchesPages()
        {
            // TODO(DEVEXP-992): implemented iterator 
            _batchList = new List<IBatch>();
            _totalPages = 0;
            ListBatchesResponse listBatchesResponse = null;
            while (true)
            {
                listBatchesResponse = await _sinchSmsBatches.List(new ListBatchesRequest()
                {
                    PageSize = 2,
                    Page = listBatchesResponse?.Page + 1
                });
                if (listBatchesResponse?.Batches?.Count == 0) break;
                _batchList.AddRange(listBatchesResponse.Batches!);
                _totalPages++;
            }
        }

        [Then(@"the SMS batches iteration result contains the data from ""(.*)"" pages")]
        public void ThenTheSmsBatchesIterationResultContainsTheDataFromPages(int count)
        {
            _totalPages.Should().Be(count);
        }

        [When(@"I send a request to retrieve an SMS batch")]
        public async Task WhenISendARequestToRetrieveAnSmsBatch()
        {
            _batch = await _sinchSmsBatches.Get("01W4FFL35P4NC4K35SMSBATCH1");
        }

        [Then(@"the response contains the SMS batch details")]
        public void ThenTheResponseContainsTheSmsBatchDetails()
        {
            _batch.As<TextBatch>().Should().BeEquivalentTo(new TextBatch
            {
                Id = "01W4FFL35P4NC4K35SMSBATCH1",
                Canceled = false,
                Body = "SMS body message",
                FlashMessage = false,
                To = new List<string> { "12017777777" },
                From = "12015555555",
                // TODO: DEVEXP-982 add created_at and modified_at
                DeliveryReport = DeliveryReport.Full,
                SendAt = Helpers.ParseUtc("2024-06-06T09:25:00Z"),
                ExpireAt = Helpers.ParseUtc("2024-06-09T09:25:00Z"),
                FeedbackEnabled = true
            });
        }

        [When(@"I send a request to update an SMS batch")]
        public async Task WhenISendARequestToUpdateAnSmsBatch()
        {
            _batch = await _sinchSmsBatches.Update("01W4FFL35P4NC4K35SMSBATCH1", new UpdateTextBatchRequest()
            {
                From = "+12016666666",
                ToAdd = ["01W4FFL35P4NC4K35SMSGROUP1"],
                DeliveryReport = DeliveryReport.Summary
            });
        }

        [Then(@"the response contains the SMS batch details with updated data")]
        public void ThenTheResponseContainsTheSmsBatchDetailsWithUpdatedData()
        {
            _batch.As<TextBatch>().Should().BeEquivalentTo(
                new TextBatch
                {
                    Id = "01W4FFL35P4NC4K35SMSBATCH1",
                    Canceled = false,
                    Body = "SMS body message",
                    FlashMessage = false,
                    To = new List<string> { "12017777777", "01W4FFL35P4NC4K35SMSGROUP1" },
                    From = "12016666666",
                    // TODO: DEVEXP-982 add created_at and modified_at
                    DeliveryReport = DeliveryReport.Summary,
                    SendAt = Helpers.ParseUtc("2024-06-06T09:25:00Z"),
                    ExpireAt = Helpers.ParseUtc("2024-06-09T09:25:00Z"),
                    FeedbackEnabled = true
                }
            );
        }

        [When(@"I send a request to replace an SMS batch")]
        public async Task WhenISendARequestToReplaceAnSmsBatch()
        {
            _batch = await _sinchSmsBatches.Replace("01W4FFL35P4NC4K35SMSBATCH1", new SendTextBatchRequest()
            {
                From = "+12016666666",
                To = ["+12018888888"],
                Body = "This is the replacement batch",
                SendAt = Helpers.ParseUtc("2024-06-06T09:35:00Z")
            });
        }

        [Then(@"the response contains the new SMS batch details with the provided data for replacement")]
        public void ThenTheResponseContainsTheNewSmsBatchDetailsWithTheProvidedDataForReplacement()
        {
            _batch.As<TextBatch>().Should().BeEquivalentTo(new TextBatch
            {
                Id = "01W4FFL35P4NC4K35SMSBATCH1",
                Canceled = false,
                Body = "This is the replacement batch",
                FlashMessage = false,
                To = ["12018888888"],
                From = "12016666666",
                // TODO: DEVEXP-982 add created_at and modified_at
                DeliveryReport = DeliveryReport.None,
                SendAt = Helpers.ParseUtc("2024-06-06T09:35:00Z"),
                ExpireAt = Helpers.ParseUtc("2024-06-09T09:35:00Z"),
                FeedbackEnabled = null
            });
        }

        [When(@"I send a request to cancel an SMS batch")]
        public async Task WhenISendARequestToCancelAnSmsBatch()
        {
            _batch = await _sinchSmsBatches.Cancel("01W4FFL35P4NC4K35SMSBATCH1");
        }

        [Then(@"the response contains the SMS batch details with a cancelled status")]
        public void ThenTheResponseContainsTheSmsBatchDetailsWithACancelledStatus()
        {
            var batch = _batch.As<TextBatch>();
            batch.Id.Should().Be("01W4FFL35P4NC4K35SMSBATCH1");
            batch.Canceled.Should().BeTrue();
        }

        [When(@"I send a request to send delivery feedbacks")]
        public void WhenISendARequestToSendDeliveryFeedbacks()
        {
            _deliveryFeedbackOp = () => _sinchSmsBatches.SendDeliveryFeedback("01W4FFL35P4NC4K35SMSBATCH1",
                ["+12017777777"]);
        }

        [Then(@"the delivery feedback response contains no data")]
        public async Task ThenTheDeliveryFeedbackResponseContainsNoData()
        {
            await _deliveryFeedbackOp.Should().NotThrowAsync();
        }
    }
}
