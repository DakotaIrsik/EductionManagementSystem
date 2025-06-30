using SilverLeaf.Common.Models;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class ComprehensionScreenerSearchRequest : PagingRequest
    {
        public ComprehensionScreenerSearchRequest() { }

        public ComprehensionScreenerSearchRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        public string Preface { get; set; }

        public string SecondPreface { get; set; }

        public int? Order { get; set; }

        public string Question { get; set; }

        public string Answers { get; set; }

        public string CorrectAnswer { get; set; }

        public string Image { get; set; }

        public string SecondImage { get; set; }

        public int StudentId { get; set; }

        public string UpdatedBy { get; set; }

        public bool Incomplete { get; set; }

        public bool IsActive => true;
    }
}
