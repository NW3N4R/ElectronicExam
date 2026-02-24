using Examiner.Helpers;
using Examiner.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Examiner
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public CurrentSession currentSession;
        public MainWindow()
        {
            InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            Instance = this;
            currentSession = new();
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

        public async void StartExam()
        {
            if (currentSession.ExamHeader is null || currentSession.LoggedStudent is null)
            {
                ShowInfo("an unhandled exaception occured", InfoBarSeverity.Error);
                return;
            }
            await ExamQuestionsHelper.GetExamQuestions();
            currentSession.ExamQuestions =
                new ObservableCollection<Models.ExamQuestions>(
                    ExamQuestionsHelper.questions
                        .Where(x => x.ExamId == currentSession.ExamHeader.id)
                );
            mainWindowFrame.Content = new QuestionView();
        }
    }
}
