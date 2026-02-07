using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ElectronicExam.Administrator.Uploaders
{
    public sealed partial class NewExam : Page
    {
        ExamsPrimary exPrimary = new();
        public NewExam()
        {
            InitializeComponent();
            this.DataContext = exPrimary;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await ExamPrimaryHelper.InsertExamPrimary(exPrimary);
            ExamsView.Current.Reload();
        }
    }
}
