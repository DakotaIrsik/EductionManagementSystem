using SilverLeaf.Entities.DTOs;

namespace SilverLeaf.Entities.Models
{
    public class ComprehensionScreenerDTO : BaseDTO
    {
        public string Preface { get; set; }

        public string SecondPreface { get; set; }

        public int Order { get; set; }

        public string Question { get; set; }

        public string Answers { get; set; }

        public string CorrectAnswer { get; set; }

        public string Image { get; set; }

        public string SecondImage { get; set; }

        public bool? IsCorrect { get; set; }

        public int StudentId { get; set; }

        public string Assessor { get; set; } = "System";
    }
}
