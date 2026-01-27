using ElectronicExam.Administrator.Helpers;

using Microsoft.UI.Xaml;


namespace ElectronicExam.Administrator
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            load();
        }
        async void load()
        {
            await ConnectionHelper.OpenConnectionAsync();
            await StudentsHelper.GetStudents();
            await ExamPrimaryHelper.GetExamsPrimary();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
