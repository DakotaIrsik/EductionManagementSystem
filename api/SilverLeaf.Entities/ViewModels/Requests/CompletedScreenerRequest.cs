using SilverLeaf.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class CompletedScreenerRequest : PagingRequest
    {
        public CompletedScreenerRequest() { }
        public CompletedScreenerRequest(PagingRequest pagingRequest)
        {
            From = pagingRequest.From;
            Sort = pagingRequest.Sort;
            Fields = pagingRequest.Fields;
            Size = pagingRequest.Size;
        }

        [Range(0, int.MaxValue, ErrorMessage = "Must supply a valid StudentId")]
        public int StudentId { get; set; } = 0;
    }
}
