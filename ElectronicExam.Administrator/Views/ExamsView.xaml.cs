using ElectronicExam.Administrator.Helpers;

using Microsoft.UI.Xaml.Controls;


namespace ElectronicExam.Administrator.Views
{
    public sealed partial class ExamsView : Page
    {
        public static ExamsView Current = new();
        public ExamsView()
        {
            InitializeComponent();
            mylist.ItemsSource = ExamPrimaryHelper.exams;
        }
    }
}
