using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class RoomSearchRequest : PagingRequest
    {
        public RoomSearchRequest() { }

        public RoomSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public string Name { get; set; }

        public string CenterName { get; set; }

        public string UserId { get; set; }

        public int Id { get; set; }

        public bool IsActive => true;
    }
}
