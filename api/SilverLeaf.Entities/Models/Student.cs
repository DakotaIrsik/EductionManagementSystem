using SilverLeaf.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Student")]
    public class Student : Locatable
    {
        public string Grade { get; set; }

        public int Age { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string Assessor { get; set; }

        public bool IsStarReadingTestComplete { get; set; }

        public bool IsPhonicsScreenerComplete { get; set; }

        public bool IsBeginnerOralScreenerComplete { get; set; }

        public bool IsComprehensionScreenerComplete { get; set; }

        public List<PhonicsScreenerResult> PhonicsScreenerResults { get; set; }

        public List<OralScreenerResult> OralScreenerResults { get; set; }

        public List<ComprehensionScreenerResult> ComprehensionScreenerResults { get; set; }
    }
}