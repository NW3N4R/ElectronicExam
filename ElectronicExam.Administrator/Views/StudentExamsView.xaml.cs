using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicExam.Administrator.Views
{
    public sealed partial class StudentExamsView : Page
    {
        public Students? Student { get; set; }
        public List<ExamsModelWithStatus> ExamsResultsList { get; set; } = new();
        public StudentExamsView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Student = (Students)e.Parameter;
            load();
            base.OnNavigatedTo(e);
        }
        async void load()
        {
            ExamsResultsList.Clear();
            await JoinedStudentsHelper.GetJoinedStudents();
            var joined = JoinedStudentsHelper.joinedStudents.Where(x => x.StudentId == Student.id);
            await ExamPrimaryHelper.GetExamsPrimary();
            var exams = ExamPrimaryHelper.exams.Where(e => joined.Any(j => e.id == j.ExamId));
            await ExamQuestionsHelper.GetExamQuestions();
            await AnsweersHelper.GetAnsweers();
            var ExamResults = exams.Select(e =>
            {
                var questions = ExamQuestionsHelper.questions.Where(x => x.ExamId == e.id).ToList();
                var answeers = AnsweersHelper.answeers.Where(a => questions.Any(q => a.QuestionId == q.id)).ToList();
                var totalMark = questions.Sum(x => x.Mark);
                var tokenMark = answeers.Sum(x => x.Mark);
                var model = new ExamsModelWithStatus
                {
                    id = e.id,
                    Title = e.Title,
                    TeacherName = e.TeacherName,
                    SubjectName = e.SubjectName,
                    Questions = questions,
                    Answeers = answeers,
                    TokenMark = $"{tokenMark} out of {totalMark}",
                    Status = (tokenMark >= (totalMark / 2)) ? "Pass" : "Fail",
                    isPass = (tokenMark >= (totalMark / 2))
                };
                return model;
            });
            TotalExamsJoined = ExamResults.Count();
            ExamsPassed = ExamResults.Where(x => x.isPass).Count();
            ExamsFailed = ExamResults.Where(x => !x.isPass).Count();
            ExamsResultsList.AddRange(ExamResults);
            this.DataContext = this;
        }

        private async void SeeAnsweers_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is not int id)
                return;
            var exam = ExamPrimaryHelper.exams.First(x => x.id == id);
            var dialog = new ContentDialog
            {
                Title = $"{Student!.FirstName}'s Answeers to {exam.Title}",
                Content = new AnsweersView(Student, id),
                SecondaryButtonText = "Close",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        public class ExamsModelWithStatus : ExamsPrimary
        {
            public List<ExamQuestions> Questions { get; set; }

            public List<Answeers> Answeers { get; set; }

            public string TokenMark { get; set; }

            public string Status { get; set; }

            public bool isPass { get; set; }
        }

        public int TotalExamsJoined { get; set; }

        public int ExamsPassed { get; set; }

        public int ExamsFailed { get; set; }
    }
}
