using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("ClassStudent")]
    public class ClassStudent : Trackable
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }

        public bool InAttendance { get; set; }
    }
}
