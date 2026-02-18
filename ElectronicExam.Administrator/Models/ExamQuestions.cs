using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicExam.Administrator.Models
{
    public class ExamQuestions : ObservableObject
    {
        public int id { get; set; }

        public string Question { get; set; } = "";

        public string Explaining { get; set; } = "";

        public string CorrectAnsweer { get; set; } = "A";

        public string AnsA { get; set; } = "";

        public string AnsB { get; set; } = "";

        public string AnsC { get; set; } = "";

        public string AnsD { get; set; } = "";

        public byte Mark { get; set; }

        public int ExamId { get; set; }

    }
}
