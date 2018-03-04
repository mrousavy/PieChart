using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PieChart.Implementation;

namespace PieChart
{
    /// <summary>
    ///     Renders a bound dataset as a pie chart
    /// </summary>
    public partial class PiePlotter
    {
        /// <summary>
        ///     A list which contains the current piece pieces, where the piece index
        ///     is the same as the index of the item within the collection view which
        ///     it represents.
        /// </summary>
        private readonly List<PiePiece> _piePieces = new List<PiePiece>();

        public PiePlotter()
        {
            // register any dependency property change handlers
            var dpd = DependencyPropertyDescriptor.FromProperty(PieChartLayout.PlottedPropertyProperty,
                typeof(PiePlotter));
            dpd.AddValueChanged(this, PlottedPropertyChanged);

            InitializeComponent();

            DataContextChanged += DataContextChangedHandler;
        }

        private double GetPlottedPropertyValue(object item)
        {
            var filterPropDesc = TypeDescriptor.GetProperties(item);
            var itemValue = filterPropDesc[PlottedProperty].GetValue(item);

            return Convert.ToDouble(itemValue);
        }

        /// <summary>
        ///     Constructs pie pieces and adds them to the visual tree for this control's canvas
        /// </summary>
        private void ConstructPiePieces()
        {
            var myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            if(myCollectionView == null)
                return;

            double halfWidth = Width / 2;
            double innerRadius = halfWidth * HoleSize;

            // compute the total for the property which is being plotted
            double total = myCollectionView.Cast<object>().Sum(item => GetPlottedPropertyValue(item));

            // add the pie pieces
            Canvas.Children.Clear();
            _piePieces.Clear();

            double accumulativeAngle = 0;
            foreach(var item in myCollectionView)
            {
                bool selectedItem = item == myCollectionView.CurrentItem;

                double wedgeAngle = GetPlottedPropertyValue(item) * 360 / total;

                var piece = new PiePiece
                {
                    Radius = halfWidth,
                    InnerRadius = innerRadius,
                    CentreX = halfWidth,
                    CentreY = halfWidth,
                    PushOut = selectedItem ? 10.0 : 0,
                    WedgeAngle = wedgeAngle,
                    PieceValue = GetPlottedPropertyValue(item),
                    RotationAngle = accumulativeAngle,
                    Fill = ColorSelector != null
                        ? ColorSelector.SelectBrush(item, myCollectionView.IndexOf(item))
                        : Brushes.Black,
                    // record the index of the item which this pie slice represents
                    Tag = myCollectionView.IndexOf(item),
                    ToolTip = new ToolTip()
                };

                piece.ToolTipOpening += PiePieceToolTipOpening;
                piece.MouseUp += PiePieceMouseUp;

                _piePieces.Add(piece);
                Canvas.Children.Insert(0, piece);

                accumulativeAngle += wedgeAngle;
            }
        }

        /// <summary>
        ///     Handles the event which occurs just before a pie piece tooltip opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PiePieceToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var piece = (PiePiece)sender;

            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            if(collectionView == null)
                return;

            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            if(piece.ToolTip != null)
            {
                var tip = (ToolTip)piece.ToolTip;
                tip.DataContext = collectionView.GetItemAt(index);
            }
        }

        #region dependency properties

        /// <summary>
        ///     The property of the bound object that will be plotted
        /// </summary>
        public string PlottedProperty
        {
            get => PieChartLayout.GetPlottedProperty(this);
            set => PieChartLayout.SetPlottedProperty(this, value);
        }

        /// <summary>
        ///     The currently selected pie piece (type: type of Collection)
        /// </summary>
        public object SelectedPiece
        {
            get => PieChartLayout.GetSelectedPiece(this);
            set => PieChartLayout.SetSelectedPiece(this, value);
        }

        /// <summary>
        ///     A class which selects a color based on the item being rendered.
        /// </summary>
        public IColorSelector ColorSelector
        {
            get => PieChartLayout.GetColorSelector(this);
            set => PieChartLayout.SetColorSelector(this, value);
        }


        /// <summary>
        ///     The size of the hole in the centre of circle (as a percentage)
        /// </summary>
        public double HoleSize
        {
            get => (double)GetValue(HoleSizeProperty);
            set
            {
                SetValue(HoleSizeProperty, value);
                ConstructPiePieces();
            }
        }

        public static readonly DependencyProperty HoleSizeProperty =
            DependencyProperty.Register("HoleSize", typeof(double), typeof(PiePlotter), new UIPropertyMetadata(0.0));

        #endregion

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
            if(DataContext is INotifyCollectionChanged observable)
            {
                observable.CollectionChanged += BoundCollectionChanged;
            }

            // handle the selection change events
            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            collectionView.CurrentChanged += CollectionViewCurrentChanged;
            collectionView.CurrentChanging += CollectionViewCurrentChanging;

            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        ///     Handles changes to the PlottedProperty property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlottedPropertyChanged(object sender, EventArgs e)
        {
            ConstructPiePieces();
        }

        #endregion

        #region event handlers

        /// <summary>
        ///     Handles the MouseUp event from the individual Pie Pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PiePieceMouseUp(object sender, MouseButtonEventArgs e)
        {
            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            if(collectionView == null)
                return;

            if(!(sender is PiePiece piece))
                return;

            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            if(DataContext is IList list)
                SelectedPiece = list[index];

            collectionView.MoveCurrentToPosition(index);
        }

        /// <summary>
        ///     Handles the event which occurs when the selected item is about to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionViewCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            var collectionView = (CollectionView)sender;

            if(collectionView != null && collectionView.CurrentPosition >= 0 &&
                collectionView.CurrentPosition <= _piePieces.Count)
            {
                var piece = _piePieces[collectionView.CurrentPosition];

                var a = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }
        }

        /// <summary>
        ///     Handles the event which occurs when the selected item has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionViewCurrentChanged(object sender, EventArgs e)
        {
            var collectionView = (CollectionView)sender;

            if(collectionView != null && collectionView.CurrentPosition >= 0 &&
                collectionView.CurrentPosition <= _piePieces.Count)
            {
                var piece = _piePieces[collectionView.CurrentPosition];

                var a = new DoubleAnimation
                {
                    To = 10,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }
        }

        /// <summary>
        ///     Handles events which are raised when the bound collection changes (i.e. items added/removed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        ///     Iterates over the items inthe bound collection, adding handlers for PropertyChanged events
        /// </summary>
        private void ObserveBoundCollectionChanges()
        {
            var myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);

            foreach(var item in myCollectionView)
                if(item is INotifyPropertyChanged observable)
                {
                    observable.PropertyChanged += ItemPropertyChanged;
                }
        }


        /// <summary>
        ///     Handles events which occur when the properties of bound items change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the property which this pie chart represents has changed, re-construct the pie
            if(e.PropertyName.Equals(PlottedProperty))
                ConstructPiePieces();
        }

        #endregion
    }
}