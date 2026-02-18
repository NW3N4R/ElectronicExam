using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Xaml.Controls;

using System.Diagnostics;
using System.Linq;
namespace ElectronicExam.Administrator.Uploaders;

public sealed partial class NewExamQuestion : Page
{
    TempQuestionsList listWrapper = new();
    public NewExamQuestion()
    {
        InitializeComponent();
    }

    async void Load()
    {
        await ExamPrimaryHelper.GetExamsPrimary();
        SelectExamHeader.ItemsSource = ExamPrimaryHelper.exams;
        QuestionsView.ItemsSource = listWrapper.Questions;
        addNewQuestion();
    }

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Load();
    }

    private void SelectExamHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectExamHeader.SelectedValue == null)
            return;
        var ex = ExamPrimaryHelper.exams.FirstOrDefault(x => x.id.ToString() == SelectExamHeader.SelectedValue.ToString());
        ExamTitle.Text = ex?.Title;
        ExamSubject.Text = ex?.SubjectName;
        ExamTeacher.Text = ex?.TeacherName;
    }

    private void AddQuestion_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        addNewQuestion();
    }

    void addNewQuestion()
    {
        var newQuestion = new TempQuestions();
        listWrapper.Questions.Add(newQuestion);
        listWrapper.Questions.Last().QNo = listWrapper.Questions.Count;
    }

    private void Expander_Expanding(Expander sender, ExpanderExpandingEventArgs args)
    {

        foreach (var item in listWrapper.Questions)
        {
            if (sender.DataContext != item)
            {
                Debug.WriteLine("Data Context Did not Match");
                item.isExpanded = false;
            }
        }
    }

    private void RemoveQuestion_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button?.DataContext is not TempQuestions q)
            return;
        listWrapper.Questions.Remove(q);
        foreach (var question in listWrapper.Questions)
        {
            question.QNo = listWrapper.Questions.IndexOf(question) + 1;
        }
    }

    private async void SaveQuestions_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (var q in listWrapper.Questions.ToList())
        {
            var model = new ExamQuestions()
            {
                Question = q.Question,
                Explaining = q.Explaining,
                AnsA = q.AnsA,
                AnsB = q.AnsB,
                AnsC = q.AnsC,
                AnsD = q.AnsD,
                Mark = q.Mark,
                CorrectAnsweer = q.CorrectAnsweer,
                ExamId = int.Parse(SelectExamHeader.SelectedValue.ToString()!),
            };
            bool isInserted = await ExamQuestionsHelper.InsertExamQuestion(model);

            if (isInserted)
                listWrapper.Questions.Remove(q);

        }
        if (listWrapper.Questions.Count == 0)
            new ToastContentBuilder().AddText("Success").AddText("Exam Questions Inserted").Show();
        else
            new ToastContentBuilder().AddText("Failure").AddText("One of The Question Failed To Be Inserted").Show();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cm = sender as ComboBox;
        if (cm != null)
        {
            var model = cm.DataContext as TempQuestions;
            if (model != null)
            {
                model.CorrectAnsweer = cm.SelectedItem.ToString() ?? "-";
            }
        }
    }
}
