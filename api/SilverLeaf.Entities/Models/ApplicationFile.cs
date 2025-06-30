using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Image")]
    public partial class ApplicationFile : Trackable
    {
        public string Url { get; set; }
        public string FileType { get; set; }

        public string SubType { get; set; }

        public int? RoomId { get; set; }

        public bool IsDefault { get; set; }

        public int RewardId { get; set; }
    }
}