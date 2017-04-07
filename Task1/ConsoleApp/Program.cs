using System;
using System.Linq;
using PrimeNumbers;

namespace ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var primeGenerator = new PrimeGenerator();

            while (true)
            {
                Console.Write("Enter count of prime numbers or 'e' to exit: ");
                var key = Console.ReadLine();

                if (key == "e")
                {
                    break;
                }

                int count;
                bool parseResult = int.TryParse(key, out count);

                if (!parseResult)
                {
                    Console.WriteLine("Entered value is invalid");
                    Console.WriteLine();
                    continue;
                }

                var primeNumbers = primeGenerator.GeneratePrimeNumbres(count);
                primeNumbers.ToList().ForEach(Console.WriteLine);

                var sum = primeGenerator.GetSpecialSum(primeNumbers);
                Console.WriteLine($"Special sum: {sum}");
                Console.WriteLine();
            }
        }
    }
}
