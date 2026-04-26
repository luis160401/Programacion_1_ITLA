using Microsoft.Data.SqlClient;

namespace BirthdayTracker.DataAccess
{
    // Administra la cadena de conexión a SQL Server Express.
    // La instancia por defecto de SQL Express es .\SQLEXPRESS
    public static class DatabaseConfig
    {
        private static readonly string _connectionString =
            "Server=.\\SQLEXPRESS;Database=BirthdayTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Verifica que la conexión a la base de datos funcione correctamente
        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
