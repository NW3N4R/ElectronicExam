using Microsoft.UI.Xaml;
namespace Examiner
{
    public partial class App : Application
    {
        private Window? _window;
 
        public App()
        {
            InitializeComponent();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
