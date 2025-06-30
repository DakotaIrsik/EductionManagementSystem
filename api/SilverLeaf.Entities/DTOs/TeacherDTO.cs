using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.DTOs
{

    public class TeacherDTO : BaseDTO
    {
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        public string ZipCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string About { get; set; }

        [StringLength(1000)]
        public string Rules { get; set; }

        [StringLength(1000)]
        public string CancellationPolicy { get; set; }

        public IEnumerable<string> Amenities { get; set; }

        public string SkillLevel { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public string WebSiteLink { get; set; }

        public string AudioClipLink { get; set; }

        public string ProfileImage { get; set; }

        public string BannerImage { get; set; }
    }
}