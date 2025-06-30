using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class ClassStudentFeedback : Trackable
    {
        public int ClassStudentId { get; set; }

        public ClassStudent ClassStudent { get; set; }

        public int PointId { get; set; }

        public StudentFeedback Feedback { get; set; }
    }
}
