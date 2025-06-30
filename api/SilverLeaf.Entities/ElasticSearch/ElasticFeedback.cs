using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticFeedback : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("feedback").Map<ElasticFeedback>(m => m.AutoMap());

        public string WorkItem { get; set; }

        public string Importance { get; set; }

        public string WillingToPayForFeature { get; set; }

        public string WillingToPayForFeatureAmount { get; set; }

        public string WillingtToPayForSubscription { get; set; }

        public string WillingToPayForSubscriptionAmount { get; set; }

        public string Comments { get; set; }

        [Keyword]
        public string UserId { get; set; }
    }
}
