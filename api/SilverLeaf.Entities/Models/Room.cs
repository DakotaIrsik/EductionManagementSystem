using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Room")]
    public class Room : Trackable
    {
        [StringLength(1000)]
        public string Name { get; set; }

        public int OccupancyMaximum { get; set; }

        public int CenterId { get; set; }
    }
}