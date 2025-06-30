using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticPhonicsSkill : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("phonicsskill").Map<ElasticPhonicsSkill>(m => m.AutoMap());

        public int Order { get; set; }

        [Keyword]
        public string Name { get; set; }

        [Keyword]
        public string Description { get; set; }

        [Keyword]
        public string zhDescription { get; set; }

        [Keyword]
        public string zhName { get; set; }
    }
}
