using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicExam.Administrator.Helpers
{
    public static class JoinedStudentsHelper
    {
        public static readonly List<JoinedStudents> joinedStudents = new List<JoinedStudents>();

        public static async Task GetJoinedStudents()
        {
            using var cmd = new SqlCommand("select * from joinedstudents", ConnectionHelper.connection);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                joinedStudents.Clear();
                while (await reader.ReadAsync())
                {
                    joinedStudents.Add(new JoinedStudents
                    {
                        id = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        ExamId = reader.GetInt32(2),
                        JoinedDateTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    });
                }
            }
        }

        public static async Task InsertJoinedStudent(JoinedStudents item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            const string sql = @"INSERT INTO joinedstudents (StudentId, ExamId,DateTimeJoined)
                                                        VALUES (@studentId, @examId,@DateTimeJoined)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@studentId", item.StudentId);
            cmd.Parameters.AddWithValue("@examId", item.ExamId);
            cmd.Parameters.AddWithValue("@DateTimeJoined", DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            await GetJoinedStudents();
        }

        public static async Task UpdateJoinedStudent(JoinedStudents item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            const string sql = @"UPDATE joinedstudents SET 
                                    StudentId = @studentId, ExamId = @examId, DateTimeJoined = @DateTimeJoined
                                    WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@studentId", item.StudentId);
            cmd.Parameters.AddWithValue("@examId", item.ExamId);
            cmd.Parameters.AddWithValue("@DateTimeJoined", item.JoinedDateTime);
            cmd.Parameters.AddWithValue("@id", item.id);

            await cmd.ExecuteNonQueryAsync();
            await GetJoinedStudents();
        }

        public static async Task DeleteJoinedStudent(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM joinedstudents WHERE id = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            await GetJoinedStudents();
        }
    }
}
