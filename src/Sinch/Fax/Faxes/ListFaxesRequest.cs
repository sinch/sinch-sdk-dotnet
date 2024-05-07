using System.Text;

namespace Sinch.Fax.Faxes
{
    public class ListFaxesRequest
    {
        public string? CreateTime { get; set; }

        public string? CreateTimeAfter { get; set; }
        public string? CreateTimeBefore { get; set; }
        public Direction? Direction { get; set; }
        public FaxStatus? Status { get; set; }
        public string? To { get; set; }
        public string? From { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string ToQueryString()
        {
            var queryString = new StringBuilder();

            if (!string.IsNullOrEmpty(CreateTime))
            {
                queryString.Append($"createTime={CreateTime}&");
            }

            if (!string.IsNullOrEmpty(CreateTimeAfter) && string.IsNullOrEmpty(CreateTime))
            {
                queryString.Append($"createTime>={CreateTimeAfter}&");
            }

            if (CreateTimeBefore != null)
            {
                queryString.Append($"createTime<={CreateTimeBefore}&");
            }

            if (Direction != null)
            {
                queryString.Append($"direction={Direction}&");
            }

            if (Status != null)
            {
                queryString.Append($"status={Status}&");
            }

            if (To != null)
            {
                queryString.Append($"to={To}&");
            }

            if (From != null)
            {
                queryString.Append($"from={From}&");
            }

            if (Page != 0)
            {
                queryString.Append($"page={Page}&");
            }

            if (PageSize != 0)
            {
                queryString.Append($"&pageSize={PageSize}");
            }

            return queryString.ToString();
        }
    }
}
