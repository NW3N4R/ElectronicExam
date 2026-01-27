using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;
using Microsoft.Toolkit.Uwp.Notifications;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicExam.Administrator.Helpers
{
    public static class StudentsHelper
    {
        public static readonly List<Students> students = new List<Students>();
        public static async Task GetStudents()
        {
            using var cmd = new SqlCommand("select * from students", ConnectionHelper.connection);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                students.Clear();
                while (await reader.ReadAsync())
                {
                    students.Add(new Students
                    {
                        id = reader.GetInt32(0),
                        StudentName = reader.GetString(1),
                        Code = reader.GetString(2),
                        ClassName = reader.GetString(3),
                        Grade = reader.GetInt32(4),
                    });
                }
            }
        }
        public static async Task InsertStudent(Students student)
        {
            const string sql = @"INSERT INTO students (SName, Code, ClassName, Grade)
                                                        VALUES (@name, @code, @class, @grade)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@name", student.StudentName ?? string.Empty);
            cmd.Parameters.AddWithValue("@code", Guid.NewGuid().ToString("N")[..6].ToUpperInvariant());
            cmd.Parameters.AddWithValue("@class", student.ClassName ?? string.Empty);
            cmd.Parameters.AddWithValue("@grade", student.Grade);

            await cmd.ExecuteNonQueryAsync();
            await GetStudents();
            new ToastContentBuilder().AddText("Success").AddText("Student Inserted").Show();
        }
        public static async Task UpdateStudent(Students student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));

            const string sql = @"UPDATE students SET 
                                    SName = @name, ClassName = @class, Grade = @grade WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@name", student.StudentName ?? string.Empty);
            cmd.Parameters.AddWithValue("@class", student.ClassName ?? string.Empty);
            cmd.Parameters.AddWithValue("@grade", student.Grade);
            cmd.Parameters.AddWithValue("@id", student.id);

            await cmd.ExecuteNonQueryAsync();
            await GetStudents();

            new ToastContentBuilder().AddText("Success").AddText("Student Updated").Show();
        }
        public static async Task DeleteStudent(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM students WHERE id = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            await GetStudents();
            new ToastContentBuilder().AddText("Success").AddText("Student Deleted").Show();
        }
    }
}
