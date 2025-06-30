using SilverLeaf.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Course")]
    public class Course : Trackable
    {
        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime IntroducedOn { get; set; }
    }
}
