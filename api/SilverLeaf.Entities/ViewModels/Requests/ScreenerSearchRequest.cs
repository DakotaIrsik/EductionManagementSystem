using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class ScreenerSearchRequest : PagingRequest
    {
        public ScreenerSearchRequest() { }

        public ScreenerSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public int? StudentId { get; set; }

        public bool IsActive => true;
    }
}
