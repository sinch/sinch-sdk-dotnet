using System.Collections.Generic;
using Sinch.Core;
using Sinch.SMS.Batches.Send;

namespace Sinch.SMS.Batches.DryRun
{
    public class DryRunRequest
    {
        /// <summary>
        ///     Whether to include per recipient details in the response
        /// </summary>
        public bool? PerRecipient { get; set; }

        /// <summary>
        ///     Max number of recipients to include per recipient details for in the response
        /// </summary>
        public int? NumberOfRecipients { get; set; }

        /// <summary>
        ///     The request to calculate based on.
        /// </summary>
        public ISendBatchRequest BatchRequest { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();

            if (PerRecipient.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("per_recipient", PerRecipient.ToString().ToLowerInvariant()));
            }

            if (NumberOfRecipients.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("number_of_recipients", NumberOfRecipients.ToString()));
            }

            return StringUtils.ToQueryString(kvp);
        }
    }
}
