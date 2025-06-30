using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticStudent : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("student").Map<ElasticStudent>(m => m.AutoMap());

        public int Age { get; set; }

        [Text]
        public string Grade { get; set; }

        [Text]
        public string Email { get; set; }

        [Text]
        public string EnglishName { get; set; }

        [Text]
        public string NativeName { get; set; }

        [Text]
        public string Assessor { get; set; }

        [Text]
        public string PhoneNumber { get; set; }

        [Text]
        public string Address1 { get; set; }

        [Text]
        public string Address2 { get; set; }

        [Text]
        public string City { get; set; }

        [Text]
        public string State { get; set; }

        [Text]
        public string ZipCode { get; set; }

        public bool IsComplete { get; set; }

        [Keyword]
        public string UserId { get; set; }

        public bool IsPhonicsScreenerComplete { get; set; }

        public bool IsBeginnerOralScreenerComplete { get; set; }

        public bool IsComprehensionScreenerComplete { get; set; }

        public bool IsStarReadingTestComplete { get; set; }
    }
}
