using Examiner.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Examiner.Helpers
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
    }
}
