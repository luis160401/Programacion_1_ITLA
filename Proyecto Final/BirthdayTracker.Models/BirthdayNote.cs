namespace BirthdayTracker.Models
{
    // Nota asociada al cumpleaños de un contacto
    public class BirthdayNote : BaseEntity
    {
        public int    ContactId { get; set; }
        public string NoteText  { get; set; } = string.Empty;

        public BirthdayNote() { }

        public BirthdayNote(int contactId, string noteText)
        {
            ContactId = contactId;
            NoteText  = noteText;
        }

        public override string GetDisplayName() => NoteText;
    }

    // Registro de recordatorio generado por el sistema
    public class ReminderLog : BaseEntity
    {
        public int      ContactId    { get; set; }
        public DateTime ReminderDate { get; set; }
        public string   Message      { get; set; } = string.Empty;

        public ReminderLog() { }

        public ReminderLog(int contactId, string message)
        {
            ContactId    = contactId;
            ReminderDate = DateTime.Today;
            Message      = message;
        }

        public override string GetDisplayName() => $"[{ReminderDate:dd/MM/yyyy}] {Message}";
    }
}
