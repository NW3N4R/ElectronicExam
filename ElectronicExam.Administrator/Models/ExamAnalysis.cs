using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;
using System.Linq;

namespace ElectronicExam.Administrator.Models
{
    public class ExamAnalysis : ObservableObject
    {
        public int TotalMark
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int TotalQuestions
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int JoinedStudentsNo
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int UnAttended
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int Attended
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int PassedStudents
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public int FailedStudents
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public ExamsPrimary? ExamHeader { get; set; }
        public ObservableCollection<ExamQuestions>? ThisExamQuestions
        {
            get => field;
            set
            {
                SetProperty(ref field, value);
                if (value != null)
                {
                    TotalMark = value.Sum(x => x.Mark);
                    TotalQuestions = value.Count();
                }
            }
        }
        public ObservableCollection<StudentResult> StudentsResult
        {
            get => field;
            set => SetProperty(ref field, value);
        } = new();
    }

    public class StudentResult : ObservableObject
    {
        public int StudentId { get; set; }

        public byte TotalMark { get; set; }

        public bool isPassed { get; set; }
    }
}
