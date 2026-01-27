using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicExam.Administrator.Models
{
    public class Students : ObservableObject
    {
        public int id { get; set; }

        public string StudentName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        }

        public string Code
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        } = "";

        public string ClassName
        {
            get => field ?? default!;
            set => SetProperty(ref field, value);
        } = "";

        public int Grade
        {
            get => field;
            set => SetProperty(ref field, value);
        }

    }
}
