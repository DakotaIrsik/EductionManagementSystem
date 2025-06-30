using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class OralScreenerResult : Trackable
    {
        public string Assessor { get; set; }

        public int Order { get; set; }

        public string Question { get; set; }

        public string ZH_Question { get; set; }

        public string Image { get; set; }

        public bool? IsCorrect { get; set; }

        public Student Student { get; set; }

        public int StudentId { get; set; }

        public int OralScreenerId { get; set; }

        public OralScreener OralScreener { get; set; }
    }
}
