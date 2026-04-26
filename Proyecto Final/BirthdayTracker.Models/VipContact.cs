namespace BirthdayTracker.Models
{
    // Herencia: VipContact extiende Contact con prioridad especial.
    // Demuestra sobrecarga de GetDisplayName() y constructores heredados.
    public class VipContact : Contact
    {
        public int PriorityLevel { get; set; } = 1; // 1=familia, 2=amigo cercano, 3=conocido

        public VipContact() : base() { }

        public VipContact(string firstName, string lastName, DateTime birthDate, int priorityLevel)
            : base(firstName, lastName, birthDate)
        {
            PriorityLevel = priorityLevel;
        }

        // Sobrecarga del método de la clase base
        public override string GetDisplayName()
        {
            string stars = new string('★', PriorityLevel);
            return $"{stars} {base.GetDisplayName()}";
        }
    }
}
