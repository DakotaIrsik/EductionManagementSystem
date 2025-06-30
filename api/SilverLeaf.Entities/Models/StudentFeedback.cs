using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class StudentFeedback : Trackable
    {
        public int ClassStudentId { get; set; }

        public ClassStudent ClassStudent { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public string ImageUrl { get; set; }
    }
}
