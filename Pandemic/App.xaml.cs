using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Windows;

namespace Pandemic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //AppCenter.Start("e5c882b7-812c-4424-aa23-87e7026b69b8",
            //       typeof(Analytics), typeof(Crashes));
        }
    }
}