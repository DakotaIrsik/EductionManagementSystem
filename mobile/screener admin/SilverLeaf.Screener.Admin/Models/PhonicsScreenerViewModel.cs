using SilverLeaf.Entities.Models;

namespace SilverLeaf.Screener.Admin.Models
{
    public class PhonicsScreenerViewModel : PhonicsScreenerDTO
    {
        public double FontSize
        {
            get
            {
                var size = 100;
                if (Prefix == null)
                {
                    size = 80;
                }
                else if (Prefix.ToLower().Contains("word"))
                {
                    size = 60;
                }
                else if (Prefix.ToLower().Contains("sentence") || Prefix.ToLower().Contains("finished"))
                {
                    size = 40;
                }

                return size;
            }
        }
    }
}
