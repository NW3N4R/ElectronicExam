using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicExam.Administrator.Models
{
    public class HomeModel : ObservableObject
    {
        public int StudentsNo
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public int TeachersNo
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public int ExamsNo
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

        public string? RateOfSuccess
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public string RateOfFail
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
