using Examiner.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examiner.Helpers
{
    public static class JoinedStudentsHelper
    {
        public static async Task<List<JoinedStudents>?> GetJoinedStudents(int id)
        {
            List<JoinedStudents> joinedExams = new();
            using var cmd = new SqlCommand("select * from joinedstudents where studentid = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    joinedExams.Add(new JoinedStudents
                    {
                        id = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        ExamId = reader.GetInt32(2),
                        JoinedDateTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    });
                }
                return joinedExams;
            }
            return null;
        }
        public static async Task joinStudent(int stuId, int examId)
        {
            using var cmd = new SqlCommand("update joinedStudents set " +
                  "dateTimeJoined = @dt where studentId = @stuId and examId = @examId", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@examId", examId);
            cmd.Parameters.AddWithValue("@stuId", stuId);
            cmd.Parameters.AddWithValue("@dt", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
