using SilverLeaf.Common.LookUps;
using System;

namespace SilverLeaf.Entities.DTOs
{
    public class CompletionScreenerDTO
    {
        public StudentDTO Student { get; set; }

        public DateTime GeneratedOn => DateTime.UtcNow.AddHours(8);

        public string Assessor { get; set; }

        public string StarReaderId { get; set; } = "N/A";

        public string LastScreenerDate { get; set; } = "N/A";

        public string AreasOfStrength { get; set; }

        public string AreasForImprovement { get; set; }

        public string ExtraInformationGained { get; set; }

        public PhonicsMetrics Phonics { get; set; } = new PhonicsMetrics();

        public StarReadingDTO StarReading { get; set; } = new StarReadingDTO();

        public string PrimaryRecommendation { get; set; } = "N/A";

        public string ReasonsForPrimaryRecommendation { get; set; } = "N/A";

        public string SecondaryRecommendation { get; set; } = "N/A";

        public string ReasonsForSecondaryRecommendation { get; set; } = "N/A";

        public string Url { get; set; }
    }
}
