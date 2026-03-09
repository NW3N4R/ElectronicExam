using ElectronicExam.Administrator.Models;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicExam.Administrator.Helpers
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
                        Mark = reader.GetInt32(4),
                    });
                }
            }
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
