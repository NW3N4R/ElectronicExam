using System;

namespace Examiner.Models
{
    public class JoinedStudents
    {
        public int id { get; set; }

        public int StudentId { get; set; }

        public int ExamId { get; set; }

        public DateTime? JoinedDateTime { get; set; }
    }
}
