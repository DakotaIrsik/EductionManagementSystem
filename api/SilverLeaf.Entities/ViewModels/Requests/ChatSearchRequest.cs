using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class ChatSearchRequest : PagingRequest
    {
        public ChatSearchRequest() { }
        public ChatSearchRequest(string userId)
        {
            UserId = userId;
        }

        public ChatSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }
        public string UserId { get; set; }
    }
}
