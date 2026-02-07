using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
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
                        Mark = reader.GetInt32(8),
                        ExamId = reader.GetInt32(9)
                    });
                }
            }
        }

        public static async Task InsertExamQuestion(ExamQuestions q)
        {
            if (q == null) throw new ArgumentNullException(nameof(q));

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
        }

        public static async Task UpdateExamQuestion(ExamQuestions q)
        {
            if (q == null) throw new ArgumentNullException(nameof(q));

            const string sql = @"UPDATE ExamQuestions SET Question = @question,
                                 Explaining = @explaining, CorrectAnsweer = @correct,
                                 AnsA = @a,
                                 AnsB = @b,
                                 AnsC = @c,
                                 AnsD = @d,
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
        }

        public static async Task DeleteExamQuestion(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid question id.", nameof(id));

            using var cmd = new SqlCommand("DELETE FROM ExamQuestions WHERE id = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            await GetExamQuestions();
        }
    }
}
