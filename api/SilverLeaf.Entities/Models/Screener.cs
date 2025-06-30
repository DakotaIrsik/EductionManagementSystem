using SilverLeaf.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Screener")]
    public class Screener : Trackable
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public DateTime GeneratedOn => DateTime.UtcNow.AddHours(8);

        public string Assessor { get; set; }

        public string StarReaderId { get; set; } = "N/A";

        public string LastScreenerDate { get; set; } = "N/A";

        public string AreasOfStrength { get; set; }

        public string AreasForImprovement { get; set; }

        public string ExtraInformationGained { get; set; }

        public string Course1 { get; set; }

        public string Course2 { get; set; }

        public string Course3 { get; set; }

        public string Course4 { get; set; }

        public string PrimaryRecommendation { get; set; }

        public string ReasonsForPrimaryRecommendation { get; set; } = "N/A";

        public string SecondaryRecommendation { get; set; }

        public string ReasonsForSecondaryRecommendation { get; set; } = "N/A";

        public string Url { get; set; }
    }
}
