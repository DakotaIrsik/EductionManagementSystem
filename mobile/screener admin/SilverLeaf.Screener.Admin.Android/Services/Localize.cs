using Java.Util;

namespace SilverLeaf.Screener.Admin.Droid.Services
{
    public class Localize : Admin.Services.Localize
    {
        protected override string GetLocale()
        {
            return Locale.Default.ToString().Replace("_", "-");
        }
    }
}