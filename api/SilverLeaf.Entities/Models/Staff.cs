using SilverLeaf.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Staff")]
    public class Staff : Locatable
    {
        public string Number { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public Center Center { get; set; }
        public int CenterId { get; set; }

        public string Type { get; set; }

        public long PhoneNumber { get; set; }

        public string Email { get; set; }

        public IList<ClassStaff> ClassStaff { get; set; }
    }
}