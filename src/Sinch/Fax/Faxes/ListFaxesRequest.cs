using System;
using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    public class ListFaxesRequest
    {
        /// <summary>
        ///     Does not filter taxes by creation date 
        /// </summary>
        public ListFaxesRequest()
        {
        }
        
        /// <summary>
        ///     Filters faxes created on the specified date in UTC
        /// </summary>
        /// <param name="createTime"></param>
        public ListFaxesRequest(DateTime createTime)
        {
            
            CreateTime = new DateTime(createTime.Ticks, DateTimeKind.Utc);
        }
        
        /// <summary>
        ///     Filters faxes created in a time range in UTC
        /// </summary>
        /// <param name="createTimeAfter"></param>
        /// <param name="createTimeBefore"></param>
        public ListFaxesRequest(DateTime createTimeAfter, DateTime createTimeBefore)
        {
            CreateTimeAfter = createTimeAfter;
            CreateTimeBefore = createTimeBefore;
        }
        
        public DateTime? CreateTime { get; private set; }
        
        public DateTime? CreateTimeAfter { get; private set; }
        
        
        public DateTime? CreateTimeBefore { get; private set; }
        
        public Direction? Direction { get; set; }
        public FaxStatus? Status { get; set; }
        public string? To { get; set; }
        public string? From { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        
        public string ToQueryString()
        {
            var queryString = new List<KeyValuePair<string, string>>();
            
            if (CreateTime.HasValue)
            {
                queryString.Add(new("createTime",
                    StringUtils.ToIso8601NoTicks(CreateTime.Value)));
            }
            
            if (CreateTimeAfter.HasValue && CreateTimeBefore.HasValue)
            {
                queryString.Add(new("createTime>=",
                    StringUtils.ToIso8601(CreateTimeAfter.Value)));
                queryString.Add(new("createTime<=",
                    StringUtils.ToIso8601(CreateTimeBefore.Value)));
            }
            
            if (Direction != null)
            {
                queryString.Add(new("direction", Direction.Value));
            }
            
            if (Status != null)
            {
                queryString.Add(new("status", Status.Value));
            }
            
            if (To != null)
            {
                queryString.Add(new("to", To));
            }
            
            if (From != null)
            {
                queryString.Add(new("from", From));
            }
            
            if (Page.HasValue)
            {
                queryString.Add(new("page", Page.Value.ToString()));
            }
            
            if (PageSize.HasValue)
            {
                queryString.Add(new("pageSize", PageSize.Value.ToString()));
            }
            
            return StringUtils.ToQueryString(queryString);
        }
    }
}
