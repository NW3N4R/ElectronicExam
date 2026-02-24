using Examiner.Models;

using Microsoft.Data.SqlClient;

using System.Threading.Tasks;
namespace Examiner.Helpers
{
    public static class StudentsHelper
    {
        public static async Task<Students?> GetStudents(string code)
        {
            using var cmd = new SqlCommand("select * from students where code = @code", ConnectionHelper.connection);
            cmd.Parameters.AddWithValue("@code", code);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Students
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
                };
            }

            return null;
        }
    }
}
