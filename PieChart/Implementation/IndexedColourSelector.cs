using System.Windows;
using System.Windows.Media;

namespace PieChart.Implementation
{
    /// <summary>
    ///     Selects a colour purely based on its location within a collection.
    /// </summary>
    public class IndexedColourSelector : DependencyObject, IColorSelector
    {
        public static readonly DependencyProperty BrushesProperty =
            DependencyProperty.Register("BrushesProperty", typeof(Brush[]), typeof(IndexedColourSelector),
                new UIPropertyMetadata(null));

        /// <summary>
        ///     An array of brushes.
        /// </summary>
        public Brush[] Brushes
        {
            get => (Brush[]) GetValue(BrushesProperty);
            set => SetValue(BrushesProperty, value);
        }


        public Brush SelectBrush(object item, int index)
        {
            if (Brushes == null || Brushes.Length == 0) return System.Windows.Media.Brushes.Black;
            return Brushes[index % Brushes.Length];
        }
    }
}