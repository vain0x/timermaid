using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using VainZero.Misc;

namespace VainZero.Windows.Converters
{
    public sealed class NullableDateTimeToStringConverter
        : IValueConverter
    {
        string Format(object parameter)
        {
            return (parameter as string) ?? "yyyy/MM/dd HH:mm:ss";
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime?)value;
            return dateTime?.ToLocalTime().ToString(Format(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (string)value;
            if (string.IsNullOrWhiteSpace(source)) return default(DateTime?);

            if (DateTime.TryParse(source, out var dateTime))
            {
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }
                return dateTime.ToUniversalTime();
            }

            return DependencyProperty.UnsetValue;
        }

        public static NullableDateTimeToStringConverter Instance { get; } =
            new NullableDateTimeToStringConverter();
    }
}
