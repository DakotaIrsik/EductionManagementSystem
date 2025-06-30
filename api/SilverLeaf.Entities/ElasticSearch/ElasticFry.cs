using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticFry : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("fry").Map<ElasticFry>(m => m.AutoMap());


        public int Order { get; set; }

        [Text]
        public string Word { get; set; }

        public int Set { get; set; }
    }
}
