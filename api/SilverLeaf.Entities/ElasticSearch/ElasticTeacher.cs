using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticTeacher : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("teacher").Map<ElasticTeacher>(m => m.AutoMap());

        [Text]
        public string Email { get; set; }

        [Text]
        public string FirstName { get; set; }

        [Text]
        public string LastName { get; set; }

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

        [Keyword]
        public string UserId { get; set; }
    }
}
