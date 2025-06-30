using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class ClassSearchRequest : PagingRequest
    {
        public ClassSearchRequest()
        {

        }

        public ClassSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public int CourseId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Lesson { get; set; }

        public int Week { get; set; }

        public string Session { get; set; }

        public string Card { get; set; }

        public string Fictionality { get; set; }

        public string Genre { get; set; }

        public string Title { get; set; }

        public bool IsActive => true;
    }
}
