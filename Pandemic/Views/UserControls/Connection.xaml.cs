using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pandemic.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Point), typeof(Connection), new FrameworkPropertyMetadata(default(Point),
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty DestinationProperty =
            DependencyProperty.Register(nameof(Destination), typeof(Point), typeof(Connection), new FrameworkPropertyMetadata(default(Point),
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Connection()
        {
            InitializeComponent();
        }

        public Point Source
        {
            get { return (Point)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public Point Destination
        {
            get { return (Point)this.GetValue(DestinationProperty); }
            set { this.SetValue(DestinationProperty, value); }
        }
    }
}
