using System;
using System.Globalization;

using Xamarin.Forms;

namespace SOColeta.Converters
{
    internal class DenyBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool obj)
                return !obj;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool obj)
                return !obj;
            return false;
        }
    }
}
