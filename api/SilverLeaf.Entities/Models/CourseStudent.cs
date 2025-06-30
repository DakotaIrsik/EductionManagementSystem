using SilverLeaf.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Course")]
    public class CourseStudent : Trackable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime IntroducedOn { get; set; }
    }
}
