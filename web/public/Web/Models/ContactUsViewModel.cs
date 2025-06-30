using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SilverLeaf.Public.Web.Common;

namespace SilverLeaf.Public.Web.Models
{
    public class ContactUsViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        [Alphanumeric(ErrorMessage = "You may only enter Alphanumeric characters.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(500)]
        [Alphanumeric(ErrorMessage = "You may only enter Alphanumeric characters.")]
        [DisplayName("What can we help you with?")]
        public string Description { get; set; }
    }
}
