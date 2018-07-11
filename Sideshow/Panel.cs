using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Sideshow
{
    [TemplatePart(Name = PartResizeGrip, Type = typeof(Thumb))]
    class Panel : ContentControl
    {
        const String PartResizeGrip = "PART_ResizeGrip";

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(Panel),
                new PropertyMetadata(Orientation.Vertical));

        public Boolean IsDirectionReversed
        {
            get => (Boolean)GetValue(IsDirectionReversedProperty);
            set => SetValue(IsDirectionReversedProperty, value);
        }

        public static readonly DependencyProperty IsDirectionReversedProperty =
            DependencyProperty.RegisterAttached("IsDirectionReversed", typeof(Boolean), typeof(Panel),
                new PropertyMetadata(false));

        [TypeConverterAttribute(typeof(LengthConverter))]
        public Double Length
        {
            get => (Double)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.RegisterAttached("Length", typeof(Double), typeof(Panel), new PropertyMetadata((Double)100, OnDimensionalPropertyChangedCallback));

        /*[TypeConverterAttribute(typeof(LengthConverter))]
        public Double MinLength
        {
            get => (Double)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.RegisterAttached("MinLength", typeof(Double), typeof(Panel), new PropertyMetadata(0, OnDimensionalPropertyChangedCallback));

        [TypeConverterAttribute(typeof(LengthConverter))]
        public Double MaxLength
        {
            get => (Double)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.RegisterAttached("MaxLength", typeof(Double), typeof(Panel), new PropertyMetadata(Double.PositiveInfinity, OnDimensionalPropertyChangedCallback));*/

        static void OnDimensionalPropertyChangedCallback(Object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            if (panel.Orientation == Orientation.Horizontal)
            {
                BindingOperations.SetBinding(panel, Panel.WidthProperty, panel._lengthBinding);

                if (BindingOperations.GetBinding(panel, Panel.HeightProperty) == panel._lengthBinding)
                    BindingOperations.ClearBinding(panel, Panel.HeightProperty);

                /*panel.Height = double.NaN;
                if (panel.Width > panel.MaxLength)
                    panel.Width = panel.MaxLength;
                else if (panel.Width < panel.MinLength)
                    panel.Width = panel.MinLength;
                else
                    panel.Width = panel.Length;*/
            }
            else
            {
                BindingOperations.SetBinding(panel, Panel.HeightProperty, panel._lengthBinding);

                if (BindingOperations.GetBinding(panel, Panel.WidthProperty) == panel._lengthBinding)
                    BindingOperations.ClearBinding(panel, Panel.WidthProperty);

                /*panel.Width = double.NaN;
                if (panel.Height > panel.MaxLength)
                    panel.Height = panel.MaxLength;
                else if (panel.Height < panel.MinLength)
                    panel.Height = panel.MinLength;
                else
                    panel.Height = panel.Length;*/
            }
        }

        Binding _lengthBinding;

        public Panel()
        {
            _lengthBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("Length"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            OnDimensionalPropertyChangedCallback(this, new DependencyPropertyChangedEventArgs());
        }

        Thumb _resizeGrip;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _resizeGrip = GetTemplateChild(PartResizeGrip) as Thumb;
            if (_resizeGrip != null)
            {
                _resizeGrip.DragDelta += ResizeGrip_DragDelta;
            }
        }

        private void ResizeGrip_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (IsDirectionReversed)
                    Length += e.HorizontalChange;
                else
                    Length -= e.HorizontalChange;
            }
            else
            {
                if (IsDirectionReversed)
                    Length += e.VerticalChange;
                else
                    Length -= e.VerticalChange;
            }
        }
    }
}
