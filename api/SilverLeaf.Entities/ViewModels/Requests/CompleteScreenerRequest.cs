using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class CompleteScreenerRequest
    {
        public string AreasOfStrength { get; set; }

        public string AreasForImprovement { get; set; }

        public string ExtraInformationGained { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must supply a valid StudentId")]
        public int StudentId { get; set; }
    }
}
