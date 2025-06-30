using System.Collections.Generic;
using System.Linq;

namespace SilverLeaf.Common.Extensions
{
    public static class MappingExtensions
    {
        public static List<string> ToListFromCsv(this string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return null;
            }

            return model.Split(',').Select(x => x).ToList();
        }

        public static string ToCsvFromList(this IEnumerable<object> model)
        {
            if (!model.Safe().Any())
            {
                return null;
            }

            return string.Join(",", model);
        }
    }
}
