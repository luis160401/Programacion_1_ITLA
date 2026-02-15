using System;
using System.Runtime.InteropServices;
namespace MyProgram
{
    class Practica2
    {
        static void Main(string[] args0)
        {   
            Console.WriteLine($"Bienvenido al determinador de pares o Impares/n");
            Console.WriteLine($"-----------------------------------------------");
            Console.Write($"Escribe el numero : ");
            int num= int.Parse(Console.ReadLine());

            if (num % 2 == 0 )
                {Console.WriteLine($"El numero {num} es par");}

            else 
                {Console.WriteLine($"El numero {num} es impar");}
        }
    }
}
