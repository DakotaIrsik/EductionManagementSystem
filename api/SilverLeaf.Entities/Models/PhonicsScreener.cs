using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class PhonicsScreener : Trackable
    {
        public int Order { get; set; }

        public string ZH_Prefix { get; set; }

        public string Prefix { get; set; }

        public string Test { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int Task { get; set; }
    }
}
