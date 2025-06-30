using Nest;
using System;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticScreener : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("screener").Map<ElasticScreener>(m => m.AutoMap());

        public int StudentId { get; set; }

        public ElasticStudent Student { get; set; }

        public DateTime GeneratedOn { get; set; }

        [Text]
        public string Assessor { get; set; }

        [Text]
        public string StarReaderId { get; set; } = "N/A";

        [Text]
        public string LastScreenerDate { get; set; } = "N/A";

        [Text]
        public string AreasOfStrength { get; set; }

        [Text]
        public string AreasForImprovement { get; set; }

        [Text]
        public string ExtraInformationGained { get; set; }

        [Text]
        public string Course1 { get; set; }

        [Text]
        public string Course2 { get; set; }

        [Text]
        public string Course3 { get; set; }

        [Text]
        public string Course4 { get; set; }

        [Text]
        public string PrimaryRecommendation { get; set; }

        [Text]
        public string ReasonsForPrimaryRecommendation { get; set; } = "N/A";

        [Text]
        public string SecondaryRecommendation { get; set; }

        [Text]
        public string ReasonsForSecondaryRecommendation { get; set; } = "N/A";

        [Text]
        public string Url { get; set; }
    }
}
