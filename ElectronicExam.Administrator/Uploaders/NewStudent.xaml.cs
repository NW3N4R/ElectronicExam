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
        }

        private async void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await StudentsHelper.InsertStudent(stu);
            StudentsView.Current.myListView.ItemsSource = null;
            StudentsView.Current.myListView.ItemsSource = StudentsHelper.students;
        }
    }
}
