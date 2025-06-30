using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class PhonicsScreenerSearchRequest : PagingRequest
    {
        public PhonicsScreenerSearchRequest() { }

        public PhonicsScreenerSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public int? Order { get; set; }

        public string Prefix { get; set; }

        public string Test { get; set; }

        public bool Incomplete { get; set; }

        public int CourseId { get; set; }

        public int Task { get; set; }

        public int StudentId { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive => true;
    }
}
