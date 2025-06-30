using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("ClassStaff")]
    public class ClassStaff : Trackable
    {
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}