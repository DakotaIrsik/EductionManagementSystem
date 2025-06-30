using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class OralScreener : Trackable
    {
        public int Order { get; set; }

        public string Question { get; set; }

        public string Image { get; set; }

        public string ZH_Question { get; set; }
    }
}
