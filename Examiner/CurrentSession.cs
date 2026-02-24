using CommunityToolkit.Mvvm.ComponentModel;

using Examiner.Models;

using System.Collections.ObjectModel;

namespace Examiner
{
    public class CurrentSession : ObservableObject
    {
        public Students? LoggedStudent { get; set; }
        public ExamsPrimary? ExamHeader { get; set; }
        public ObservableCollection<ExamQuestions>? ExamQuestions
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
