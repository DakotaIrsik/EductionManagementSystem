using Nest;
using System;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class BaseElastic
    {
        public int Id { get; set; }

        [Keyword]
        public bool IsActive { get; set; } = true;

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}
