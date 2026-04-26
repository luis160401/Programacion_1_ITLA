using Microsoft.Data.SqlClient;
using BirthdayTracker.Models;

namespace BirthdayTracker.DataAccess
{
    public class NoteRepository
    {
        public int Insert(BirthdayNote note)
        {
            const string sql = @"
                INSERT INTO BirthdayNote (ContactId, NoteText, CreatedAt)
                VALUES (@ContactId, @NoteText, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ContactId", note.ContactId);
            cmd.Parameters.AddWithValue("@NoteText",  note.NoteText);
            cmd.Parameters.AddWithValue("@CreatedAt", note.CreatedAt);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<BirthdayNote> GetByContact(int contactId)
        {
            const string sql = "SELECT * FROM BirthdayNote WHERE ContactId = @Id ORDER BY CreatedAt DESC";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", contactId);

            var list = new List<BirthdayNote>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new BirthdayNote
                {
                    Id        = reader.GetInt32(reader.GetOrdinal("NoteId")),
                    ContactId = reader.GetInt32(reader.GetOrdinal("ContactId")),
                    NoteText  = reader.GetString(reader.GetOrdinal("NoteText")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                });
            }
            return list;
        }

        public bool Delete(int noteId)
        {
            const string sql = "DELETE FROM BirthdayNote WHERE NoteId = @Id";
            using var conn = DatabaseConfig.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", noteId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
