using BirthdayTracker.DataAccess;
using BirthdayTracker.Presentation;

// ── Punto de entrada del programa ─────────────────────────────
Console.Title = "BirthdayTracker v1.0";

// Verificar conexión a base de datos al iniciar
if (!DatabaseConfig.TestConnection())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: No se pudo conectar a la base de datos.");
    Console.WriteLine("Asegúrate de que SQL Server Express está activo y");
    Console.WriteLine("que la base de datos BirthdayTrackerDB existe.");
    Console.ResetColor();
    Console.ReadKey();
    return;
}

var contactMenu  = new ContactMenu();
var birthdayMenu = new BirthdayMenu();

bool exit = false;
while (!exit)
{
    Console.Clear();
    ConsoleHelper.PrintTitle("BIRTHDAYTRACKER  🎂  Agenda de Cumpleaños");
    Console.WriteLine("  1. Gestión de Contactos");
    Console.WriteLine("  2. Consulta de Cumpleaños");
    Console.WriteLine("  0. Salir");
    ConsoleHelper.PrintSeparator();

    int opt = ConsoleHelper.ReadInt("Opción", 0, 2);
    switch (opt)
    {
        case 1: contactMenu.Show();  break;
        case 2: birthdayMenu.Show(); break;
        case 0: exit = true;         break;
    }
}

Console.Clear();
ConsoleHelper.PrintTitle("¡Hasta luego!");
Console.WriteLine("  BirthdayTracker cerrado correctamente.");
Console.WriteLine();
