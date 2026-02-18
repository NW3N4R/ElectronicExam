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
                        FirstName = reader.GetString(1),
                        MiddleName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Email = reader.GetString(5),
                        Gender = reader.GetString(6),
                        Stage = reader.GetByte(7),
                        Group = reader.GetString(8),
                        Code = reader.GetString(9),
                    });
                }
            }
        }
        public static async Task InsertStudent(Students student)
        {
            const string sql = @"INSERT INTO students (FirstName, MiddleName, LastName, Phone, Email, Gender, Stage, stuGroup, Code)
                         VALUES (@fName, @mName, @lName, @phone, @email, @gender, @stage, @group, @code)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@fName", student.FirstName ?? string.Empty);
            cmd.Parameters.AddWithValue("@mName", student.MiddleName ?? string.Empty);
            cmd.Parameters.AddWithValue("@lName", student.LastName ?? string.Empty);
            cmd.Parameters.AddWithValue("@phone", student.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", student.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@gender", student.Gender ?? string.Empty);
            cmd.Parameters.AddWithValue("@stage", student.Stage);
            cmd.Parameters.AddWithValue("@group", student.Group ?? string.Empty);
            cmd.Parameters.AddWithValue("@code", Guid.NewGuid().ToString("N")[..6].ToUpperInvariant());

            await cmd.ExecuteNonQueryAsync();
            await GetStudents();
            new ToastContentBuilder().AddText("Success").AddText("Student Inserted").Show();
        }
        public static async Task UpdateStudent(Students student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));

            const string sql = @"UPDATE students SET 
                        FirstName = @fName, MiddleName = @mName, LastName = @lName, 
                        Phone = @phone, Email = @email, Gender = @gender, 
                        Stage = @stage, stuGroup = @group WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@fName", student.FirstName ?? string.Empty);
            cmd.Parameters.AddWithValue("@mName", student.MiddleName ?? string.Empty);
            cmd.Parameters.AddWithValue("@lName", student.LastName ?? string.Empty);
            cmd.Parameters.AddWithValue("@phone", student.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", student.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@gender", student.Gender ?? string.Empty);
            cmd.Parameters.AddWithValue("@stage", student.Stage);
            cmd.Parameters.AddWithValue("@group", student.Group ?? string.Empty);
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
