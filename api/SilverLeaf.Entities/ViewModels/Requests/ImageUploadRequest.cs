namespace SilverLeaf.Entities.ViewModels.Requests
{
    public class ImageUploadRequest
    {
        public string RewardId { get; set; }

        public string SubType { get; set; }

        public string FileType { get; set; }

        public bool IsDefault { get; set; }
    }
}
