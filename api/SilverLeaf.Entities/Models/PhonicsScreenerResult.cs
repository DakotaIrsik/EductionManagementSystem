using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class PhonicsScreenerResult : Trackable
    {
        public string Assessor { get; set; }

        public string ZH_Prefix { get; set; }

        public string Prefix { get; set; }

        public string Test { get; set; }

        public int CourseId { get; set; }

        public int Task { get; set; }

        public bool? IsCorrect { get; set; }

        public int Order { get; set; }

        public Student Student { get; set; }

        public int StudentId { get; set; }

        public PhonicsScreener PhonicsScreener { get; set; }

        public int PhonicsScreenerId { get; set; }
    }
}
