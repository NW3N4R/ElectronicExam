using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElectronicExam.Administrator.Views
{
    public sealed partial class HomeView : Page
    {
        public HomeModel Analysis { get; set; }
        public HomeView()
        {
            InitializeComponent();
            Analysis = new HomeModel();
            this.DataContext = Analysis;
        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await StudentsHelper.GetStudents();
            await ExamPrimaryHelper.GetExamsPrimary();
            await ExamQuestionsHelper.GetExamQuestions();
            await AnsweersHelper.GetAnsweers();
            await JoinedStudentsHelper.GetJoinedStudents();
            var students = StudentsHelper.students;
            var exams = ExamPrimaryHelper.exams;
            var questions = ExamQuestionsHelper.questions;
            var answers = AnsweersHelper.answeers;
            var joinedExams = JoinedStudentsHelper.joinedStudents;

            Analysis.StudentsNo = students.Count;
            Analysis.TeachersNo = exams.DistinctBy(x => x.TeacherName).Count();
            Analysis.ExamsNo = exams.Count();
            List<Students> PassedStudents = new List<Students>();
            List<Students> FailedStudents = new List<Students>();
            foreach (var student in students)
            {
                var studentExams = joinedExams.Where(x => x.StudentId == student.id).ToList();
                bool hasFailedAny = studentExams.Any(je =>
                {
                    // Case: Didn't show up
                    if (je.JoinedDateTime == null) return true;

                    // Case: Calculate score for this specific exam
                    var examQuestions = questions.Where(q => q.ExamId == je.ExamId).ToList();
                    double totalPossible = examQuestions.Sum(q => q.Mark);

                    // Sum marks from the answers table for these specific questions
                    double studentScore = answers
                        .Where(a => a.StudentId == student.id && examQuestions.Any(q => q.id == a.QuestionId))
                        .Sum(a => a.Mark);

                    return studentScore < (totalPossible / 2.0); // True if failed
                });
                if (hasFailedAny)
                {
                    FailedStudents.Add(student);
                }
                else
                {
                    PassedStudents.Add(student);
                }
            }
            Analysis.FailedStudents = FailedStudents.Count();
            Analysis.PassedStudents = PassedStudents.Count();

            Analysis.RateOfSuccess = (((double)Analysis.PassedStudents / (double)students.Count) * 100).ToString("F2") + "%";
            Analysis.RateOfFail = (((double)Analysis.FailedStudents / (double)students.Count) * 100).ToString("F2") + "%";
        }
    }
}
