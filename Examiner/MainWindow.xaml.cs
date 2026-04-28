using Examiner.Helpers;
using Examiner.Views;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using WinRT.Interop;

namespace Examiner
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public CurrentSession currentSession;
        public static bool allowClose = false;
        public MainWindow()
        {
            InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            if (AppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            }
            Instance = this;
            currentSession = new();

            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            appWindow.Closing += AppWindow_Closing;

        }
    
        private async void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            try
            {
                if (!allowClose)
                {
                    args.Cancel = true;

                    var dialog = new ContentDialog
                    {
                        Title = "Closing App is Not Allowed",
                        Content = "closing the examiner app by your own marks your result to failure",
                        PrimaryButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void mainWindowFrame_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindowFrame.Content = new Login();
            await ConnectionHelper.OpenConnectionAsync();
        }

        public async void ShowInfo(string message, InfoBarSeverity sev)
        {
            mainInfo.IsOpen = false;
            await Task.Delay(500);
            mainInfo.Severity = sev;
            mainInfo.Content = message;
            mainInfo.IsClosable = false;
            mainInfo.IsOpen = true;
            hideMainInfo();
        }

        private async void hideMainInfo()
        {
            await Task.Delay(2000);
            mainInfo.IsOpen = false;
        }
    }
}
