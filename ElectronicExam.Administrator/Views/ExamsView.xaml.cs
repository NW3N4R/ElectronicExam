using ElectronicExam.Administrator.Helpers;
using ElectronicExam.Administrator.Updaters;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using System;
using System.Linq;
namespace ElectronicExam.Administrator.Views
{
    public sealed partial class ExamsView : Page
    {
        public static ExamsView Current = new();

        public ExamsView()
        {
            InitializeComponent();
            Current = this;
            Reload();

        }

        private void TeachersFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedItem = TeachersFilterCombo.SelectedItem?.ToString();
            int selectedIndex = TeachersFilterCombo.SelectedIndex;
            SubjectsFilterCombo.SelectedIndex = 0;
            mylist.ItemsSource = ExamPrimaryHelper.exams.Where(x => x.TeacherName == selectedItem || selectedIndex == 0);
        }

        private void SubjectsFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedItem = SubjectsFilterCombo.SelectedItem?.ToString();
            int selectedIndex = SubjectsFilterCombo.SelectedIndex;
            TeachersFilterCombo.SelectedIndex = 0;
            mylist.ItemsSource = ExamPrimaryHelper.exams.Where(x => x.SubjectName == selectedItem || selectedIndex == 0);
        }

        private void FilterByDate_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            mylist.ItemsSource = ExamPrimaryHelper.exams.Where(x => x.ExamDateTime == FilterByDate.SelectedDate?.Date || FilterByDate.SelectedDate == null);
        }

        private void CleanFilters_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Reload();
        }

        public void Reload()
        {
            mylist.ItemsSource = null;
            mylist.ItemsSource = ExamPrimaryHelper.exams;
            var teachers = ExamPrimaryHelper.exams.Select(x => x.TeacherName).Distinct().ToList();
            teachers.Insert(0, "Filter By Teachers");
            TeachersFilterCombo.ItemsSource = teachers;
            TeachersFilterCombo.SelectedIndex = 0;


            var subjects = ExamPrimaryHelper.exams.Select(x => x.SubjectName).Distinct().ToList();
            subjects.Insert(0, "Filter By Subjects");
            SubjectsFilterCombo.ItemsSource = subjects;
            SubjectsFilterCombo.SelectedIndex = 0;

            FilterByDate.SelectedDate = null;
        }

        private async void DeleteTeacher_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var item = sender as Button;
            if (item?.Tag is not Int32 tag)
                return;
            var dialog = new ContentDialog()
            {
                Title = "Are You Sure Deleting This Teacher and its Exams?",
                Content = "You Can't Undo This Action",
                XamlRoot = this.XamlRoot,
                PrimaryButtonText = "Yes, Delete",
                SecondaryButtonText = "No, Cancel"

            };
            dialog.PrimaryButtonClick +=
                async (s, e) =>
                {
                    await ExamPrimaryHelper.DeleteExamPrimary(tag);
                    Reload();
                };

            await dialog.ShowAsync();
        }

        private void UpdateTeacher_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is not int tag)
                return;

            var Exam = ExamPrimaryHelper.exams.First(x => x.id == tag);
            UpdateExam ue = new UpdateExam(Exam);
            Flyout fl = new Flyout();
            fl.Placement = Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Left;
            fl.Content = ue;
            fl.ShowAt(button);
        }

        private void ViewQuestions_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var bttn = sender as Button;
            if (bttn?.Tag is not int Id)
                return;

            var model = ExamPrimaryHelper.exams.First(x => x.id == Id);
            MainWindow.instance.ContentFrame.Navigate(typeof(ExamQuestionsView), model, new SuppressNavigationTransitionInfo());
        }
    }
}
