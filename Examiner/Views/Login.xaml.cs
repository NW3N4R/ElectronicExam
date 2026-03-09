using Examiner.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Linq;


namespace Examiner.Views
{
    public sealed partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }


        private async void SubmitBttn_Click(object sender, RoutedEventArgs e)
        {
            DetailsStack.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(StudentCode.Text) || string.IsNullOrEmpty(StudentPhone.Text))
            {
                MainWindow.Instance.ShowInfo("Provide Your Details", InfoBarSeverity.Error);
                return;
            }
            var student = await StudentsHelper.GetStudents(StudentCode.Text, StudentPhone.Text);
            if (student is null)
            {
                MainWindow.Instance.ShowInfo("Invalid Detail", InfoBarSeverity.Error);
                return;
            }
            MainWindow.Instance.ShowInfo("Login Success", InfoBarSeverity.Success);
            MainWindow.Instance.currentSession.LoggedStudent = student;
            var examIds = await JoinedStudentsHelper.GetJoinedStudents(student.id);
            if (examIds is null)
            {
                MainWindow.Instance.ShowInfo("No Exam Assigned to this Student", InfoBarSeverity.Error);
                return;
            }
            await ExamPrimaryHelper.GetExamsPrimary();
            var exams = ExamPrimaryHelper.exams.Where(x => examIds.Any(eid => eid.ExamId == x.id));
            ExamSelector.ItemsSource = exams;
            ExamSelector.SelectedIndex = 0;
            DetailsStack.DataContext = MainWindow.Instance.currentSession.LoggedStudent;
            DetailsStack.Visibility = Visibility.Visible;

        }

        private void ExamSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExamSelector.SelectedValue is int selectedId)
            {
                MainWindow.Instance.currentSession.ExamHeader = ExamPrimaryHelper.exams.First(x => x.id == selectedId);
            }
        }
    
        private async void LoginBttn_Click(object sender, RoutedEventArgs e)
        {

            await JoinedStudentsHelper.joinStudent(MainWindow.Instance.currentSession.LoggedStudent!.id, MainWindow.Instance.currentSession.ExamHeader!.id);
           MainWindow.Instance.currentSession.StartExam();
        }
    }
}
