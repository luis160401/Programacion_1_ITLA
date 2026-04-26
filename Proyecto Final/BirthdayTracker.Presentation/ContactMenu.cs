using BirthdayTracker.Business;
using BirthdayTracker.Models;

namespace BirthdayTracker.Presentation
{
    // Menú de gestión de contactos en consola.
    public class ContactMenu
    {
        private readonly ContactService _service = new();
        private readonly NoteService    _notes   = new();

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("GESTIÓN DE CONTACTOS");
                Console.WriteLine("  1. Listar todos los contactos");
                Console.WriteLine("  2. Agregar contacto");
                Console.WriteLine("  3. Buscar por nombre");
                Console.WriteLine("  4. Editar contacto");
                Console.WriteLine("  5. Eliminar contacto");
                Console.WriteLine("  6. Ver notas de un contacto");
                Console.WriteLine("  7. Agregar nota a contacto");
                Console.WriteLine("  0. Volver al menú principal");
                ConsoleHelper.PrintSeparator();

                int opt = ConsoleHelper.ReadInt("Opción", 0, 7);
                switch (opt)
                {
                    case 1: ListAll();       break;
                    case 2: AddContact();    break;
                    case 3: SearchByName();  break;
                    case 4: EditContact();   break;
                    case 5: DeleteContact(); break;
                    case 6: ViewNotes();     break;
                    case 7: AddNote();       break;
                    case 0: back = true;     break;
                }
            }
        }

        // ── Listar ────────────────────────────────────────────────────
        private void ListAll()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("TODOS LOS CONTACTOS");
            var list = _service.Search();
            PrintContactList(list);
            ConsoleHelper.PressAnyKey();
        }

        // ── Agregar ───────────────────────────────────────────────────
        private void AddContact()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("NUEVO CONTACTO");
            try
            {
                string first   = ConsoleHelper.ReadInput("Nombre");
                string last    = ConsoleHelper.ReadInput("Apellido");
                DateTime birth = ConsoleHelper.ReadDate("Fecha de nacimiento");
                string nick    = ConsoleHelper.ReadInput("Apodo (opcional, Enter para omitir)");
                string phone   = ConsoleHelper.ReadInput("Teléfono (opcional)");
                string email   = ConsoleHelper.ReadInput("Correo electrónico (opcional)");

                var contact = new Contact(first, last, birth)
                {
                    Nickname = string.IsNullOrEmpty(nick) ? null : nick,
                    Phone    = string.IsNullOrEmpty(phone) ? null : phone,
                    Email    = string.IsNullOrEmpty(email) ? null : email,
                };

                int id = _service.AddContact(contact);
                ConsoleHelper.PrintSuccess($"Contacto guardado con ID #{id}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            ConsoleHelper.PressAnyKey();
        }

        // ── Buscar ────────────────────────────────────────────────────
        private void SearchByName()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("BUSCAR CONTACTO");
            string query = ConsoleHelper.ReadInput("Nombre o apodo");
            try
            {
                var results = _service.Search(query);
                PrintContactList(results);
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.PressAnyKey();
        }

        // ── Editar ────────────────────────────────────────────────────
        private void EditContact()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("EDITAR CONTACTO");
            int id = ConsoleHelper.ReadInt("ID del contacto a editar", 1);
            var c  = _service.GetById(id);
            if (c == null) { ConsoleHelper.PrintError("Contacto no encontrado."); ConsoleHelper.PressAnyKey(); return; }

            ConsoleHelper.PrintInfo($"Editando: {c.GetDisplayName()} (Enter para mantener valor actual)");
            try
            {
                string first = ConsoleHelper.ReadInput($"Nombre [{c.FirstName}]");
                if (!string.IsNullOrEmpty(first)) c.FirstName = first;

                string last = ConsoleHelper.ReadInput($"Apellido [{c.LastName}]");
                if (!string.IsNullOrEmpty(last)) c.LastName = last;

                string phone = ConsoleHelper.ReadInput($"Teléfono [{c.Phone ?? "vacío"}]");
                if (!string.IsNullOrEmpty(phone)) c.Phone = phone;

                string email = ConsoleHelper.ReadInput($"Email [{c.Email ?? "vacío"}]");
                if (!string.IsNullOrEmpty(email)) c.Email = email;

                _service.UpdateContact(c);
                ConsoleHelper.PrintSuccess("Contacto actualizado correctamente.");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.PressAnyKey();
        }

        // ── Eliminar ──────────────────────────────────────────────────
        private void DeleteContact()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ELIMINAR CONTACTO");
            int id = ConsoleHelper.ReadInt("ID del contacto a eliminar", 1);
            var c  = _service.GetById(id);
            if (c == null) { ConsoleHelper.PrintError("Contacto no encontrado."); ConsoleHelper.PressAnyKey(); return; }

            ConsoleHelper.PrintInfo($"Se eliminará: {c.GetDisplayName()}");
            if (ConsoleHelper.Confirm("¿Confirmas la eliminación?"))
            {
                _service.DeleteContact(id);
                ConsoleHelper.PrintSuccess("Contacto eliminado.");
            }
            else
            {
                ConsoleHelper.PrintInfo("Operación cancelada.");
            }
            ConsoleHelper.PressAnyKey();
        }

        // ── Notas ─────────────────────────────────────────────────────
        private void ViewNotes()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("NOTAS DE CONTACTO");
            int id    = ConsoleHelper.ReadInt("ID del contacto", 1);
            var notes = _notes.GetNotesForContact(id);
            if (notes.Count == 0)
                ConsoleHelper.PrintInfo("No hay notas para este contacto.");
            else
                foreach (var n in notes)
                    Console.WriteLine($"  [{n.Id}] {n.NoteText}  ({n.CreatedAt:dd/MM/yyyy})");
            ConsoleHelper.PressAnyKey();
        }

        private void AddNote()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("AGREGAR NOTA");
            int    id   = ConsoleHelper.ReadInt("ID del contacto", 1);
            string text = ConsoleHelper.ReadInput("Nota");
            try
            {
                _notes.AddNote(id, text);
                ConsoleHelper.PrintSuccess("Nota guardada.");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.PressAnyKey();
        }

        // ── Helper de impresión ───────────────────────────────────────
        private static void PrintContactList(List<Contact> list)
        {
            if (list.Count == 0) { ConsoleHelper.PrintInfo("No se encontraron contactos."); return; }
            ConsoleHelper.PrintSeparator();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  {0,-4} {1,-28} {2,-12} {3,4} {4,4}",
                "ID", "Nombre", "Cumpleaños", "Edad", "Días");
            Console.ResetColor();
            ConsoleHelper.PrintSeparator();
            foreach (var c in list)
            {
                Console.WriteLine("  {0,-4} {1,-28} {2,-12} {3,4} {4,4}",
                    c.Id,
                    c.GetDisplayName()[..Math.Min(c.GetDisplayName().Length, 27)],
                    c.BirthDate.ToString("dd/MM/yyyy"),
                    c.GetAge(),
                    c.DaysUntilBirthday());
            }
            ConsoleHelper.PrintSeparator();
            ConsoleHelper.PrintInfo($"Total: {list.Count} contacto(s)");
        }
    }
}
