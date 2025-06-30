using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticChat : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("chat").Map<ElasticChat>(m => m.AutoMap());

        [Keyword]
        public string ToUserId { get; set; }

        [Keyword]
        public string UserId { get; set; }

        public bool Read { get; set; }

        [Text]
        public string Message { get; set; }
    }
}
