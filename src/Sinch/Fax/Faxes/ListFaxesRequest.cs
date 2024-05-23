using System;
using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    public class ListFaxesRequest
    {
        /// <summary>
        ///     Does not specify createTime filter, the default value of the server is used.
        /// </summary>
        public ListFaxesRequest()
        {
        }

        /// <summary>
        ///     Filters faxes created on the specified date in UTC
        /// </summary>
        /// <param name="createTime"></param>
        public ListFaxesRequest(DateOnly createTime)
        {
            CreateTime = createTime;
        }

        /// <summary>
        ///     Filters faxes created in a time range in UTC
        /// </summary>
        /// <param name="createTimeAfter">Faxes created after the date inclusive.</param>
        /// <param name="createTimeBefore">Faxes created before the date inclusive.</param>
        public ListFaxesRequest(DateTime? createTimeAfter, DateTime? createTimeBefore)
        {
            CreateTimeAfter = createTimeAfter;
            CreateTimeBefore = createTimeBefore;
        }

        public DateOnly? CreateTime { get; private set; }

        public DateTime? CreateTimeAfter { get; private set; }


        public DateTime? CreateTimeBefore { get; private set; }

        /// <summary>
        ///     Limits results to faxes with the specified direction.
        /// </summary>
        public Direction? Direction { get; set; }

        /// <summary>
        ///     Limits results to faxes with the specified status.
        /// </summary>
        public FaxStatus? Status { get; set; }

        /// <summary>
        ///     A phone number that you want to use to filter results. The parameter search with startsWith,
        ///     so you can pass a partial number to get all faxes sent to numbers that start with the number you passed.   
        /// </summary>
        /// <example>+14155552222</example>
        public string? To { get; set; }

        /// <summary>
        ///     A phone number that you want to use to filter results. The parameter search with startsWith,
        ///     so you can pass a partial number to get all faxes sent to numbers that start with the number you passed.
        /// </summary>
        /// <example>+15551235656</example>
        public string? From { get; set; }

        /// <summary>
        ///     The page you want to retrieve returned from a previous List request, if any
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        ///     The maximum number of items to return per request. The default is 100 and the maximum is 1000.
        ///     If you need to export larger amounts and pagination is not suitable for you can use
        ///     the Export function in the dashboard.
        /// </summary>
        public int? PageSize { get; set; }

        public string ToQueryString()
        {
            var queryString = new List<KeyValuePair<string, string>>();

            if (CreateTime.HasValue) // mutually exclusive 
            {
                queryString.Add(new("createTime",
                    StringUtils.ToIso8601(CreateTime.Value)));
            }
            else
            {
                if (CreateTimeAfter.HasValue)
                {
                    // result will be inclusive createTime>=
                    queryString.Add(new("createTime>",
                        StringUtils.ToIso8601NoTicks(CreateTimeAfter.Value)));
                }

                if (CreateTimeBefore.HasValue)
                {
                    // result will be inclusive createTime<=
                    queryString.Add(new("createTime<",
                        StringUtils.ToIso8601NoTicks(CreateTimeBefore.Value)));
                }
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
