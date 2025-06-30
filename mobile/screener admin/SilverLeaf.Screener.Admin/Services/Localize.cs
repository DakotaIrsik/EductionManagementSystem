using SilverLeaf.Screener.Admin.Resources;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System.Globalization;

namespace SilverLeaf.Screener.Admin.Services
{
    public abstract class Localize : ILocalize
    {
        public void SetLocale()
        {
            try
            {
                Lang.Culture = new CultureInfo(GetLocale());
            }
            catch (CultureNotFoundException)
            {
                Lang.Culture = new CultureInfo("en-US");
            }
        }

        protected abstract string GetLocale();
    }
}
