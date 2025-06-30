using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class StudentRegistrationRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Must specify a grade: K, or 1-12")]
        public string Grade { get; set; }

        [Required]
        [Range(4, 16, ErrorMessage = "Must be between 4 and 16")]
        public int Age { get; set; }

        public int StarReadingId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be at least two characters.")]
        public string EnglishName { get; set; }

        public string Assessor { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public bool IsActive => true;
    }
}
