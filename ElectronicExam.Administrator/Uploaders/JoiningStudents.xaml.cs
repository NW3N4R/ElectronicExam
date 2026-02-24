using ElectronicExam.Administrator.Helpers;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Linq;

namespace ElectronicExam.Administrator.Uploaders
{
    public sealed partial class JoiningStudents : Page
    {
        int examId;
        public List<int> StudentIds = new();
        public JoiningStudents(int _examId)
        {
            InitializeComponent();
            examId = _examId;
        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await StudentsHelper.GetStudents();

        }

        private void StageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initiate();
        }

        void initiate()
        {
            var students = StudentsHelper.students;
            if (ClassList == null) return;

            if (StageList.SelectedIndex < 0)
                StageList.SelectedIndex = 0;
            if (int.Parse(StageList.SelectedItem.ToString()!) >= 4)
            {
                ClassList.ItemsSource = students.Where(x => x.DepGroups.Contains(x.Group))
                      .Select(x => x.Group).Distinct();
            }
            else
            {
                ClassList.ItemsSource = students.Where(x => x.LetterGroups.Contains(x.Group))
                        .Select(x => x.Group).Distinct();
            }
            ClassList.SelectedIndex = 0;

        }

        private void ClassList_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            initiate();
        }

        private void ClassList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassList.SelectedItem == null)
                return;
            var students = StudentsHelper.students;
            StudentIds = students.Where(x => x.Stage.ToString() == StageList.SelectedItem.ToString()
                            && x.Group == ClassList.SelectedItem.ToString()).Select(x => x.id).ToList();
            SelectedStudentsCount.Text = $"Selected Students No: {StudentIds.Count()}";
        }
    }
}
