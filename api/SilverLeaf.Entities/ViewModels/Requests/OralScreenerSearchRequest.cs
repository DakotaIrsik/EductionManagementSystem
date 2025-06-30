using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class OralScreenerSearchRequest : PagingRequest
    {
        public OralScreenerSearchRequest() { }

        public OralScreenerSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public int? Order { get; set; }

        public string Question { get; set; }

        public string Image { get; set; }

        public bool Incomplete { get; set; }

        public int StudentId { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive => true;
    }
}
