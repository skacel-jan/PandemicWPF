using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pandemic
{
    public class TouchScrolling : DependencyObject
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TouchScrolling), new UIPropertyMetadata(false, IsEnabledChanged));

        private static Dictionary<object, MouseCapture> _captures = new Dictionary<object, MouseCapture>();

        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ScrollViewer target)) return;

            if ((bool)e.NewValue)
            {
                target.Loaded += target_Loaded;
            }
            else
            {
                target_Unloaded(target, new RoutedEventArgs());
            }
        }

        private static void target_Unloaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Target Unloaded");

            if (!(sender is ScrollViewer target)) return;

            _captures.Remove(sender);

            target.Loaded -= target_Loaded;
            target.Unloaded -= target_Unloaded;
            target.PreviewMouseLeftButtonDown -= target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove -= target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp -= target_PreviewMouseLeftButtonUp;
        }

        private static void target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ScrollViewer target)) return;

            _captures[sender] = new MouseCapture
            {
                HorizontalOffset = target.HorizontalOffset,
                Point = e.GetPosition(target),
            };
        }

        private static void target_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is ScrollViewer target)) return;

            System.Diagnostics.Debug.WriteLine("Target Loaded");

            target.Unloaded += target_Unloaded;
            target.PreviewMouseLeftButtonDown += target_PreviewMouseLeftButtonDown;
            target.PreviewMouseMove += target_PreviewMouseMove;

            target.PreviewMouseLeftButtonUp += target_PreviewMouseLeftButtonUp;
        }

        private static void target_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ScrollViewer target)) return;

            target.ReleaseMouseCapture();

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        if (!target.CheckAccess())
            //        {
            //            target.Dispatcher.Invoke(() => target.ScrollToHorizontalOffset(target.HorizontalOffset - 2));
            //        }

            //        Thread.Sleep(10);
            //    }
            //});
        }

        private static void target_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_captures.ContainsKey(sender)) return;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _captures.Remove(sender);
                return;
            }

            if (!(sender is ScrollViewer target)) return;

            var capture = _captures[sender];

            var point = e.GetPosition(target);

            var dx = point.X - capture.Point.X;

            if (Math.Abs(dx) > 5)
            {
                target.CaptureMouse();
            }

            var newOffset = capture.HorizontalOffset - dx;
            if (newOffset < 0)
            {
                target.ScrollToRightEnd();
                _captures[sender] = new MouseCapture
                {
                    HorizontalOffset = target.HorizontalOffset,
                    Point = e.GetPosition(target),
                };
            }
            else if (newOffset > target.ActualWidth)
            {
                target.ScrollToLeftEnd();
                _captures[sender] = new MouseCapture
                {
                    HorizontalOffset = target.HorizontalOffset,
                    Point = e.GetPosition(target),
                };
            }
            else
            {
                target.ScrollToHorizontalOffset(newOffset);
            }
        }

        internal class MouseCapture
        {
            public Double HorizontalOffset { get; set; }
            public Point Point { get; set; }
        }
    }
}