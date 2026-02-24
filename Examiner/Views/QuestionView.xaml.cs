using Examiner.Helpers;
using Examiner.Models;

using Microsoft.UI.Xaml.Controls;

using System;
using System.Diagnostics;
using System.Linq;


namespace Examiner.Views
{
    public sealed partial class QuestionView : Page
    {
        CurrentSession session => MainWindow.Instance.currentSession;
        public QuestionView()
        {
            InitializeComponent();
            TitleBar.DataContext = new titleBarModel();
            session.ExamQuestions!.CollectionChanged += ExamQuestions_CollectionChanged;
        }

        private void ExamQuestions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("collection changed from q view");
            SendAnsweers.IsEnabled = !session.ExamQuestions!.Any(x => !x.isAnsweered);
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            QuestionsTileList.ItemsSource = session.ExamQuestions;
        }

        private void Previous_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (QuestionsTileList.SelectedIndex == 0)
                return;
            QuestionsTileList.SelectedIndex = --QuestionsTileList.SelectedIndex;
        }

        private void Next_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (session.ExamQuestions!.Count == QuestionsTileList.SelectedIndex + 1)
                return;
            QuestionsTileList.SelectedIndex = ++QuestionsTileList.SelectedIndex;
        }

        private void QuestionsTileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentQuestion = session.ExamQuestions![QuestionsTileList.SelectedIndex];
            CurrentQuestionContainer.DataContext = currentQuestion;
            Previous.IsEnabled = QuestionsTileList.SelectedIndex != 0;
            Next.IsEnabled = QuestionsTileList.SelectedIndex + 1 != session.ExamQuestions.Count;
        }
        private async void SendAnsweers_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (session.ExamQuestions!.Any(x => !x.isAnsweered))
            {
                MainWindow.Instance.ShowInfo("You have at least one \nremaining question to answer", InfoBarSeverity.Warning);
                return;
            }
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Did you review your answers?",
                Content = "it's recommended to review your answers before sending them",
                SecondaryButtonText = "Yes, Proceed",
                PrimaryButtonText = "Let me Think Twice!",
                XamlRoot = this.XamlRoot,
            };
            dialog.SecondaryButtonClick += async (s, e) =>
            {
                foreach (var q in session.ExamQuestions!)
                {
                    var answerModel = new Answeers
                    {
                        StudentId = session.LoggedStudent!.id,
                        QuestionId = q.id,
                        SelectedAnsweer = q.SelectedAnsweer,
                        ExamId = session.ExamHeader!.id,
                        isCorrect = q.CorrectAnsweer == q.SelectedAnsweer
                    };
                    await AnsweersHelper.InsertAnsweer(answerModel);
                }
                Microsoft.UI.Xaml.Application.Current.Exit();
            };
            await dialog.ShowAsync();

        }
    }
    class titleBarModel
    {
        public Students student => MainWindow.Instance.currentSession.LoggedStudent!;
        public ExamsPrimary exam => MainWindow.Instance.currentSession.ExamHeader!;
    }
}
