using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("User")]
    public class User : Trackable
    {
        [Key]
        public string IdentityUserId { get; set; }
    }
}