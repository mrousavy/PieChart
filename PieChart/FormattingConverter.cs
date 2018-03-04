using System;
using System.Globalization;
using System.Windows.Data;

namespace PieChart
{
    /// <summary>
    ///     A value converter that delegates to String.Format
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class FormattingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (parameter is string formatString)
                return string.Format(culture, formatString, value);
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}