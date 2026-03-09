using CommunityToolkit.Mvvm.ComponentModel;
using Examiner.Helpers;
using Examiner.Models;
using Examiner.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace Examiner
{
    public class CurrentSession : ObservableObject
    {
        public Students? LoggedStudent { get; set; }
        public ExamsPrimary? ExamHeader { get; set; }
        public double Duration
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 1;
        public double PassedMinutes
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 0;
        public ObservableCollection<ExamQuestions>? ExamQuestions
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public async void StartExam()
        {
            if (ExamHeader is null || LoggedStudent is null)
            {
                MainWindow.Instance.ShowInfo("an unhandled exaception occured", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                return;
            }
            await ExamQuestionsHelper.GetExamQuestions();
            ExamQuestions =
                new ObservableCollection<Models.ExamQuestions>(
                    ExamQuestionsHelper.questions
                        .Where(x => x.ExamId == ExamHeader.id)
                );
            MainWindow.Instance.Content = new QuestionView();
        }
        public async void StopExam(bool Expelled = false)
        {
            if (ExamQuestions == null || LoggedStudent == null || ExamHeader == null)
                return;
            foreach (var q in ExamQuestions!)
            {
                var answerModel = new Answeers
                {
                    StudentId = LoggedStudent!.id,
                    QuestionId = q.id,
                    SelectedAnsweer = q.SelectedAnsweer,
                    ExamId = ExamHeader!.id,
                    Mark = (Expelled ?  -1 : (q.SelectedAnsweer == q.CorrectAnsweer ? q.Mark : 0))
                };
                await AnsweersHelper.InsertAnsweer(answerModel);
            }
            Microsoft.UI.Xaml.Application.Current.Exit();
        }
    }
}
