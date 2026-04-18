using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Updaters;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;

using System;
using System.Diagnostics;
using System.Linq;

namespace ElectronicExam.Administrator.Views
{
    public sealed partial class StudentsView : Page
    {
        public static StudentsView Current = new();
        public StudentsView()
        {
            InitializeComponent();
            myListView.ItemsSource = StudentsHelper.students;
            Current = this;
        }

        private async void DeleteStudent_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                var item = sender as Button;
                if (item?.Tag is not Int32 tag)
                    return;
                var dialog = new ContentDialog()
                {
                    Title = "Are You Sure Deleting This Student?",
                    Content = "You Can't Undo This Action",
                    PrimaryButtonText = "Yes, Delete",
                    SecondaryButtonText = "No, Cancel",
                    XamlRoot = this.XamlRoot,
                    

                };
                dialog.PrimaryButtonClick +=
                    async (ContentDialog sender, ContentDialogButtonClickEventArgs args) =>
                {
                    await StudentsHelper.DeleteStudent(tag);
                    myListView.ItemsSource = null;
                    myListView.ItemsSource = StudentsHelper.students;
                };

                await dialog.ShowAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        private void UpdateStudent_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var item = sender as Button;
            if (item?.Tag is not Int32 tag)
            {
                Debug.WriteLine("Tag was Not Int32");
                if (item != null)
                {
                    Debug.WriteLine(item.Tag.GetType());
                }
                return;
            }
            var flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.RightEdgeAlignedTop;
            flyout.Content = new UpdateStudent(StudentsHelper.students.First(x => x.id == tag));
            flyout.ShowAt(item);
        }

        private void SearchTXT_TextChanged(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (sender.Text == "")
            {
                myListView.ItemsSource = StudentsHelper.students;
                return;
            }

            string text = sender.Text.Trim().ToLower();
            var rows = StudentsHelper.students.Where(x =>
                          x.Group.Trim().ToLower().StartsWith(text) ||
                          x.Code.Trim().ToLower().StartsWith(text) ||
                          x.FirstName.Trim().ToLower().StartsWith(text));

            myListView.ItemsSource = rows;

        }

        private void ViewExams_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var bttn = sender as Button;
            if (bttn?.Tag is not Int32 id)
                throw new Exception("tag was null");

            var student = StudentsHelper.students.First(x => x.id == id);

            MainWindow.instance.ContentFrame.Navigate(typeof(StudentExamsView), student, new SuppressNavigationTransitionInfo());
        }
    }
}
