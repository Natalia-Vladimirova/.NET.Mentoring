using System;
using System.Linq;

namespace Attributes
{
    public class AuthorService
    {
        public void DisplayAuthorInfo<T>()
        {
            var author = GetAuthorAttribute<T>();

            if (author == null)
            {
                Console.WriteLine($"The class {typeof(T)} does not have author info");
            }
            else
            {
                Console.WriteLine($"Name: {author.Name}; Email: {author.Email}");
            }
        }

        private AuthorAttribute GetAuthorAttribute<T>()
        {
            return typeof(T).GetCustomAttributes(typeof(AuthorAttribute), true)
                .FirstOrDefault() as AuthorAttribute;
        }
    }
}
