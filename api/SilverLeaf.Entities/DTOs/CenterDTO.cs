using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class CenterDTO : Locatable
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public string NativeName { get; set; }

        public long PhoneNumber { get; set; }
    }
}