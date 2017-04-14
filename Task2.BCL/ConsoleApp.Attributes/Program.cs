using System;
using Attributes;

namespace ConsoleApp.Attributes
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var authorService = new AuthorService();

            authorService.DisplayAuthorInfo<FirstClass>();
            authorService.DisplayAuthorInfo<SecondClass>();
            authorService.DisplayAuthorInfo<ThirdClass>();

            Console.ReadKey();
        }
    }
}
