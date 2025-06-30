using SilverLeaf.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class StudentQuestionsRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Must be a valid student Id.")]
        public int StudentId { get; set; }
        public PagingRequest PagingRequest { get; set; }
    }
}
