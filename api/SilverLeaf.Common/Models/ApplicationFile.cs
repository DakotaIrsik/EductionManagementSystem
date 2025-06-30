namespace SilverLeaf.Common.Models
{
    public partial class ApplicationFile
    {
        public ApplicationFile(string fileType, string subType)
        {
            FileType = fileType;
            SubType = subType;
        }

        public string Url { get; set; }

        public string FileType { get; set; }

        public string SubType { get; set; }
    }
}
