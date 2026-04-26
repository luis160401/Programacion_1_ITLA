using BirthdayTracker.Business;
using BirthdayTracker.Models;

namespace BirthdayTracker.Presentation
{
    // Menú principal de consulta de cumpleaños.
    public class BirthdayMenu
    {
        private readonly ContactService _service = new();

        private static readonly string[] Months =
        {
            "", "Enero","Febrero","Marzo","Abril","Mayo","Junio",
            "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
        };

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("CONSULTA DE CUMPLEAÑOS");
                Console.WriteLine("  1. Cumpleaños del mes actual");
                Console.WriteLine("  2. Cumpleaños por mes específico");
                Console.WriteLine("  3. Próximos cumpleaños (30 días)");
                Console.WriteLine("  4. Próximos cumpleaños (personalizado)");
                Console.WriteLine("  0. Volver al menú principal");
                ConsoleHelper.PrintSeparator();

                int opt = ConsoleHelper.ReadInt("Opción", 0, 4);
                switch (opt)
                {
                    case 1: ShowCurrentMonth(); break;
                    case 2: ShowByMonth();      break;
                    case 3: ShowUpcoming(30);   break;
                    case 4: ShowUpcomingCustom(); break;
                    case 0: back = true;         break;
                }
            }
        }

        private void ShowCurrentMonth()
        {
            int month = DateTime.Today.Month;
            Console.Clear();
            ConsoleHelper.PrintTitle($"CUMPLEAÑOS DE {Months[month].ToUpper()}");
            var list = _service.Search(month);
            PrintBirthdayList(list);
            ConsoleHelper.PressAnyKey();
        }

        private void ShowByMonth()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("CUMPLEAÑOS POR MES");
            for (int i = 1; i <= 12; i++)
                Console.WriteLine($"  {i,2}. {Months[i]}");
            int month = ConsoleHelper.ReadInt("Selecciona un mes", 1, 12);
            Console.Clear();
            ConsoleHelper.PrintTitle($"CUMPLEAÑOS DE {Months[month].ToUpper()}");
            var list = _service.Search(month);
            PrintBirthdayList(list);
            ConsoleHelper.PressAnyKey();
        }

        private void ShowUpcoming(int days)
        {
            Console.Clear();
            ConsoleHelper.PrintTitle($"PRÓXIMOS CUMPLEAÑOS ({days} DÍAS)");
            var list = _service.GetUpcoming(days);
            PrintBirthdayList(list);
            ConsoleHelper.PressAnyKey();
        }

        private void ShowUpcomingCustom()
        {
            int days = ConsoleHelper.ReadInt("¿Cuántos días hacia adelante?", 1, 365);
            ShowUpcoming(days);
        }

        private static void PrintBirthdayList(List<Contact> list)
        {
            if (list.Count == 0) { ConsoleHelper.PrintInfo("No hay cumpleaños en este período."); return; }
            ConsoleHelper.PrintSeparator();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  {0,-28} {1,-6} {2,-12} {3}",
                "Nombre", "Edad", "Cumpleaños", "Faltan");
            Console.ResetColor();
            ConsoleHelper.PrintSeparator();
            foreach (var c in list)
            {
                int days = c.DaysUntilBirthday();
                string label = days == 0 ? "¡HOY! 🎂" : $"{days} día(s)";

                if (days == 0) Console.ForegroundColor = ConsoleColor.Yellow;
                else if (days <= 7) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ResetColor();

                Console.WriteLine("  {0,-28} {1,-6} {2,-12} {3}",
                    c.GetDisplayName()[..Math.Min(c.GetDisplayName().Length, 27)],
                    c.GetAge() + 1,
                    c.BirthDate.ToString("dd/MM"),
                    label);
                Console.ResetColor();
            }
            ConsoleHelper.PrintSeparator();
        }
    }
}
