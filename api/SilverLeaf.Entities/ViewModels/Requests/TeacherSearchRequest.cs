using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class TeacherSearchRequest : PagingRequest
    {
        public TeacherSearchRequest() { }

        public TeacherSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string About { get; set; }

        public string Amenities { get; set; }

        public string UserId { get; set; }

        public int Id { get; set; }

        public bool IsActive => true;
    }
}
