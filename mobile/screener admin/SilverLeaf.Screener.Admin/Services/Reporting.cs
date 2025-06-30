using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Diagnostics;

namespace SilverLeaf.Screener.Admin.Services
{
    public class Reporting : IReporting
    {
        public virtual void Report(Exception exception)
        {
            Debug.WriteLine(exception.ToString());
        }
    }
}
