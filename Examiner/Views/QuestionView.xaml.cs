using Examiner.Helpers;
using Examiner.Models;

using Microsoft.UI.Xaml.Controls;

using System;
using System.Linq;
namespace Examiner.Views
{
    public sealed partial class QuestionView : Page
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        CurrentSession session => MainWindow.Instance.currentSession;
        public QuestionView()
        {
            InitializeComponent();
            TitleBar.DataContext = new titleBarModel();
            session.ExamQuestions!.CollectionChanged += ExamQuestions_CollectionChanged;
            _timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

        }
        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            QuestionsTileList.ItemsSource = session.ExamQuestions;
            session.Duration = ((double)session.ExamHeader!.DurationHour * 60) + session.ExamHeader.DurationMin;
            TimerProgress.DataContext = session;
        }
        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(async () =>
            {
                session.PassedMinutes += 1;
                ProgressLabe.Text = $"Minutes Elapsed: {session.PassedMinutes}";

                if (session.PassedMinutes >= session.Duration)
                {
                    _timer.Stop();
                    var dialog = new ContentDialog
                    {
                        Title = "time limit reached",
                        Content = "you may send the answeers, sorry",
                        PrimaryButtonText = "send answeers",
                        XamlRoot = this.XamlRoot,

                    };
                    dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
                    dialog.Closing += (s, e) =>
                    {
                        // Cancel the close
                        e.Cancel = true;
                    };

                    await dialog.ShowAsync();
                }
            });

        }

        private void Dialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = true;
        }

        private async void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            foreach (var q in session.ExamQuestions!)
            {
                var answerModel = new Answeers
                {
                    StudentId = session.LoggedStudent!.id,
                    QuestionId = q.id,
                    SelectedAnsweer = q.SelectedAnsweer,
                    ExamId = session.ExamHeader!.id,
                    Mark = (byte)(q.SelectedAnsweer == q.CorrectAnsweer ? q.Mark : 0)
                };
                await AnsweersHelper.InsertAnsweer(answerModel);
            }
            Microsoft.UI.Xaml.Application.Current.Exit();
        }

        private void ExamQuestions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SendAnsweers.IsEnabled = !session.ExamQuestions!.Any(x => !x.isAnsweered);
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
                        Mark = (byte)(q.SelectedAnsweer == q.CorrectAnsweer ? q.Mark : 0)
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
