using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticPhonicsScreenerResult : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("phonicsscreenerresult").Map<ElasticPhonicsScreenerResult>(m => m.AutoMap());

        public int Order { get; set; }

        [Text]
        public string Prefix { get; set; }

        [Text]
        public string ZH_Prefix { get; set; }

        [Text]
        public string Test { get; set; }

        public ElasticCourse Course { get; set; }

        public int CourseId { get; set; }

        [Text]
        public string Assessor { get; set; }

        public int Task { get; set; }

        public int StudentId { get; set; }

        public bool IsCorrect { get; set; } = false;
    }
}
