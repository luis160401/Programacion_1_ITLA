using BirthdayTracker.DataAccess;
using BirthdayTracker.Models;

namespace BirthdayTracker.Business
{
    public class NoteService
    {
        private readonly NoteRepository _repo;

        public NoteService()
        {
            _repo = new NoteRepository();
        }

        public int AddNote(int contactId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("La nota no puede estar vacía.");

            var note = new BirthdayNote(contactId, text.Trim());
            return _repo.Insert(note);
        }

        public List<BirthdayNote> GetNotesForContact(int contactId)
            => _repo.GetByContact(contactId);

        public bool DeleteNote(int noteId)
            => _repo.Delete(noteId);
    }
}
