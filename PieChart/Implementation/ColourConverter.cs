using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PieChart.Implementation
{
    /// <summary>
    ///     Converter which uses the IColorSelector associated with the Legend to
    ///     select a suitable color for rendering an item.
    /// </summary>
    [ValueConversion(typeof(object), typeof(Brush))]
    public class ColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // find the item 
            var element = (FrameworkElement) value;
            var item = element.Tag;

            // find the item container
            var container = Helper.FindElementOfTypeUp(element, typeof(ListBoxItem));

            // locate the items control which it belongs to
            var owner = ItemsControl.ItemsControlFromItemContainer(container);

            // locate the legend
            var legend = (Legend) Helper.FindElementOfTypeUp(owner, typeof(Legend));

            var collectionView = (CollectionView) CollectionViewSource.GetDefaultView(owner.DataContext);

            // locate this item (which is bound to the tag of this element) within the collection
            int index = collectionView.IndexOf(item);

            if (legend.ColorSelector != null)
                return legend.ColorSelector.SelectBrush(item, index);
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}