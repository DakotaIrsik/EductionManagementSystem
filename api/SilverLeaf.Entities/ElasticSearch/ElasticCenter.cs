using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticCenter : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("center").Map<ElasticCenter>(m => m.AutoMap());
        [Text]
        public string Number { get; set; }

        [Text]
        public string Name { get; set; }

        [Text]
        public string NativeName { get; set; }

        [Text]
        public long PhoneNumber { get; set; }
    }
}
