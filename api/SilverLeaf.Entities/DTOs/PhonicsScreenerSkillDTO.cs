using SilverLeaf.Entities.Models;

namespace SilverLeaf.Entities.DTOs
{
    public class PhonicsScreenerSkillDTO
    {
        public int PhonicsScreenerId { get; set; }
        public PhonicsScreener PhonicsScreener { get; set; }

        public int PhonicsSkillId { get; set; }

        public PhonicsSkill PhonicsSkill { get; set; }
    }
}
