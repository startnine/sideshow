using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;

namespace Sideshow
{
    [TemplatePart(Name = PartSeconds, Type = typeof(RotateTransform))]
    [TemplatePart(Name = PartMinutes, Type = typeof(RotateTransform))]
    [TemplatePart(Name = PartHours, Type = typeof(RotateTransform))]
    public class AnalogClock : Control
    {
        public bool AutoDateTime
        {
            get => (bool)GetValue(AutoDateTimeProperty);
            set => SetValue(AutoDateTimeProperty, (value));
        }

        public static readonly DependencyProperty AutoDateTimeProperty =
            DependencyProperty.Register("AutoDateTime", typeof(bool), typeof(AnalogClock), new PropertyMetadata(true, OnAutoDateTimePropertyChangedCallback));

        static void OnAutoDateTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = (d as AnalogClock);

            if ((bool)(e.NewValue))
            {
                clock._clockTimer.Start();
            }
            else
            {
                clock._clockTimer.Stop();
                Debug.WriteLine("S T O P P");
            }
        }

        readonly Timer _clockTimer = new Timer(1);

        const string PartSeconds = "PART_SecondsHand";
        const string PartMinutes = "PART_MinutesHand";
        const string PartHours = "PART_HoursHand";

        RotateTransform _seconds;
        RotateTransform _minutes;
        RotateTransform _hours;

        public DateTime Time
        {
            get => (DateTime)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(AnalogClock), new PropertyMetadata(DateTime.Now));

        static void OnTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = (d as AnalogClock);
            clock.UpdateTime();
        }

        public AnalogClock()
        {
            _clockTimer.Elapsed += (sneder, args) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Time = DateTime.Now;
                    UpdateTime();
                }));
            };
            Loaded += (sneder, args) =>
            {
                _clockTimer.Start();
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var sec = GetTemplateChild(PartSeconds) as UIElement;
            if (sec != null)
            {
                _seconds = sec.RenderTransform as RotateTransform;
            }

            var min = GetTemplateChild(PartMinutes) as UIElement;
            if (min != null)
            {
                _minutes = min.RenderTransform as RotateTransform;
            }

            var hr = GetTemplateChild(PartHours) as UIElement;
            if (hr != null)
            {
                _hours = hr.RenderTransform as RotateTransform;
            }

            UpdateTime();
        }

        void UpdateTime()
        {
            if (_seconds != null)
            {
                _seconds.Angle = (((double)(Time.Second)) / 60) * 360;
            }
            else
            {
                Debug.WriteLine("_seconds");
            }

            if (_minutes != null)
            {
                _minutes.Angle = (((double)(Time.Minute)) / 60) * 360;
            }
            else
            {
                Debug.WriteLine("_minutes");
            }

            if (_hours != null)
            {
                _hours.Angle = (((double)(Time.Hour)) / 12) * 360;
            }
            else
            {
                Debug.WriteLine("_hours");
            }
        }
    }
}
