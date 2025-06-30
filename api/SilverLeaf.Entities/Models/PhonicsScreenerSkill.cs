using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("PhonicsScreenerSkill")]
    public class PhonicsScreenerSkill : Trackable
    {
        public int PhonicsScreenerId { get; set; }
        public PhonicsScreener PhonicsScreener { get; set; }

        public int PhonicsSkillId { get; set; }

        public PhonicsSkill PhonicsSkill { get; set; }
    }
}
