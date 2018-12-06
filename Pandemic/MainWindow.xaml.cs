using System.Windows;
using System.Windows.Media;

namespace Pandemic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
        }
    }
}