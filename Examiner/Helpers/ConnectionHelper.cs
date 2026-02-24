using Microsoft.Data.SqlClient;

using System.Data;
using System.Threading.Tasks;

namespace Examiner.Helpers
{
    public static class ConnectionHelper
    {
        private static readonly string connectionString = "Data Source=Nwenar;Initial Catalog=e-Exam;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        public static SqlConnection connection = new SqlConnection(connectionString);
        public static async Task OpenConnectionAsync()
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
            {
                await connection.OpenAsync();
            }
        }

        public static void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
