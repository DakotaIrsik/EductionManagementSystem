
namespace SilverLeaf.Entities.DTOs
{
    public class FeedbackDTO : BaseDTO
    {
        public string WorkItem { get; set; }
        public string Importance { get; set; }
        public string WillingToPayForFeature { get; set; }
        public string WillingToPayForFeatureAmount { get; set; }
        public string WillingToPayForSubscription { get; set; }
        public string WillingToPayForSubscriptionAmount { get; set; }
        public string Comments { get; set; }
    }
}