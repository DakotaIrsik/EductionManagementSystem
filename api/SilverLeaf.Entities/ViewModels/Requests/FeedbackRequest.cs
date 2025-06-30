namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class FeedbackRequest
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
