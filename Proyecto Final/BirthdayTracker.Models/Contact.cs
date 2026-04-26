namespace BirthdayTracker.Models
{
    // Entidad principal del sistema: representa un contacto con cumpleaños.
    public class Contact : BaseEntity
    {
        // ── Propiedades ───────────────────────────────────────────────
        public string FirstName  { get; set; } = string.Empty;
        public string LastName   { get; set; } = string.Empty;
        public string? Nickname  { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Phone     { get; set; }
        public string? Email     { get; set; }

        // ── Constructores (requisito POO) ─────────────────────────────

        // Constructor por defecto (necesario para Dapper / ADO.NET)
        public Contact() { }

        // Constructor principal con los datos obligatorios
        public Contact(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName  = lastName;
            BirthDate = birthDate;
        }

        // Constructor completo
        public Contact(string firstName, string lastName, DateTime birthDate,
                       string? nickname, string? phone, string? email)
            : this(firstName, lastName, birthDate)
        {
            Nickname = nickname;
            Phone    = phone;
            Email    = email;
        }

        // ── Métodos ───────────────────────────────────────────────────

        // Implementación obligatoria de la clase abstracta
        public override string GetDisplayName() =>
            string.IsNullOrWhiteSpace(Nickname)
                ? $"{FirstName} {LastName}"
                : $"{FirstName} {LastName} ({Nickname})";

        // Calcula la edad actual
        public int GetAge()
        {
            var today = DateTime.Today;
            int age   = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        // Días que faltan para el próximo cumpleaños
        public int DaysUntilBirthday()
        {
            var today   = DateTime.Today;
            var next    = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
            if (next < today) next = next.AddYears(1);
            return (next - today).Days;
        }

        // Próxima fecha de cumpleaños
        public DateTime NextBirthday()
        {
            var today = DateTime.Today;
            var next  = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
            if (next < today) next = next.AddYears(1);
            return next;
        }
    }
}
