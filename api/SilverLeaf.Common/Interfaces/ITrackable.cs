using System;

namespace SilverLeaf.Common.Interfaces
{
    public interface ITrackable
    {
        int Id { get; }

        DateTime CreateDate { get; }

        DateTime? UpdateDate { get; }

        string CreatedBy { get; }

        string UpdatedBy { get; }
    }
}
