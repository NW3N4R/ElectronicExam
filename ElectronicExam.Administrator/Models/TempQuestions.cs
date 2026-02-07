using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace ElectronicExam.Administrator.Models
{
    public class TempQuestionsList : ObservableObject
    {
        public ObservableCollection<TempQuestions> Questions
        {
            get => field;
            set => SetProperty(ref field, value);

        } = new();

        public string[] Answeers = ["A", "B", "C", "D"];
    }

    public class TempQuestions : ExamQuestions
    {
        public bool isExpanded
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public int QNo
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 0;

    }
}
