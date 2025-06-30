using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("CourseClass")]
    public class CourseClass : Trackable
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }

        public string ReplacementBook { get; set; }
    }
}