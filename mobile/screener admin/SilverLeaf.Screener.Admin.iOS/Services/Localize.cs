using Foundation;

namespace SilverLeaf.Screener.iOS.Services
{
    public class Localize : Screener.Services.Localize
    {
        protected override string GetLocale()
        {
            var cl = NSLocale.CurrentLocale;
            return cl.LanguageCode;
        }
    }
}