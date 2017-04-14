using System;
using Sorting;

namespace ConsoleApp.Sorting
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var array = new [] {"A", "9", "d", "Ab", "ä", "a", "aB", "m", "aa", "ab", "ss", "ß", "Ä", "0", "Äb", "äb", "Z", "z" };

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine("Initial");
            Console.WriteLine(string.Join(", ", array));

            ArraySorting.Sort(array);
            Console.WriteLine("Ascending");
            Console.WriteLine(string.Join(", ", array));

            ArraySorting.Sort(array, false);
            Console.WriteLine("Descending");
            Console.WriteLine(string.Join(", ", array));

            Console.ReadKey();
        }
    }
}
