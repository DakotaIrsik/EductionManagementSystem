using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Feedback")]
    public class Feedback : Trackable
    {
        public string WorkItem { get; set; }
        public string Importance { get; set; }
        public string WillingToPayForFeature { get; set; }
        public string WillingToPayForFeatureAmount { get; set; }
        public string WillingToPayForSubscription { get; set; }
        public string WillingToPayForSubscriptionAmount { get; set; }
        public string Comments { get; set; }
        public string UserId { get; set; }
    }
}