using BirthdayTracker.DataAccess;
using BirthdayTracker.Models;

namespace BirthdayTracker.Business
{
    // Capa de negocio para la gestión de contactos.
    // Aplica sobrecarga de métodos (Search) y validaciones.
    public class ContactService
    {
        private readonly ContactRepository _repo;

        public ContactService()
        {
            _repo = new ContactRepository();
        }

        // ── Sobrecargas del método Search (requisito POO) ─────────────

        // Sin parámetros: devuelve todos
        public List<Contact> Search()
            => _repo.GetAll();

        // Por nombre o apodo
        public List<Contact> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de búsqueda no puede estar vacío.");
            return _repo.SearchByName(name.Trim());
        }

        // Por mes de cumpleaños
        public List<Contact> Search(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "El mes debe estar entre 1 y 12.");
            return _repo.GetByBirthMonth(month);
        }

        // ── Próximos cumpleaños ───────────────────────────────────────
        public List<Contact> GetUpcoming(int days = 30) => _repo.GetUpcoming(days);

        // ── CRUD con validaciones ─────────────────────────────────────
        public int AddContact(Contact contact)
        {
            Validate(contact);
            return _repo.Insert(contact);
        }

        public bool UpdateContact(Contact contact)
        {
            if (contact.Id <= 0)
                throw new InvalidOperationException("El contacto no tiene un Id válido.");
            Validate(contact);
            return _repo.Update(contact);
        }

        public bool DeleteContact(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id de contacto inválido.");
            return _repo.Delete(id);
        }

        public Contact? GetById(int id) => _repo.GetById(id);

        // ── Validaciones internas ─────────────────────────────────────
        private static void Validate(Contact c)
        {
            if (string.IsNullOrWhiteSpace(c.FirstName))
                throw new ArgumentException("El nombre es requerido.");
            if (string.IsNullOrWhiteSpace(c.LastName))
                throw new ArgumentException("El apellido es requerido.");
            if (c.BirthDate == default || c.BirthDate > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no es válida.");
            if (!string.IsNullOrEmpty(c.Email) && !c.Email.Contains('@'))
                throw new ArgumentException("El correo electrónico no es válido.");
        }
    }
}
