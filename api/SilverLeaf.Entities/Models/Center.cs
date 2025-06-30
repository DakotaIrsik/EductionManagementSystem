using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Center")]
    public class Center : Locatable
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public string NativeName { get; set; }

        public long PhoneNumber { get; set; }
    }
}