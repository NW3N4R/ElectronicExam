using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Models;
using ElectronicExam.Administrator.Views;

using Microsoft.UI.Xaml.Controls;


namespace ElectronicExam.Administrator.Uploaders
{
    public sealed partial class NewStudent : Page
    {
        private Students stu;

        public NewStudent()
        {
            InitializeComponent();
            stu = new();
            this.DataContext = stu;
            GroupCm.ItemsSource = stu.LetterGroups;
        }

        private async void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            bool isInserted = await StudentsHelper.InsertStudent(stu);
            if (isInserted)
            {
                StudentsView.Current.myListView.ItemsSource = null;
                StudentsView.Current.myListView.ItemsSource = StudentsHelper.students;
            }
        }

        private void StageCm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupCm.ItemsSource = stu.LetterGroups;
            if (int.Parse((StageCm.SelectedItem.ToString()!)) >= 4)
            {
                GroupCm.ItemsSource = stu.DepGroups;
            }
        }
    }
}
