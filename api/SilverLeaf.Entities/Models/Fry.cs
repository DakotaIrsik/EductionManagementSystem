using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Fry")]
    public class Fry : Trackable
    {
        public int Order { get; set; }

        public string Word { get; set; }

        public int Set { get; set; }
    }
}
