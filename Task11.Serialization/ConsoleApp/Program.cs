using System;
using System.Collections.Generic;
using System.Text;
using BookCatalog;
using BookCatalog.Models;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var xmlService = new CatalogXmlService(new SettingsProvider());

            while (true)
            {
                Console.WriteLine("1 - serialize to file; 2 - deserialize from file; other - exit");
                Console.Write("Enter a key: ");
                string key = Console.ReadLine();
                Console.WriteLine();
                
                switch (key)
                {
                    case "1":
                        xmlService.Serialize(GetCatalog());
                        break;
                    case "2":
                        PrintCatalog(xmlService.Deserialize());
                        break;
                    default:
                        return;
                }

                Console.WriteLine();
            }
        }

        private static Catalog GetCatalog()
        {
            return new Catalog
            {
                Date = new DateTime(2016, 02, 05),
                Books = new List<Book>
                {
                    new Book
                    {
                        BookId = "bk101",
                        Isbn = "0-596-00103-7",
                        Author = "Löwy, Juval",
                        Title = "COM and .NET Component Services",
                        Genre = Genre.Computer,
                        Publisher = "O'Reilly",
                        PublishDate = new DateTime(2001, 09, 01),
                        Description = "COM & smth",
                        RegistrationDate = new DateTime(2016, 01, 05)
                    },
                    new Book
                    {
                        BookId = "bk102",
                        Author = "Ralls, Kim",
                        Title = "Midnight Rain",
                        Genre = Genre.ScienceFiction,
                        Publisher = "R &amp; D",
                        PublishDate = new DateTime(2000, 12, 16),
                        Description = "description.",
                        RegistrationDate = new DateTime(2017, 01, 01)
                    }
                }
            };
        }

        private static void PrintCatalog(Catalog catalog)
        {
            Console.WriteLine($"Catalog date: {catalog.Date:yyyy-MM-dd}");
            foreach (var book in catalog.Books)
            {
                Console.WriteLine($"Book {book.BookId}");
                Console.WriteLine($"\tisbn: {book.Isbn}");
                Console.WriteLine($"\tauthor: {book.Author}");
                Console.WriteLine($"\ttitle: {book.Title}");
                Console.WriteLine($"\tgenre: {book.Genre}");
                Console.WriteLine($"\tpublisher: {book.Publisher}");
                Console.WriteLine($"\tpublish date: {book.PublishDate:yyyy-MM-dd}");
                Console.WriteLine($"\tdescription: {book.Description}");
                Console.WriteLine($"\tregistration date: {book.RegistrationDate:yyyy-MM-dd}");
            }
        }
    }
}
