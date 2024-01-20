using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sinch.Faxes
{

    /// <summary>
    /// Fax object, see https://developers.sinch.com/docs/fax for more information
    /// </summary>
    public class Fax
    {
        [ReadOnly(true)]
        public string Id { get; init; }
        /// <summary>
        /// Diection fax was sent, inbound someone sent a fax to your sinch number, outbound you sent a fax to someone
        /// </summary>
        [ReadOnly(true)]
        public Direction Direction { get; init; }
        /// <summary>
        /// e164 formatted phone number where the fax was from
        /// </summary>
        /// 
        [DataType(DataType.PhoneNumber)]
        public string From { get; init; }
        public string To { get; set; }

        [DataType(DataType.Url)]
        public string[] ContentUrl { get; set; }
        [ReadOnly(true)]
        public int NumberOfPages { get; init; }
        [ReadOnly(true)]
        public FaxStatus Status { get; init; }
        [ReadOnly(true)]
        public Money Price { get; init; }
        [ReadOnly(true)]
        public Barcode[] BarCodes { get; init; }
        [ReadOnly(true)]
        public string CreateTime { get; init; }
        [ReadOnly(true)]
        public DateTime CompletedTime { get; init; }
        public string HeaderText { get; set; }
        public bool HeaderPageNumbers { get; set; } = true;
        public string HeaderTimeZone { get; set; }
        public int RetryDelaySeconds { get; set; }
        public int CancelTimeoutMinutes { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        [DataType(DataType.Url)]
        public string CallbackUrl { get; set; }
        /// <summary>
        /// valid values aer multipart/form-data or application/json
        /// </summary>
        public string CallbackContentType { get; set; }
        public ImageConversionMethod ImageConversionMethod { get; set; }
        [ReadOnly(true)]
        public ErrorType ErrorType { get; init; }
        [ReadOnly(true)]
        public int ErrorId { get; init; }
        [ReadOnly(true)]
        public string ErrorCode { get; init; }
        [ReadOnly(true)]
        public string ProjectId { get; init; }
        public string ServiceId { get; set; }
        public int MaxRetries { get; set; }
        [ReadOnly(true)]
        public int RetryCount { get; init; }
        [ReadOnly(true)]
        public string HasFile { get; init; }
    }
}
