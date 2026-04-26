namespace BirthdayTracker.Models
{
    // Clase abstracta base para todas las entidades del sistema.
    // Aplica herencia y métodos abstractos (requisito POO).
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Cada entidad define cómo se muestra su nombre principal
        public abstract string GetDisplayName();

        public override string ToString() => GetDisplayName();
    }
}
