namespace ElectronicExam.Administrator.Models
{
    public class ExamQuestions
    {
        public int id { get; set; }

        public string Question { get; set; } = "";

        public string Explaining { get; set; } = "";

        public string CorrectAnsweer { get; set; } = "";

        public string AnsA { get; set; } = "";

        public string AnsB { get; set; } = "";

        public string AnsC { get; set; } = "";

        public string AnsD { get; set; } = "";

        public int Mark { get; set; }

        public int ExamId { get; set; }

    }
}
