using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ElectronicExam.Administrator.Helpers
{
    public static class ExamQuestionsHelper
    {
        public static readonly List<ExamQuestions> questions = new List<ExamQuestions>();

        public static async Task GetExamQuestions()
        {
            using var cmd = new SqlCommand("SELECT * FROM ExamQuestions", ConnectionHelper.connection);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                questions.Clear();
                while (await reader.ReadAsync())
                {
                    questions.Add(new ExamQuestions
                    {
                        id = reader.GetInt32(0),
                        Question = reader.GetString(1),
                        Explaining = reader.GetString(2),
                        CorrectAnsweer = reader.GetString(3),
                        AnsA = reader.GetString(4),
                        AnsB = reader.GetString(5),
                        AnsC = reader.GetString(6),
                        AnsD = reader.GetString(7),
                        Mark = reader.GetByte(8),
                        ExamId = reader.GetInt32(9)
                    });
                }
            }
        }
        public static async Task<ObservableCollection<ExamQuestions>> GetExamQuestions(int examId)
        {
            ObservableCollection<ExamQuestions> Tempquestions = new();
            using var cmd = new SqlCommand("SELECT * FROM ExamQuestions where examId = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", examId);
            using var reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    Tempquestions.Add(new ExamQuestions
                    {
                        id = reader.GetInt32(0),
                        Question = reader.GetString(1),
                        Explaining = reader.GetString(2),
                        CorrectAnsweer = reader.GetString(3),
                        AnsA = reader.GetString(4),
                        AnsB = reader.GetString(5),
                        AnsC = reader.GetString(6),
                        AnsD = reader.GetString(7),
                        Mark = reader.GetByte(8),
                        ExamId = reader.GetInt32(9)
                    });
                }
            }
            return Tempquestions;
        }

        public static async Task<bool> InsertExamQuestion(ExamQuestions q)
        {
            if (!ValidatedQuestion(q)) return false;

            const string sql = @"INSERT INTO ExamQuestions 
                                 (Question, Explaining, CorrectAnsweer, Answeer_A, Answeer_B, Answeer_C, Answeer_D, Mark, ExamId)
                                 VALUES (@question, @explaining, @correct, @a, @b, @c, @d, @mark, @examId)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@question", q.Question);
            cmd.Parameters.AddWithValue("@explaining", q.Explaining);
            cmd.Parameters.AddWithValue("@correct", q.CorrectAnsweer);
            cmd.Parameters.AddWithValue("@a", q.AnsA);
            cmd.Parameters.AddWithValue("@b", q.AnsB);
            cmd.Parameters.AddWithValue("@c", q.AnsC);
            cmd.Parameters.AddWithValue("@d", q.AnsD);
            cmd.Parameters.AddWithValue("@mark", q.Mark);
            cmd.Parameters.AddWithValue("@examId", q.ExamId);

            await cmd.ExecuteNonQueryAsync();
            await GetExamQuestions();
            return true;
        }

        public static async Task<bool> UpdateExamQuestion(ExamQuestions q)
        {
            if (!ValidatedQuestion(q)) return false;

            const string sql = @"UPDATE ExamQuestions SET Question = @question,
                                 Explaining = @explaining, CorrectAnsweer = @correct,
                                 Answeer_A = @a,
                                 Answeer_B = @b,
                                 Answeer_C = @c,
                                 Answeer_D = @d,
                                 Mark = @mark,
                                 ExamId = @examId WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@question", q.Question);
            cmd.Parameters.AddWithValue("@explaining", q.Explaining);
            cmd.Parameters.AddWithValue("@correct", q.CorrectAnsweer);
            cmd.Parameters.AddWithValue("@a", q.AnsA);
            cmd.Parameters.AddWithValue("@b", q.AnsB);
            cmd.Parameters.AddWithValue("@c", q.AnsC);
            cmd.Parameters.AddWithValue("@d", q.AnsD);
            cmd.Parameters.AddWithValue("@mark", q.Mark);
            cmd.Parameters.AddWithValue("@examId", q.ExamId);
            cmd.Parameters.AddWithValue("@id", q.id);

            await cmd.ExecuteNonQueryAsync();
            await GetExamQuestions();
            return true;
        }

        public static async Task<bool> DeleteExamQuestion(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentException("Invalid question id.", nameof(id));

                using var cmd = new SqlCommand("DELETE FROM ExamQuestions WHERE id = @id", ConnectionHelper.connection);
                cmd.Parameters.AddWithValue("@id", id);
                await cmd.ExecuteNonQueryAsync();
                await GetExamQuestions();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidatedQuestion(ExamQuestions question)
        {
            var instance = MainWindow.instance;
            string? error = null;
            if (question.Question.IsNullOrEmpty())
                error = "Question most not be empty";

            else if (question.Explaining.IsNullOrEmpty())
                error = "Explaining most not be empty";

            else if (question.AnsA.IsNullOrEmpty())
                error = "Answeer A most not be empty";
            else if (question.AnsB.IsNullOrEmpty())
                error = "Answeer B most not be empty";
            else if (question.AnsC.IsNullOrEmpty())
                error = "Answeer C most not be empty";
            else if (question.AnsD.IsNullOrEmpty())
                error = "Answeer D most not be empty";
            else if (question.Mark == 0)
                error = "Question mark is required";
            if (error != null)
            {
                instance.ShowInfo("Validation Error", error, InfoBarSeverity.Error);
                return false;
            }
            return true;
        }
    }
}
