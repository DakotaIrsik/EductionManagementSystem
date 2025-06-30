using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class Point : Trackable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string ImageUrl { get; set; }
    }
}
