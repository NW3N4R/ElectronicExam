namespace Examiner.Models
{
    public class Answeers
    {
        public int id { get; set; }

        public int StudentId { get; set; }

        public int ExamId { get; set; }

        public int QuestionId { get; set; }

        public string SelectedAnsweer { get; set; } = "";

        public bool isCorrect { get; set; }
    }
}
