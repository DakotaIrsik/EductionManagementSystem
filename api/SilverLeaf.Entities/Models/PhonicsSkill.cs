using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("PhonicsSkill")]
    public class PhonicsSkill : Trackable
    {
        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string zhDescription { get; set; }

        public string zhName { get; set; }
    }
}
