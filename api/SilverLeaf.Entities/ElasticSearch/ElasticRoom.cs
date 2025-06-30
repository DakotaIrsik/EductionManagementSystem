using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticRoom : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("room").Map<ElasticRoom>(m => m.AutoMap());

        [Text]
        public string Name { get; set; }

        public int OccupancyMaximum { get; set; }
    }
}
