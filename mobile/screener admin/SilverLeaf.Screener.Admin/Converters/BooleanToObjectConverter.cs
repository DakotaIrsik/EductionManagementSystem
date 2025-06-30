using System;
using System.Globalization;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Converters
{
    public class BooleanToObjectConverter<T> : IValueConverter // combine with NotConverter
    {
        public T FalseObject { get; set; }

        public T TrueObject { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(TrueObject);
        }
    }
}
