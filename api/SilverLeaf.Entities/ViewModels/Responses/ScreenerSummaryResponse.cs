using SilverLeaf.Common.LookUps;
using SilverLeaf.Entities.DTOs;
using System;
using System.Collections.Generic;

namespace SilverLeaf.Entities.ViewModels.Requests.Responses
{
    public class ScreenerSummaryResponse
    {
        public StudentDTO Student { get; set; }

        public List<PhonicsMetrics> PhonicsMetrics { get; set; } = new List<PhonicsMetrics>();

        public DateTime GeneratedOn { get; set; }

        public string Assessor { get; set; }

        public string StarReaderId { get; set; } = "N/A";

        public string LastScreenerDate { get; set; } = "N/A";

        public string AreasOfStrength { get; set; }

        public string AreasForImprovement { get; set; }

        public string ExtraInformationGained { get; set; }

        public string PrimaryRecommendation { get; set; }

        public string ReasonsForPrimaryRecommendation { get; set; } = "N/A";

        public string SecondaryRecommendation { get; set; }

        public string ReasonsForSecondaryRecommendation { get; set; } = "N/A";

        public string Url { get; set; }
    }
}
