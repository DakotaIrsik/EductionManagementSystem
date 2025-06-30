using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticOralScreenerResult : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("oralscreenerresult").Map<ElasticOralScreenerResult>(m => m.AutoMap());

        public int Order { get; set; }

        [Keyword]
        public string Question { get; set; }

        [Keyword]
        public string ZH_Question { get; set; }

        [Keyword]
        public string Image { get; set; }

        [Text]
        public string Assessor { get; set; }

        public int StudentId { get; set; }

        public bool IsCorrect { get; set; } = false;
    }
}
