using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Updaters;
using ElectronicExam.Administrator.Uploaders;

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Linq;
namespace ElectronicExam.Administrator.Views;

public sealed partial class ExamQuestionsView : Page
{
    public ExamAnalysis Analysis { get; set; }
    public ExamQuestionsView()
    {
        InitializeComponent();
        Analysis = new();
    }
    protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        Analysis.ExamHeader = (ExamsPrimary)e.Parameter;
        this.DataContext = Analysis;
    }
    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        load();
    }
    private async void load()
    {

        Analysis.ThisExamQuestions = await ExamQuestionsHelper.GetExamQuestions(Analysis.ExamHeader!.id);
        await JoinedStudentsHelper.GetJoinedStudents();
        await AnsweersHelper.GetAnsweers();
        var joinedStudents = JoinedStudentsHelper.joinedStudents.Where(x => x.ExamId == Analysis.ExamHeader.id);
        var answers = AnsweersHelper.answeers.Where(x => Analysis.ThisExamQuestions.Any(q => q.id == x.QuestionId) && x.Mark > 0);

        Analysis.StudentsResult.Clear();
        var results = answers
    .GroupBy(x => x.StudentId)
    .Select(g => new StudentResult
    {
        StudentId = g.Key,
        TotalMark = (byte)g.Sum(x => x.Mark),
        // Calculate pass status immediately
        isPassed = g.Sum(x => x.Mark) >= (Analysis.TotalMark / 2.0)
    });
        foreach (var res in results)
        {
            Analysis.StudentsResult.Add(res);
        }
        Analysis.JoinedStudentsNo = joinedStudents.Count();
        Analysis.UnAttended = joinedStudents.Where(x => x.JoinedDateTime is null).Count();
        Analysis.Attended = joinedStudents.Where(x => x.JoinedDateTime is not null).Count();
        Analysis.PassedStudents = Analysis.StudentsResult.Where(x => x.isPassed).Count();
        Analysis.FailedStudents = Analysis.JoinedStudentsNo - Analysis.PassedStudents;
    }
    private async void DeleteQuestionBttn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bttn = sender as Button;
        if (bttn!.Tag is not int QuestionId)
            return;
        var dialog = new ContentDialog()
        {
            Title = "Are You Sure About Deleting This Question?",
            Content = "You Can Not Undo This Action",
            PrimaryButtonText = "Yes, Proceed",
            SecondaryButtonText = "No, Cancel",
            XamlRoot = this.XamlRoot
        };
        dialog.PrimaryButtonClick += async (s, e) =>
        {
            bool isDelete = await ExamQuestionsHelper.DeleteExamQuestion(QuestionId);
            if (isDelete)
                new ToastContentBuilder().AddText("Success").AddText("Exam Questions Removed").Show();
            else
                new ToastContentBuilder().AddText("Failure").AddText("Question Couldn't Be deleted it might be answeered").Show();
            var model = Analysis.ThisExamQuestions?.First(x => x.id == QuestionId);
            if (model != null)
                Analysis.ThisExamQuestions?.Remove(model);
            load();
        };
        await dialog.ShowAsync();
    }
    private async void UpdateQuestionBttn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bttn = sender as Button;
        if (bttn!.Tag is not int QuestionId)
            return;

        var model = Analysis.ThisExamQuestions.First(x => x.id == QuestionId);
        var dialog = new ContentDialog()
        {
            Title = "Updating Question",
            Content = new UpdateQuestion(model),
            PrimaryButtonText = "Update",
            SecondaryButtonText = "Cancel",
            XamlRoot = this.XamlRoot
        };
        dialog.PrimaryButtonClick += async (s, e) =>
        {
            bool isUpdated = await ExamQuestionsHelper.UpdateExamQuestion(model);
            if (isUpdated)
            {
                load();
            }
        };
        await dialog.ShowAsync();
    }
    private void RefreshBttn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        load();
    }
    private async void RemoveExam_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var dialog = new ContentDialog()
        {
            Title = "Are You Sure Deleting This Teacher and its Exams?",
            Content = "You Can't Undo This Action",
            XamlRoot = this.XamlRoot,
            PrimaryButtonText = "Yes, Delete",
            SecondaryButtonText = "No, Cancel"

        };
        dialog.PrimaryButtonClick +=
            async (s, e) =>
            {
                await ExamPrimaryHelper.DeleteExamPrimary(Analysis.ExamHeader!.id);
            };

        await dialog.ShowAsync();
        MainWindow.instance.ContentFrame.GoBack();
    }
    private async void AddQuestion_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var uploader = new AddQuestion(Analysis.ExamHeader!.id);

        var dialog = new ContentDialog()
        {
            Title = "Adding New Question",
            Content = uploader,
            PrimaryButtonText = "Add",
            SecondaryButtonText = "Cancel",
            XamlRoot = this.XamlRoot,
        };
        dialog.PrimaryButtonClick += async (s, e) =>
        {
            bool isAdded = await ExamQuestionsHelper.InsertExamQuestion(uploader.question);
            if (isAdded)
            {
                load();
            }
            else
            {
                e.Cancel = true;
            }
        };
        await dialog.ShowAsync();
    }
    private async void JoiningStudents_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var joiner = new JoiningStudents(Analysis.ExamHeader!.id);
        var dialog = new ContentDialog()
        {
            Title = "Setting This Exam for a Class",
            Content = joiner,
            PrimaryButtonText = "Set",
            SecondaryButtonText = "Cancel",
            XamlRoot = this.XamlRoot,
        };
        dialog.PrimaryButtonClick += async (s, e) =>
        {
            foreach (var item in joiner.StudentIds)
            {
                var model = new JoinedStudents
                {
                    StudentId = item,
                    ExamId = Analysis.ExamHeader.id
                };
                await JoinedStudentsHelper.InsertJoinedStudent(model);
            }
            load();
        };
        await dialog.ShowAsync();
    }
    private async void ExamResult_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var dialog = new ContentDialog()
        {
            Title = "Exam Full Result View",
            Content = new ExamsResultView(Analysis.ExamHeader!),
            SecondaryButtonText = "Close",
            IsPrimaryButtonEnabled = false,
            XamlRoot = this.XamlRoot,
        };
        await dialog.ShowAsync();
    }


}

