using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class PendingScreenerSearchRequest : PagingRequest
    {
        public PendingScreenerSearchRequest() { }

        public PendingScreenerSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public bool IsActive => true;
    }
}
