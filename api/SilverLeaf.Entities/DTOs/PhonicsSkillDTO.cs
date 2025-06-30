namespace SilverLeaf.Entities.DTOs
{
    public class PhonicsSkillDTO : BaseDTO
    {
        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string zhDescription { get; set; }

        public string zhName { get; set; }

        public string Assessor { get; set; } = "System";
    }
}
