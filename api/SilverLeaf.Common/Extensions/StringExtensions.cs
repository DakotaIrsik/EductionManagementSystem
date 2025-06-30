using System;

namespace SilverLeaf.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool? ToNullableBoolean(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "null")
            {
                return null;
            }
            return Convert.ToBoolean(value);
        }
    }
}
