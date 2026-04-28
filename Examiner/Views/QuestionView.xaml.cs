using Examiner.Helpers;
using Examiner.Models;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Linq;
using Windows.System;
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

            // NOTE this section is fast forwarding the exam duration to simulate real time
            // 1 min =  60,000 ms
            // 0.01m * 60,000 ms = 600ms
            _timer.Interval = TimeSpan.FromMinutes(0.01).TotalMilliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            this.KeyDown += QuestionView_KeyDown;
        }

        private async void QuestionView_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {

            var ctrl = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

            if (ctrl &&
                (e.Key == VirtualKey.C ||
                 e.Key == VirtualKey.V ||
                 e.Key == VirtualKey.X))
            {

                var dialog = new ContentDialog
                {
                    Title = "Copy Paste is not Allowed",
                    Content = "the examiner doesnt accept Copy Paste, your results will be marked as failure",
                    PrimaryButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                dialog.PrimaryButtonClick += (s, e) => MainWindow.Instance.currentSession.StopExam(true);
                await dialog.ShowAsync();
                e.Handled = true;

            }
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            QuestionsTileList.ItemsSource = session.ExamQuestions;
            session.Duration = ((double)session!.ExamHeader!.ExamDuration * 60);
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

        private async void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MainWindow.Instance.currentSession.StopExam();
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
                ContentDialog answersRemainDialog = new ContentDialog()
                {
                    Title = "some answeers remain",
                    Content = "please answeer all the questions",
                    SecondaryButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };
                await answersRemainDialog.ShowAsync();
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
                        Mark = q.SelectedAnsweer == q.CorrectAnsweer ? q.Mark : 0
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
