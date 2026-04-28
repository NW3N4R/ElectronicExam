using Examiner.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examiner.Helpers
{
    public static class ExamPrimaryHelper
    {
        public static readonly List<ExamsPrimary> exams = new List<ExamsPrimary>();

        public static async Task GetExamsPrimary()
        {
            using var cmd = new SqlCommand("SELECT * FROM ExamsPrimary", ConnectionHelper.connection);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                exams.Clear();
                while (await reader.ReadAsync())
                {
                    exams.Add(new ExamsPrimary
                    {
                        id = reader.GetInt32(0),
                        TeacherName = reader.GetString(1),
                        SubjectName = reader.GetString(2),
                        Title = reader.GetString(3),
                        ExamDateTime = new DateTimeOffset(reader.GetDateTime(4)),
                        ExamDuration = reader.GetDecimal(5),
                    });
                }
            }
        }
    }
}
