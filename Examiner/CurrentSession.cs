using CommunityToolkit.Mvvm.ComponentModel;

using Examiner.Models;

using System.Collections.ObjectModel;

namespace Examiner
{
    public class CurrentSession : ObservableObject
    {
        public Students? LoggedStudent { get; set; }
        public ExamsPrimary? ExamHeader { get; set; }
        public double Duration
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 1;
        public double PassedMinutes
        {
            get => field;
            set => SetProperty(ref field, value);
        } = 0;
        public ObservableCollection<ExamQuestions>? ExamQuestions
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
