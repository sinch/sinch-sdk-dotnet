using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.SMS;
using Sinch.SMS.Batches;
using Sinch.SMS.Batches.Send;
using Xunit;

namespace Sinch.Tests.e2e.Sms
{
    public class BatchesTests : TestBase
    {
        [Fact]
        public async Task SendBatch()
        {
            var response = await SinchClient.Sms.Batches.Send(new Request
            {
                Body = "Asynchronous Spanish Inquisition",
                DeliveryReport = DeliveryReport.Summary,
                FeedbackEnabled = true,
                To = new List<string>() { "+48 737532793" },
                From = "447520650792",
            });
            response.Type.Should().Be(SmsType.MtText);
        }

        [Fact]
        public async Task DryRun()
        {
            var response = await SinchClient.Sms.Batches.DryRun(new SMS.Batches.DryRun.Request
            {
                To = new List<string>() { "+48 737532793" },
                Body = "Spanish Inquisition",
                From = "447520650792",
                PerRecipient = true,
                NumberOfRecipients = 10,
            });
            response.NumberOfMessages.Should().Be(1);
            response.NumberOfRecipients.Should().Be(1);
            response.PerRecipient.First().Encoding.Should().Be("text");
        }

        [Fact]
        public async Task ListBatches()
        {
            var response = await SinchClient.Sms.Batches.List(new SMS.Batches.List.Request
            {
                Page = 0,
            });
            response.PageSize.Should().Be(6);
        }

        [Fact]
        public async Task GetBatch()
        {
            var response = await SinchClient.Sms.Batches.Get("01GJK2J7HXWZVS11GSEQRR19GT");
            response.Canceled.Should().Be(false);
        }

        [Fact]
        public async Task UpdateBatch()
        {
            var response = await SinchClient.Sms.Batches.Update("01GK6ZMBRR3MQA0S2HA3K81EJJ",
                new SMS.Batches.Update.Request()
                {
                    Body = "Update Batch Test After Update"
                });
            response.DeliveryReport.Should().Be(DeliveryReport.None);
        }

        [Fact]
        public async Task ReplaceBatch()
        {
            var response = await SinchClient.Sms.Batches.Replace(new Batch()
            {
                Id = "01GK6Y1B6X0JFJ1DT70PSK1GHV",
                Body = "Replace SMS batch test",
                To = new List<string>()
                {
                    "+48 737532793"
                }
            });
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task CancelBatch()
        {
            var response = await SinchClient.Sms.Batches.Cancel("01GK45R67FXXG60CEGJ57V01BM");
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task DeliveryFeedbackForBatch()
        {
            Func<Task> op = () => SinchClient.Sms.Batches.SendDeliveryFeedback("01GJK2J7HXWZVS11GSEQRR19GT",
                new[] { "+48 737532793" });
            await op.Should().NotThrowAsync();
        }
    }
}
