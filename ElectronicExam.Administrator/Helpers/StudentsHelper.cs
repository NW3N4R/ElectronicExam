using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
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
        public static async Task<bool> InsertStudent(Students student)
        {
            if (!ValidatStudent(student))
            {
                return false;
            }

            const string sql = @"INSERT INTO students (FirstName, MiddleName, LastName, Phone, Email, Gender, Stage, stuGroup, Code)
                         VALUES (@fName, @mName, @lName, @phone, @email, @gender, @stage, @group, @code)";
            string code = !string.IsNullOrEmpty(student.Code) ?
                            student.Code : Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@fName", student.FirstName);
            cmd.Parameters.AddWithValue("@mName", student.MiddleName);
            cmd.Parameters.AddWithValue("@lName", student.LastName);
            cmd.Parameters.AddWithValue("@phone", student.Phone);
            cmd.Parameters.AddWithValue("@email", student.Email);
            cmd.Parameters.AddWithValue("@gender", student.Gender);
            cmd.Parameters.AddWithValue("@stage", student.Stage);
            cmd.Parameters.AddWithValue("@group", student.Group);
            cmd.Parameters.AddWithValue("@code", code);

            await cmd.ExecuteNonQueryAsync();
            await GetStudents();
            new ToastContentBuilder().AddText("Success").AddText("Student Inserted").Show();
            return true;
        }
        public static async Task<bool> UpdateStudent(Students student)
        {
            if (!ValidatStudent(student))
            {
                return false;
            }

            const string sql = @"UPDATE students SET 
                        FirstName = @fName, MiddleName = @mName, LastName = @lName, 
                        Phone = @phone, Email = @email, Gender = @gender, 
                        Stage = @stage, stuGroup = @group WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@fName", student.FirstName);
            cmd.Parameters.AddWithValue("@mName", student.MiddleName);
            cmd.Parameters.AddWithValue("@lName", student.LastName);
            cmd.Parameters.AddWithValue("@phone", student.Phone);
            cmd.Parameters.AddWithValue("@email", student.Email);
            cmd.Parameters.AddWithValue("@gender", student.Gender);
            cmd.Parameters.AddWithValue("@stage", student.Stage);
            cmd.Parameters.AddWithValue("@group", student.Group);
            cmd.Parameters.AddWithValue("@id", student.id);

            await cmd.ExecuteNonQueryAsync();
            await GetStudents();

            new ToastContentBuilder().AddText("Success").AddText("Student Updated").Show();
            return true;
        }
        public static async Task DeleteStudent(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM students WHERE id = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            await GetStudents();
            new ToastContentBuilder().AddText("Success").AddText("Student Deleted").Show();
        }
        public static bool ValidatStudent(Students student)
        {
            var main = MainWindow.instance;
            string? error = null;
            if (student.FirstName.IsNullOrEmpty() || !student.FirstName.All(char.IsLetter))
                error = "First name most not be empty or\ncontain any non-letter chars";

            else if (student.MiddleName.IsNullOrEmpty() || !student.MiddleName.All(char.IsLetter))
                error = "Second name most not be empty or\ncontain any non-letter chars";

            else if (student.LastName.IsNullOrEmpty() || !student.LastName.All(char.IsLetter))
                error = "Last name most not be empty or\ncontain any non-letter chars";

            else if (student.Phone.IsNullOrEmpty() || student.Phone.Length < 11)
                error = "Invalid Phone Number";

            else if (!student.Email.IsNullOrEmpty() && (!student.Email.Contains("@")))
                error = "Email Most be Empty or Real";
            if (error != null)
            {
                main.ShowInfo("Validation Error", error, InfoBarSeverity.Error);
                return false;
            }
            return true;
        }
    }
}
