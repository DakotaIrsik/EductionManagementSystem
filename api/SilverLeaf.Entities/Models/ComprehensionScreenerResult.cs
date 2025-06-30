using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class ComprehensionScreenerResult : Trackable
    {
        public string Assessor { get; set; }

        public string Preface { get; set; }

        public string SecondPreface { get; set; }

        public int Order { get; set; }

        public string Question { get; set; }

        public string Image { get; set; }

        public string SecondImage { get; set; }

        public string Answers { get; set; }

        public string CorrectAnswer { get; set; }

        public bool? IsCorrect { get; set; }

        public Student Student { get; set; }

        public int StudentId { get; set; }

        public int ComprehensionScreenerId { get; set; }

        public ComprehensionScreener ComprehensionScreener { get; set; }
    }
}
