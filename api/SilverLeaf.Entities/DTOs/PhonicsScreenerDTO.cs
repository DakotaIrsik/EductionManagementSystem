using SilverLeaf.Entities.DTOs;

namespace SilverLeaf.Entities.Models
{
    public class PhonicsScreenerDTO : BaseDTO
    {
        public int Order { get; set; }

        public string Prefix { get; set; }

        public string Test { get; set; }

        public CourseDTO Course { get; set; }

        public int CourseId { get; set; }

        public int Task { get; set; }

        public bool? IsCorrect { get; set; }

        public int StudentId { get; set; }

        public int PhonicsScreenerId { get; set; }

        public string ZH_Prefix { get; set; }

        public string Assessor { get; set; } = "System";
    }
}
