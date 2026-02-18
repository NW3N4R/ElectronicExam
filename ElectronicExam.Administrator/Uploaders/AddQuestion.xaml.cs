using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;

namespace ElectronicExam.Administrator.Uploaders;

public sealed partial class AddQuestion : Page
{
    public ExamQuestions question;
    public AddQuestion(int id)
    {
        InitializeComponent();
        question = new ExamQuestions();
        question.ExamId = id;
        this.DataContext = question;
    }
}
