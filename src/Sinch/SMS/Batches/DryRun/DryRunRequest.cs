using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.SMS.Batches.Send;

namespace Sinch.SMS.Batches.DryRun
{
    public class DryRunRequest
    {
        /// <summary>
        ///     Whether to include per recipient details in the response
        /// </summary>
        [JsonIgnore]
        public bool PerRecipient { get; set; }

        /// <summary>
        ///     Max number of recipients to include per recipient details for in the response
        /// </summary>
        [JsonIgnore]
        public int NumberOfRecipients { get; set; } = 100;

        /// <summary>
        ///     The request to calculate based on.
        /// </summary>
        public IBatchRequest BatchRequest { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("per_recipient", PerRecipient.ToString().ToLowerInvariant()));
            kvp.Add(new KeyValuePair<string, string>("number_of_recipients", NumberOfRecipients.ToString()));
            return StringUtils.ToQueryString(kvp);
        }
    }
}
