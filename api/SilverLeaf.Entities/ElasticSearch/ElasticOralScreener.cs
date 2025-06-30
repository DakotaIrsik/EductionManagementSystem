using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticOralScreener : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("oralscreener").Map<ElasticOralScreener>(m => m.AutoMap());

        public int Order { get; set; }

        [Keyword]
        public string Question { get; set; }

        [Keyword]
        public string Image { get; set; }

        [Keyword]
        public string ZH_Question { get; set; }
    }
}
