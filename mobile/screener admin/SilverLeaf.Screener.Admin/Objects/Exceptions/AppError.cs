using System;
using System.Collections.Generic;

namespace SilverLeaf.Screener.Admin.Objects.Exceptions
{
    public class AppError : Exception
    {
        public AppError(params string[] errors)
            : this((IEnumerable<string>)errors)
        {
        }

        public AppError(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }
    }
}
