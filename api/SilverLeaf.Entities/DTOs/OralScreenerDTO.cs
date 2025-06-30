using SilverLeaf.Entities.DTOs;

namespace SilverLeaf.Entities.Models
{
    public class OralScreenerDTO : BaseDTO
    {
        public int Order { get; set; }

        public string Question { get; set; }

        public string ZH_Question { get; set; }

        public string Image { get; set; }

        public bool? IsCorrect { get; set; }

        public int StudentId { get; set; }

        public string Assessor { get; set; } = "System";
    }
}
