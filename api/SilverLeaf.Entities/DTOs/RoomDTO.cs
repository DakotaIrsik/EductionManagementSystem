using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Entities.DTOs
{
    public class RoomDTO : BaseDTO
    {
        [StringLength(1000)]
        public string Terms { get; set; }

        public int OccupancyMaximum { get; set; }

        public decimal PricePerHour { get; set; }

        public int RewardId { get; set; }

        public string Name { get; set; }
    }
}