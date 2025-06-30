using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class OralScreenerSubmissionRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Must be at least one character.")]
        public string Image { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Must be at least one character.")]
        public string Question { get; set; }

        public bool IsCorrect { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid student Id")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid order sequence")]
        public int Order { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid Id.")]
        public int Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Must be at least one character.")]
        public string Assessor { get; set; }

        public bool IsActive => true;
    }
}
