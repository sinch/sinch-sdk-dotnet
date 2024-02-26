using System.Text;

namespace Sinch.Faxes
{
    public class ListOptions
    {
        public  string  CreateTime { get; set; } = null;

        
        public string CreateTimeAfter { get; set; } = null;
        public string CreateTimeBefore { get; set; } = null;
        public Direction? Direction { get; set; } = null;
        public FaxStatus? Status { get; set; } = null;
        public string To { get; set; } = null;
        public string From { get; set; } = null;
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string ToQueryString()
        {
            var queryString = new StringBuilder();

            if (!string.IsNullOrEmpty(CreateTime) )
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
