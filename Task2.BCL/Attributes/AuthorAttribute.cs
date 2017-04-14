using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorAttribute : Attribute
    {
        public string Name { get; }

        public string Email { get; }

        public AuthorAttribute(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
