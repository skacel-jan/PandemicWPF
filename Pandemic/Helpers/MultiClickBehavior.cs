using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Pandemic.Helpers
{
    public class MultiClickBehavior
    {
        #region private timer

        public const int DOUBLE_CLICK_TIME = 500;

        public static readonly DependencyProperty ClickWaitTimer = DependencyProperty.RegisterAttached("Timer", typeof(DispatcherTimer), typeof(MultiClickBehavior));

        private static DispatcherTimer GetClickWaitTimer(DependencyObject obj)
        {
            return (DispatcherTimer)obj.GetValue(ClickWaitTimer);
        }

        private static void SetClickWaitTimer(DependencyObject obj, DispatcherTimer timer)
        {
            obj.SetValue(ClickWaitTimer, timer);
        }

        #endregion private timer

        #region single click dependency properties

        public static ICommand GetSingleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SingleClickCommandProperty);
        }

        public static void SetSingleClickCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(SingleClickCommandProperty, command);
        }

        public static readonly DependencyProperty SingleClickCommandProperty = DependencyProperty.RegisterAttached("SingleClickCommand",
            typeof(ICommand), typeof(MultiClickBehavior),
            new UIPropertyMetadata(null, CommandChanged));

        public static object GetSingleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(SingleClickCommandParameterProperty);
        }

        public static void SetSingleClickCommandParameter(DependencyObject obj, ICommand command)
        {
            obj.SetValue(SingleClickCommandParameterProperty, command);
        }

        public static readonly DependencyProperty SingleClickCommandParameterProperty = DependencyProperty.RegisterAttached("SingleClickCommandParameter",
            typeof(object), typeof(MultiClickBehavior));

        #endregion single click dependency properties

        #region double click dependency properties

        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(DoubleClickCommandProperty, command);
        }

        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClickCommand",
            typeof(ICommand), typeof(MultiClickBehavior),
            new UIPropertyMetadata(null, CommandChanged));

        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandParameter(DependencyObject obj, object parameter)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, parameter);
        }

        public static readonly DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached("DoubleClickCommandParameter",
            typeof(object), typeof(MultiClickBehavior));

        public static bool GetDoubleClickCommandEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandEnabled(DependencyObject obj, bool enabled)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, enabled);
        }

        public static readonly DependencyProperty DoubleClickCommandEnabled = DependencyProperty.RegisterAttached("DoubleClickCommandEnabled",
            typeof(bool), typeof(MultiClickBehavior), new UIPropertyMetadata(EnabledChanged));

        private static void EnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool enabled = (bool)d.GetValue(e.Property);
            DispatcherTimer timer = GetClickWaitTimer(d);
            if (enabled)
            {
                if (timer != null)
                {
                    timer.Tag = "stopped";
                }
            }
            else
            {
                if (timer != null)
                {
                    timer.Tag = null;
                }
            }
        }

        #endregion double click dependency properties

        private static void CommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UIElement targetElement)
            {
                //remove any existing handlers
                targetElement.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(element_MouseLeftButtonDown));
                //use AddHandler to be able to listen to handled events
                targetElement.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(element_MouseLeftButtonDown), true);

                //if the timer has not been created then do so
                var timer = GetClickWaitTimer(targetElement);

                if (timer == null)
                {
                    timer = new DispatcherTimer() { IsEnabled = false };
                    timer.Interval = new TimeSpan(0, 0, 0, 0, DOUBLE_CLICK_TIME);
                    timer.Tick += (s, args) =>
                    {
                        //if the interval has been reached without a second click then execute the SingClickCommand
                        timer.Stop();

                        var commandParameter = targetElement.GetValue(SingleClickCommandParameterProperty);

                        if (targetElement.GetValue(SingleClickCommandProperty) is ICommand command)
                        {
                            if (command.CanExecute(e))
                            {
                                command.Execute(commandParameter);
                            }
                        }
                    };

                    SetClickWaitTimer(targetElement, timer);
                }
            }
        }

        private static void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement targetElement)
            {
                var timer = GetClickWaitTimer(targetElement);

                //if the timer is enabled there has already been one click within the interval and this is a second click so
                //stop the timer and execute the DoubleClickCommand
                if (timer.IsEnabled)
                {
                    timer.Stop();

                    var commandParameter = targetElement.GetValue(DoubleClickCommandParameterProperty);

                    if (targetElement.GetValue(DoubleClickCommandProperty) is ICommand command)
                    {
                        if (command.CanExecute(e))
                        {
                            command.Execute(commandParameter);
                        }
                    }
                }
                else if (timer.Tag == null || timer.Tag.ToString() != "stopped")
                {
                    timer.Start();
                }
                else
                {
                    var commandParameter = targetElement.GetValue(SingleClickCommandParameterProperty);

                    if (targetElement.GetValue(SingleClickCommandProperty) is ICommand command)
                    {
                        if (command.CanExecute(e))
                        {
                            command.Execute(commandParameter);
                        }
                    }
                }
            }
        }
    }
}