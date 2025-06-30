using SilverLeaf.Common.Constants;
using SilverLeaf.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Common.Models
{
    public class PagingRequest : IAdjustable
    {
        public int From { get; set; } = 0;

        public string Sort { get; set; } = string.Empty;

        public string Fields { get; set; } = string.Empty;

        [Range(1, Numbers.MaximumPageSize, ErrorMessage = "Size is out of Range")]
        public virtual int Size { get; set; } = Numbers.DefaultPageSize;

        public PagingRequest(int from, string sort, string fields, int size)
        {
            From = from;
            Sort = sort;
            Fields = fields;
            Size = size;
        }
        public PagingRequest(bool all)
        {
            Size = all ? Numbers.MaximumPageSize : Numbers.DefaultPageSize;
        }
        public PagingRequest() { }
    }


}
