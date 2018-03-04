using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PieChart.Implementation
{
    /// <summary>
    ///     Obtain the value of the property from the item, which is currently displayed by the pie chart.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class LegendConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // the item which we are displaying is bound to the Tag property
            var label = (TextBlock) value;
            var item = label.Tag;

            // find the item container
            DependencyObject container = Helper.FindElementOfTypeUp((Visual) value, typeof(ListBoxItem));

            // locate the items control which it belongs to
            var owner = ItemsControl.ItemsControlFromItemContainer(container);

            // locate the legend
            var legend = (Legend) Helper.FindElementOfTypeUp(owner, typeof(Legend));

            var filterPropDesc = TypeDescriptor.GetProperties(item);
            var itemValue = filterPropDesc[legend.PlottedProperty].GetValue(item);
            return itemValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}