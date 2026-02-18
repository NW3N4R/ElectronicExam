using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace ElectronicExam.Administrator.Updaters;

public sealed partial class UpdateExam : Page
{
    ExamsPrimary exPrimary;
    public UpdateExam(ExamsPrimary _exam)
    {
        InitializeComponent();
        exPrimary = _exam;
        this.DataContext = exPrimary;
    }
    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        bool isUpdated = await ExamPrimaryHelper.UpdateExamPrimary(exPrimary);
        if (isUpdated)
        {
            ExamsView.Current.Reload();
        }
    }
}
