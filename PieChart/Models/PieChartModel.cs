using PieChart.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PieChart.Models
{
    public class PieChartModel : INotifyPropertyChanged
    {
        public PieChartModel(PieChart view)
        {
            View = view;
        }



        #region Dependency Properties

        /// <summary>
        ///     The width of the pie chart
        /// </summary>
        public double PieWidth
        {
            get => (double)View.GetValue(PieWidthProperty);
            set
            {
                View.SetValue(PieWidthProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty PieWidthProperty =
            DependencyProperty.Register("PieWidth", typeof(double), typeof(PieChart), new UIPropertyMetadata(200.0));

        /// <summary>
        ///     The height of the pie chart
        /// </summary>
        public double PieHeight
        {
            get => (double)View.GetValue(PieHeightProperty);
            set
            {
                View.SetValue(PieHeightProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty PieHeightProperty =
            DependencyProperty.Register("PieHeight", typeof(double), typeof(PieChart), new UIPropertyMetadata(100.0));

        /// <summary>
        ///     The width of the pie chart legend
        /// </summary>
        public double LegendWidth
        {
            get => (double)View.GetValue(LegendWidthProperty);
            set
            {
                View.SetValue(LegendWidthProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty LegendWidthProperty =
            DependencyProperty.Register("LegendWidth", typeof(double), typeof(PieChart), new UIPropertyMetadata(200.0));

        /// <summary>
        ///     The height of the pie chart legend
        /// </summary>
        public double LegendHeight
        {
            get => (double)View.GetValue(LegendHeightProperty);
            set
            {
                View.SetValue(LegendHeightProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty LegendHeightProperty =
            DependencyProperty.Register("LegendHeight", typeof(double), typeof(PieChart), new UIPropertyMetadata(100.0));

        #endregion

        #region Properties
        public PieChart View { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
