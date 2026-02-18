using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Updaters;
using ElectronicExam.Administrator.Uploaders;

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace ElectronicExam.Administrator.Views;

public sealed partial class ExamQuestionsView : Page
{
    public ExamsPrimary? header { get; set; }
    public ObservableCollection<ExamQuestions> tempQuestions { get; set; } = new();
    public int TotalMark { get; set; }
    public int TotalQuestions { get; set; }
    public ExamQuestionsView()
    {
        InitializeComponent();
    }
    protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        header = (ExamsPrimary)e.Parameter;
    }
    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        load();
    }
    private async void load()
    {
        if (header == null)
            return;
        tempQuestions = await ExamQuestionsHelper.GetExamQuestions(header.id);
        TotalMark = tempQuestions.Sum(x => x.Mark);
        TotalQuestions = tempQuestions.Count;
        this.DataContext = null;
        this.DataContext = this;
    }
    private async void DeleteQuestionBttn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bttn = sender as AppBarButton;
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
            await ExamQuestionsHelper.DeleteExamQuestion(QuestionId);
            new ToastContentBuilder().AddText("Success").AddText("Exam Questions Removed").Show();
            var model = tempQuestions.First(x => x.id == QuestionId);
            tempQuestions.Remove(model);
            load();
        };
        await dialog.ShowAsync();
    }
    private async void UpdateQuestionBttn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bttn = sender as AppBarButton;
        if (bttn!.Tag is not int QuestionId)
            return;

        var model = tempQuestions.First(x => x.id == QuestionId);
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
                await ExamPrimaryHelper.DeleteExamPrimary(header!.id);
            };

        await dialog.ShowAsync();
        MainWindow.instance.ContentFrame.GoBack();
    }
    private async void AddQuestion_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var uploader = new AddQuestion(header!.id);

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
}
