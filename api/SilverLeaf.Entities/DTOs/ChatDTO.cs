namespace SilverLeaf.Entities.DTOs
{
    public class ChatDTO
    {
        public string Message { get; set; }

        public string ToUserId { get; set; }

        public string ToUserName { get; set; }

        public string UserId { get; private set; }
    }
}
