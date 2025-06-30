using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class StudentSearchRequest : PagingRequest
    {
        public StudentSearchRequest() { }

        public StudentSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public bool? IsPhonicsScreenerComplete { get; set; }

        public bool? IsBeginnerOralScreenerComplete { get; set; }

        public bool? IsComprehensionScreenerComplete { get; set; }

        public bool? IsStarReadingTestComplete { get; set; }

        public bool IsActive => true;
    }
}
