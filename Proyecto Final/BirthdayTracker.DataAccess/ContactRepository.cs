using Microsoft.Data.SqlClient;
using BirthdayTracker.Models;

namespace BirthdayTracker.DataAccess
{
    // Repositorio de acceso a datos para la entidad Contact.
    // Implementa operaciones CRUD usando ADO.NET puro.
    public class ContactRepository
    {
        // ── CREATE ────────────────────────────────────────────────────
        public int Insert(Contact contact)
        {
            const string sql = @"
                INSERT INTO Contact (FirstName, LastName, Nickname, BirthDate, Phone, Email, CreatedAt)
                VALUES (@FirstName, @LastName, @Nickname, @BirthDate, @Phone, @Email, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FirstName",  contact.FirstName);
            cmd.Parameters.AddWithValue("@LastName",   contact.LastName);
            cmd.Parameters.AddWithValue("@Nickname",   (object?)contact.Nickname  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BirthDate",  contact.BirthDate);
            cmd.Parameters.AddWithValue("@Phone",      (object?)contact.Phone     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email",      (object?)contact.Email     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt",  contact.CreatedAt);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ── READ ALL ──────────────────────────────────────────────────
        public List<Contact> GetAll()
        {
            const string sql = "SELECT * FROM Contact ORDER BY FirstName, LastName";
            return ExecuteQuery(sql);
        }

        // ── READ BY ID ────────────────────────────────────────────────
        public Contact? GetById(int id)
        {
            const string sql = "SELECT * FROM Contact WHERE ContactId = @Id";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) return MapContact(reader);
            return null;
        }

        // ── SEARCH BY NAME ────────────────────────────────────────────
        public List<Contact> SearchByName(string name)
        {
            const string sql = @"
                SELECT * FROM Contact
                WHERE FirstName LIKE @Name OR LastName LIKE @Name OR Nickname LIKE @Name
                ORDER BY FirstName";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", $"%{name}%");
            return ReadAll(cmd);
        }

        // ── SEARCH BY MONTH ───────────────────────────────────────────
        public List<Contact> GetByBirthMonth(int month)
        {
            const string sql = @"
                SELECT * FROM Contact
                WHERE MONTH(BirthDate) = @Month
                ORDER BY DAY(BirthDate)";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Month", month);
            return ReadAll(cmd);
        }

        // ── UPCOMING (next N days) ─────────────────────────────────────
        public List<Contact> GetUpcoming(int days = 30)
        {
            // Busca cumpleaños en los próximos N días (sin importar el año)
            const string sql = @"
                SELECT * FROM Contact
                WHERE (
                    DATEADD(YEAR, YEAR(GETDATE()) - YEAR(BirthDate), BirthDate) 
                    BETWEEN CAST(GETDATE() AS DATE) 
                    AND DATEADD(DAY, @Days, CAST(GETDATE() AS DATE))
                )
                OR (
                    DATEADD(YEAR, YEAR(GETDATE()) + 1 - YEAR(BirthDate), BirthDate)
                    BETWEEN CAST(GETDATE() AS DATE)
                    AND DATEADD(DAY, @Days, CAST(GETDATE() AS DATE))
                )
                ORDER BY MONTH(BirthDate), DAY(BirthDate)";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Days", days);
            return ReadAll(cmd);
        }

        // ── UPDATE ────────────────────────────────────────────────────
        public bool Update(Contact contact)
        {
            const string sql = @"
                UPDATE Contact
                SET FirstName = @FirstName,
                    LastName  = @LastName,
                    Nickname  = @Nickname,
                    BirthDate = @BirthDate,
                    Phone     = @Phone,
                    Email     = @Email
                WHERE ContactId = @Id";

            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id",        contact.Id);
            cmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
            cmd.Parameters.AddWithValue("@LastName",  contact.LastName);
            cmd.Parameters.AddWithValue("@Nickname",  (object?)contact.Nickname ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BirthDate", contact.BirthDate);
            cmd.Parameters.AddWithValue("@Phone",     (object?)contact.Phone    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email",     (object?)contact.Email    ?? DBNull.Value);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ── DELETE ────────────────────────────────────────────────────
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Contact WHERE ContactId = @Id";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ── Helpers privados ──────────────────────────────────────────
        private static List<Contact> ExecuteQuery(string sql)
        {
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            return ReadAll(cmd);
        }

        private static List<Contact> ReadAll(SqlCommand cmd)
        {
            var list = new List<Contact>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(MapContact(reader));
            return list;
        }

        private static Contact MapContact(SqlDataReader r) => new()
        {
            Id        = r.GetInt32(r.GetOrdinal("ContactId")),
            FirstName = r.GetString(r.GetOrdinal("FirstName")),
            LastName  = r.GetString(r.GetOrdinal("LastName")),
            Nickname  = r.IsDBNull(r.GetOrdinal("Nickname")) ? null : r.GetString(r.GetOrdinal("Nickname")),
            BirthDate = r.GetDateTime(r.GetOrdinal("BirthDate")),
            Phone     = r.IsDBNull(r.GetOrdinal("Phone")) ? null : r.GetString(r.GetOrdinal("Phone")),
            Email     = r.IsDBNull(r.GetOrdinal("Email")) ? null : r.GetString(r.GetOrdinal("Email")),
            CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt")),
        };
    }
}
