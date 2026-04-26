namespace BirthdayTracker.Presentation
{
    // Utilidades de presentación para la consola.
    // Centraliza colores, bordes y formato de tablas.
    public static class ConsoleHelper
    {
        private static readonly string Separator = new string('─', 60);

        public static void PrintTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  ╔{'═'.ToString().PadRight(56, '═')}╗");
            Console.WriteLine($"  ║  {title.PadRight(54)}║");
            Console.WriteLine($"  ╚{'═'.ToString().PadRight(56, '═')}╝");
            Console.ResetColor();
        }

        public static void PrintSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {Separator}");
            Console.ResetColor();
        }

        public static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✓  {msg}");
            Console.ResetColor();
        }

        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ✗  ERROR: {msg}");
            Console.ResetColor();
        }

        public static void PrintInfo(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  →  {msg}");
            Console.ResetColor();
        }

        public static string ReadInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  {prompt}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static int ReadInt(string prompt, int min = 0, int max = int.MaxValue)
        {
            while (true)
            {
                string raw = ReadInput(prompt);
                if (int.TryParse(raw, out int val) && val >= min && val <= max)
                    return val;
                PrintError($"Ingrese un número entre {min} y {max}.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                string raw = ReadInput($"{prompt} (dd/MM/yyyy)");
                if (DateTime.TryParseExact(raw, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dt))
                    return dt;
                PrintError("Formato de fecha inválido. Usa dd/MM/yyyy.");
            }
        }

        public static bool Confirm(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {prompt} (s/n): ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim().ToLower() == "s";
        }

        public static void PressAnyKey()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Presiona cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
