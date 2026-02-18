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
            GroupCm.ItemsSource = student.Stage <= 3 ? student.LetterGroups : student.DepGroups;
            GroupCm.SelectedItem = student.Group;
        }

        private async void UpdateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            bool isUpdated = await StudentsHelper.UpdateStudent(student);
            if (isUpdated)
            {
                StudentsView.Current.myListView.ItemsSource = null;
                StudentsView.Current.myListView.ItemsSource = StudentsHelper.students;
            }

        }
        private void StageCm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupCm.ItemsSource = student.LetterGroups;
            if (int.Parse((StageCm.SelectedItem.ToString()!)) >= 4)
            {
                GroupCm.ItemsSource = student.DepGroups;
            }
        }
    }
}
