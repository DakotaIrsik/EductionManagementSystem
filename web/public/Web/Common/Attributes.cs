using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SilverLeaf.Public.Web.Common
{
    public class Alphanumeric : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex r = new Regex(@"^[a-zA-Z0-9\s]*$");
            return !string.IsNullOrWhiteSpace(value?.ToString()) &&  r.IsMatch(value.ToString());
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}
