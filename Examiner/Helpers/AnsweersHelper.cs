using Examiner.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examiner.Helpers
{
    public static class AnsweersHelper
    {
        public static readonly List<Answeers> answeers = new List<Answeers>();

        public static async Task GetAnsweers()
        {
            using var cmd = new SqlCommand("select * from answeers", ConnectionHelper.connection);
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                answeers.Clear();
                while (await reader.ReadAsync())
                {
                    answeers.Add(new Answeers
                    {
                        id = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        QuestionId = reader.GetInt32(2),
                        SelectedAnsweer = reader.GetString(3),
                        isCorrect = reader.GetBoolean(4),
                    });
                }
            }
        }

        public static async Task InsertAnsweer(Answeers item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            const string sql = @"INSERT INTO answeers (studentId, questionId, SelectedAnsweers, isCorrect)
                                VALUES (@studentId, @questionId, @selected, @isCorrect)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@studentId", item.StudentId);
            cmd.Parameters.AddWithValue("@questionId", item.QuestionId);
            cmd.Parameters.AddWithValue("@selected", item.SelectedAnsweer ?? string.Empty);
            cmd.Parameters.AddWithValue("@isCorrect", item.isCorrect);

            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task UpdateAnsweer(Answeers item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            const string sql = @" UPDATE answeers SET studentId = @studentId,
                                ExamId = @examId, questionId = @questionId,
                                SelectedAnsweers = @selected, isCorrect = @ WHERE id = @id";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@studentId", item.StudentId);
            cmd.Parameters.AddWithValue("@examId", item.ExamId);
            cmd.Parameters.AddWithValue("@questionId", item.QuestionId);
            cmd.Parameters.AddWithValue("@selected", item.SelectedAnsweer ?? string.Empty);
            cmd.Parameters.AddWithValue("@isCorrect", item.isCorrect);
            cmd.Parameters.AddWithValue("@id", item.id);

            await cmd.ExecuteNonQueryAsync();
            await GetAnsweers();
        }

        public static async Task DeleteAnsweer(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM answeers WHERE id = @id", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            await GetAnsweers();
        }
    }
}
