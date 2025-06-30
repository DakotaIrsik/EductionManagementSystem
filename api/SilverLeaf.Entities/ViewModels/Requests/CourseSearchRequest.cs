using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class CourseSearchRequest : PagingRequest
    {
        public CourseSearchRequest()
        {

        }

        public CourseSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive => true;
    }
}
