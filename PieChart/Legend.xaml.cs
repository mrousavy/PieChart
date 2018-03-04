using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PieChart.Implementation;

namespace PieChart
{
    /// <summary>
    ///     A pie chart legend
    /// </summary>
    public partial class Legend : UserControl
    {
        public Legend()
        {
            // register any dependency property change handlers
            var dpd = DependencyPropertyDescriptor.FromProperty(PieChartLayout.PlottedPropertyProperty,
                typeof(PiePlotter));
            dpd.AddValueChanged(this, PlottedPropertyChanged);

            DataContextChanged += DataContextChangedHandler;

            InitializeComponent();
        }


        #region property change handlers

        /// <summary>
        ///     Handle changes in the datacontext. When a change occurs handlers are registered for events which
        ///     occur when the collection changes or any items within teh collection change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            // handle the events that occur when the bound collection changes
            if (DataContext is INotifyCollectionChanged observable)
                observable.CollectionChanged += BoundCollectionChanged;

            ObserveBoundCollectionChanges();
        }

        #endregion

        /// <summary>
        ///     Refreshes the view, re-computing any value which is derived from the data bindings
        /// </summary>
        private void RefreshView()
        {
            // when the PlottedProperty changes we need to recompute our bindings. However,
            // the legend is bound to the collection items, the properties of which have not changes.
            // Therefore, we use a bit of an ugly hack to fool the legend into thinking the datacontext
            // has changed which causes it to replot itself.
            var context = legend.DataContext;
            if (context != null)
            {
                legend.DataContext = null;
                legend.DataContext = context;
            }
        }

        #region dependency properties
        /// <summary>
        ///     The descriptor of the bound object that will be plotted
        /// </summary>
        public string Descriptor
        {
            get => PieChartLayout.GetDescriptor(this);
            set => PieChartLayout.SetDescriptor(this, value);
        }

        /// <summary>
        ///     The property of the bound object that will be plotted
        /// </summary>
        public string PlottedProperty
        {
            get => PieChartLayout.GetPlottedProperty(this);
            set => PieChartLayout.SetPlottedProperty(this, value);
        }

        /// <summary>
        ///     The property of the bound object that will be plotted
        /// </summary>
        public string PropertyName
        {
            get => PieChartLayout.GetPropertyName(this);
            set => PieChartLayout.SetPropertyName(this, value);
        }

        /// <summary>
        ///     A class which selects a color based on the item being rendered.
        /// </summary>
        public IColorSelector ColorSelector
        {
            get => PieChartLayout.GetColorSelector(this);
            set => PieChartLayout.SetColorSelector(this, value);
        }

        #endregion

        #region event handlers

        /// <summary>
        ///     Handles events which are raised when the bound collection changes (i.e. items added/removed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshView();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        ///     Handles changes to the PlottedProperty property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlottedPropertyChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        /// <summary>
        ///     Iterates over the items inthe bound collection, adding handlers for PropertyChanged events
        /// </summary>
        private void ObserveBoundCollectionChanges()
        {
            var myCollectionView = (CollectionView) CollectionViewSource.GetDefaultView(DataContext);

            foreach (var item in myCollectionView)
                if (item is INotifyPropertyChanged observable)
                    observable.PropertyChanged += ItemPropertyChanged;
        }

        /// <summary>
        ///     Handles events which occur when the properties of bound items change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the property which this pie chart represents has changed, re-construct the pie
            if (e.PropertyName.Equals(PlottedProperty)) RefreshView();
        }

        #endregion
    }
}