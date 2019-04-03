using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pandemic.Views
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();

            Focusable = true;
            Loaded += (_, __) => Keyboard.Focus(this);
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            FrameworkElement thumb = e.Source as FrameworkElement;
            var parent = (thumb.Parent as FrameworkElement);

            Thickness m = thumb.Margin;
            m.Left = thumb.Margin.Left + e.HorizontalChange;
            m.Top = thumb.Margin.Top + e.VerticalChange;

            if (m.Left > parent.ActualWidth - thumb.ActualWidth || m.Left < -(parent.ActualWidth - thumb.ActualWidth))
            {
                m.Left = thumb.Margin.Left;
            }
            if (m.Top > parent.ActualHeight - thumb.ActualHeight || m.Top < -(parent.ActualHeight - thumb.ActualHeight))
            {
                m.Top = thumb.Margin.Top;
            }

            thumb.Margin = m;
        }

        private void ActionPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ActionOverlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }
        }
    }
}