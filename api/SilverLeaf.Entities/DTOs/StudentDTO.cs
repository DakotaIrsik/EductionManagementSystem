using SilverLeaf.Entities.Models;
using System.Collections.Generic;

namespace SilverLeaf.Entities.DTOs
{

    public class StudentDTO : BaseDTO
    {
        public string Grade { get; set; }

        public int Age { get; set; }

        public int StarReadingId { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string Assessor { get; set; }

        public bool IsStarReadingTestComplete { get; set; }

        public bool IsPhonicsScreenerComplete { get; set; }

        public bool IsBeginnerOralScreenerComplete { get; set; }

        public bool IsComprehensionScreenerComplete { get; set; }

        public int PhonicsScreenerCompletionPercentage { get; set; }

        public int OralScreenerCompletionPercentage { get; set; }

        public int ComprehensionScreenerCompletionPercentage { get; set; }

        public List<PhonicsScreenerDTO> PhonicsScreenerResults { get; set; }

        public List<ComprehensionScreenerDTO> ComprehensionScreenerResults { get; set; }

        public List<OralScreenerDTO> OralScreenerResults { get; set; }
    }
}
