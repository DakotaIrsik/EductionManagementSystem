using SilverLeaf.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Chat")]
    public class Chat : Trackable
    {
        public string ToUserId { get; set; }

        public User ToUser { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string Message { get; set; }

        public bool Read { get; set; }
    }
}