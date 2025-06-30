using Microsoft.AspNetCore.Mvc;
using SilverLeaf.Common.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SilverLeaf.Common.Models
{
    public class Adjustable : IAdjustable
    {
        public int Size { get; set; }

        public int From { get; set; }

        [ReadOnly(true)]
        [HiddenInput]
        public long Total { get; set; }

        [StringLength(200)]
        public string Sort { get; set; }

        [StringLength(200)]
        public string Fields { get; set; }

    }

    public class AdjustableDTO<T> : Adjustable
    {
        public AdjustableDTO()
        {
            Data = new List<T>();
        }

        public AdjustableDTO(IAdjustable paging)
        {
            From = paging.From;
            Size = paging.Size;
            Sort = paging.Sort;
            Fields = paging.Fields;
        }

        public AdjustableDTO(IAdjustable paging, IEnumerable<T> list)
        {
            From = paging.From;
            Size = paging.Size;
            Sort = paging.Sort;
            Fields = paging.Fields;
            Data = list;
        }

        public AdjustableDTO(IAdjustable paging, IEnumerable<T> list, long total)
        {
            From = paging.From;
            Size = paging.Size;
            Sort = paging.Sort;
            Fields = paging.Fields;
            Data = list;
            Total = total;
        }

        public IEnumerable<T> Data { get; set; }
    }
}
