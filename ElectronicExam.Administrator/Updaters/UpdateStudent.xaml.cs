using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml.Controls;


namespace ElectronicExam.Administrator.Updaters
{
    public sealed partial class UpdateStudent : Page
    {
        private Students student;
        public UpdateStudent(Students _student)
        {
            InitializeComponent();
            student = _student;
            this.DataContext = student;
        }

        private async void UpdateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await StudentsHelper.UpdateStudent(student);
            StudentsView.Current.myListView.ItemsSource = null;
            StudentsView.Current.myListView.ItemsSource = StudentsHelper.students;

        }
    }
}
