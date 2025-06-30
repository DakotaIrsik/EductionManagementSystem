using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticComprehensionScreener : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("comprehensionscreener").Map<ElasticComprehensionScreener>(m => m.AutoMap());

        public int Order { get; set; }

        [Text]
        public string Preface { get; set; }

        [Text]
        public string SecondPreface { get; set; }

        [Text]
        public string Question { get; set; }

        [Text]
        public string Answers { get; set; }

        [Text]
        public string CorrectAnswer { get; set; }

        [Text]
        public string Image { get; set; }

        [Text]
        public string SecondImage { get; set; }
    }
}
