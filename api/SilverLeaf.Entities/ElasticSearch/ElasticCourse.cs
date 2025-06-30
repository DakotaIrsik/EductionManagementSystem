using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticCourse : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("course").Map<ElasticCourse>(m => m.AutoMap());

        [Text]
        public string Name { get; set; }

        [Text]
        public string Description { get; set; }

    }
}
