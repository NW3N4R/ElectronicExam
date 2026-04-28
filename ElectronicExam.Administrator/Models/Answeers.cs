namespace ElectronicExam.Administrator.Models
{
    public class Answeers
    {
        public int id { get; set; }

        public int StudentId { get; set; }

        public int QuestionId { get; set; }

        public string SelectedAnsweer { get; set; } = "";

        public byte Mark { get; set; }
    }
}
