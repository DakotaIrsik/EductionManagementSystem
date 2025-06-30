using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("HoursOfOperation")]
    public class HoursOfOperation : Trackable
    {
        public string DayOfWeek { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool Open { get; set; }

        public bool IsDefault { get; set; }
    }
}