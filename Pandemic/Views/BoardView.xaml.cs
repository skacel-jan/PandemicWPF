using GalaSoft.MvvmLight.Ioc;
using Pandemic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
