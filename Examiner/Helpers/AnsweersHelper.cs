using Examiner.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Threading.Tasks;

namespace Examiner.Helpers
{
    public static class AnsweersHelper
    {
        public static async Task InsertAnsweer(Answeers item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            const string sql = @"INSERT INTO answeers (studentId, questionId, SelectedAnsweers, Mark)
                                VALUES (@studentId, @questionId, @selected, @Mark)";

            using var cmd = new SqlCommand(sql, ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@studentId", item.StudentId);
            cmd.Parameters.AddWithValue("@questionId", item.QuestionId);
            cmd.Parameters.AddWithValue("@selected", item.SelectedAnsweer ?? string.Empty);
            cmd.Parameters.AddWithValue("@Mark", item.Mark);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
