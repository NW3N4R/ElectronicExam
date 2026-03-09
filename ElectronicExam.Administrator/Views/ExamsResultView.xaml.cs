using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

using System.Collections.Generic;
using System.Linq;

namespace ElectronicExam.Administrator.Views
{
    public sealed partial class ExamsResultView : Page
    {
        public ExamsPrimary Exam { get; set; }
        public List<StudentsViewModel> StudentResultList { get; set; } = new();
        public ExamsResultView(ExamsPrimary _exam)
        {
            InitializeComponent();
            Exam = _exam;
        }
        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            load();
        }
        async void load()
        {
            await StudentsHelper.GetStudents();
            await JoinedStudentsHelper.GetJoinedStudents();
            await AnsweersHelper.GetAnsweers();
            await ExamQuestionsHelper.GetExamQuestions();
            var questions = ExamQuestionsHelper.questions.Where(x => x.ExamId == Exam.id);
            var answeers = AnsweersHelper.answeers.Where(x => questions.Any(q => x.QuestionId == q.id)).ToList();

            var joinedStudents = JoinedStudentsHelper.joinedStudents.Where(x => x.ExamId == Exam.id);
            var students = StudentsHelper.students.Where(x => joinedStudents.Any(j => x.id == j.StudentId)).ToList();
            StudentResultList.Clear();
            double totalMark = questions.Sum(x => x.Mark);
            var results = students.Select(s =>
            {
                int studentScore = answeers.Where(a => a.StudentId == s.id).Sum(a => a.Mark);
                bool didStudentAttend = joinedStudents.First(x => x.StudentId == s.id).JoinedDateTime is not null;
                return new StudentsViewModel
                {
                    id = s.id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    MiddleName = s.MiddleName,
                    Code = s.Code,
                    TotalMark = $"{studentScore} out of {totalMark}",
                    Status = didStudentAttend ? (studentScore >= (totalMark / 2)) ? "Pass" : "Failed" : "did not attend"
                };
            });
            StudentResultList.AddRange(results);
            this.DataContext = this;
        }
        public class StudentsViewModel : Students
        {
            public string Status { get; set; }

            public string TotalMark { get; set; }
        }

        private async void ShowStudentsAnsweers_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is not int id)
                return;
            var student = StudentsHelper.students.First(x => x.id == id);
            Flyout fl = new Flyout();
            fl.Content = new AnsweersView(student, Exam.id);
            fl.Placement = FlyoutPlacementMode.RightEdgeAlignedTop;
            fl.ShowAt(btn);
        }
    }
}
