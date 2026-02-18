using ElectronicExam.Administrator.Models;

using Microsoft.UI.Xaml.Controls;


namespace ElectronicExam.Administrator.Updaters;

public sealed partial class UpdateQuestion : Page
{
    ExamQuestions question;
    public UpdateQuestion(ExamQuestions _question)
    {
        InitializeComponent();
        question = _question;
        this.DataContext = question;
    }
}
