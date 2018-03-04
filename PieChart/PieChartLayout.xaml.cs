using System.Windows;
using System.Windows.Controls;
using PieChart.Implementation;

namespace PieChart
{
    /// <summary>
    ///     Defines the layout of the pie chart
    /// </summary>
    public partial class PieChartLayout : UserControl
    {
        public PieChartLayout()
        {
            InitializeComponent();
        }

        #region dependency properties

        /// <summary>
        ///     The property of the bound object that will be plotted (CLR wrapper)
        /// </summary>
        public string PlottedProperty
        {
            get => GetPlottedProperty(this);
            set => SetPlottedProperty(this, value);
        }

        // PlottedProperty dependency property
        public static readonly DependencyProperty PlottedPropertyProperty =
            DependencyProperty.RegisterAttached("PlottedProperty", typeof(string), typeof(PieChartLayout),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PlottedProperty attached property accessors
        public static void SetPlottedProperty(UIElement element, string value)
        {
            element.SetValue(PlottedPropertyProperty, value);
        }

        public static string GetPlottedProperty(UIElement element)
        {
            return (string)element.GetValue(PlottedPropertyProperty);
        }

        // Descriptor dependency property
        public static readonly DependencyProperty DescriptorProperty =
            DependencyProperty.RegisterAttached("Descriptor", typeof(string), typeof(PieChartLayout),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PlottedProperty attached property accessors
        public static void SetDescriptor(UIElement element, string value)
        {
            element.SetValue(DescriptorProperty, value);
        }

        public static string GetDescriptor(UIElement element)
        {
            return (string)element.GetValue(DescriptorProperty);
        }

        // Descriptor dependency property
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(PieChartLayout),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PropertyName attached property accessors
        public static void SetPropertyName(UIElement element, string value)
        {
            element.SetValue(PropertyNameProperty, value);
        }

        public static string GetPropertyName(UIElement element)
        {
            return (string)element.GetValue(PropertyNameProperty);
        }


        // SelectedPiece dependency property
        public static readonly DependencyProperty SelectedPieceProperty =
            DependencyProperty.RegisterAttached("SelectedPiece", typeof(object), typeof(PieChartLayout),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // SelectedPiece attached property accessors
        public static void SetSelectedPiece(UIElement element, object value)
        {
            element.SetValue(SelectedPieceProperty, value);
        }

        public static string GetSelectedPiece(UIElement element)
        {
            return (string)element.GetValue(SelectedPieceProperty);
        }

        /// <summary>
        ///     A class which selects a color based on the item being rendered.
        /// </summary>
        public IColorSelector ColorSelector
        {
            get => GetColorSelector(this);
            set => SetColorSelector(this, value);
        }

        // ColorSelector dependency property
        public static readonly DependencyProperty ColorSelectorProperty =
            DependencyProperty.RegisterAttached("ColorSelectorProperty", typeof(IColorSelector), typeof(PieChartLayout),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        // ColorSelector attached property accessors
        public static void SetColorSelector(UIElement element, IColorSelector value)
        {
            element.SetValue(ColorSelectorProperty, value);
        }

        public static IColorSelector GetColorSelector(UIElement element)
        {
            return (IColorSelector) element.GetValue(ColorSelectorProperty);
        }

        #endregion
    }
}