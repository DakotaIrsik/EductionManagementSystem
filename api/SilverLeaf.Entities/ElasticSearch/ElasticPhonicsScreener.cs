using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticPhonicsScreener : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("phonicsscreener").Map<ElasticPhonicsScreener>(m => m.AutoMap());

        public int Order { get; set; }

        [Text]
        public string Prefix { get; set; }

        [Text]
        public string ZH_Prefix { get; set; }

        [Text]
        public string Test { get; set; }

        [Text]
        public string Course { get; set; }

        public int Task { get; set; }
    }
}
