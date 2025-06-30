using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class ClassStudentPoint : Trackable
    {
        public int ClassStudentId { get; set; }

        public ClassStudent ClassStudent { get; set; }

        public int PointId { get; set; }

        public Point Point { get; set; }
    }
}
