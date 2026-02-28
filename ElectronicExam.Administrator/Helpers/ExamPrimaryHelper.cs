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
                        EDate = new DateTimeOffset(reader.GetDateTime(4)),
                        ETime = reader.GetTimeSpan(5)
                    });
                }
            }
        }

        public static async Task<bool> InsertExamPrimary(ExamsPrimary exam)
        {

            if (!ValidateExam(exam)) return false;

            const string sql = @" INSERT INTO ExamsPrimary (TeacherName, SubjectName, Title,EDate,ETime)
                                    VALUES (@teacher, @subject, @title,@EDate,@ETime)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@teacher", exam.TeacherName ?? string.Empty);
            cmd.Parameters.AddWithValue("@subject", exam.SubjectName ?? string.Empty);
            cmd.Parameters.AddWithValue("@title", exam.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("@EDate", exam.EDate);
            cmd.Parameters.AddWithValue("@ETime", exam.ETime);

            await cmd.ExecuteNonQueryAsync();
            await GetExamsPrimary();
            new ToastContentBuilder().AddText("Success").AddText("Exam Created").Show();
            return true;
        }

        public static async Task<bool> UpdateExamPrimary(ExamsPrimary exam)
        {
            if (!ValidateExam(exam)) return false;

            const string sql = @" UPDATE ExamsPrimary SET 
                                    TeacherName = @teacher, SubjectName = @subject,
                                    Title = @title,EDate = @EDate,ETime = @ETime WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@teacher", exam.TeacherName ?? string.Empty);
            cmd.Parameters.AddWithValue("@subject", exam.SubjectName ?? string.Empty);
            cmd.Parameters.AddWithValue("@title", exam.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("@EDate", exam.EDate);
            cmd.Parameters.AddWithValue("@ETime", exam.ETime);
            cmd.Parameters.AddWithValue("@id", exam.id);

            await cmd.ExecuteNonQueryAsync();
            await GetExamsPrimary();
            new ToastContentBuilder().AddText("Success").AddText("Exam Updated").Show();
            return true;
        }

        public static async Task DeleteExamPrimary(int id)
        {
            try
            {
                using var cmd = new SqlCommand("DELETE FROM ExamsPrimary WHERE id = @id", ConnectionHelper.connection);
                cmd.Parameters.AddWithValue("@id", id);

                await cmd.ExecuteNonQueryAsync();
                await GetExamsPrimary();
            }
            catch (SqlException)
            {
            }
        }

        public static bool ValidateExam(ExamsPrimary exam)
        {
            var instance = MainWindow.instance;
            string? error = null;

            TimeSpan startLimit = new TimeSpan(7, 40, 0);  // 7:40 AM
            TimeSpan endLimit = new TimeSpan(20, 0, 0);

            if (exam.TeacherName.IsNullOrEmpty() || !exam.TeacherName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                error = "Teacher Name most not be empty\nor contain any non-letter chars";
            else if (exam.SubjectName.IsNullOrEmpty())
                error = "Subject Name most not be empty";
            else if (exam.Title.IsNullOrEmpty())
                error = "Title most not be empty";
            else if (exam.EDate <= DateTime.Now)
                error = "Date Most not be empty or before today";
            else if (exam.ETime == default ||
                    (exam.ETime < startLimit || exam.ETime > endLimit))
                error = "Exam Time Most be between 7:40 AM and 08:00 PM";

            if (error != null)
            {
                instance.ShowInfo("Validation Error", error, InfoBarSeverity.Error);
                return false;
            }
            return true;
        }
    }
}
