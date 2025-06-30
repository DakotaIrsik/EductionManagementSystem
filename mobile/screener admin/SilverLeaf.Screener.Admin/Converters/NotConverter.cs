using System;
using System.Globalization;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Converters
{
    public class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            if (value is string)
            {
                return string.IsNullOrWhiteSpace((string)value);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
