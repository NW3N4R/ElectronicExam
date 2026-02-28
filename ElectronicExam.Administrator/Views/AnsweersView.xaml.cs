using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Linq;


namespace ElectronicExam.Administrator.Views
{
    public sealed partial class AnsweersView : Page
    {
        public Students Student { get; set; }
        public List<studentAnsweersModel> answeersList { get; set; } = new();
        private int ExamId;
        public AnsweersView(Students _students, int _examId)
        {
            InitializeComponent();
            Student = _students;
            ExamId = _examId;
        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var answers = AnsweersHelper.answeers.Where(x => x.StudentId == Student.id);
            var questions = ExamQuestionsHelper.questions.Where(x => x.ExamId == ExamId);
            answeersList.Clear();
            var ans = answers.Select(a =>
            {
                var ques = questions.First(x => x.id == a.QuestionId);
                var model = new studentAnsweersModel
                {
                    id = a.StudentId,
                    Question = ques.Question,
                    Mark = $"{a.Mark} out of {ques.Mark}",
                    Status = a.Mark == ques.Mark ? "Correct Answeer" : "Wrong Answeer"
                };
                return model;
            });
            answeersList.AddRange(ans);
            this.DataContext = this;
        }
        public class studentAnsweersModel : Students
        {
            public string Question { get; set; }

            public string Mark { get; set; }

            public string Status { get; set; }
        }
    }
}
