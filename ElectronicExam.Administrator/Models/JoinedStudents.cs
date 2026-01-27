using System;

namespace ElectronicExam.Administrator.Models
{
    public class JoinedStudents
    {
        public int id { get; set; }

        public int StudentId { get; set; }

        public int ExamId { get; set; }

        public DateTime JoinedDateTime { get; set; }
    }
}
