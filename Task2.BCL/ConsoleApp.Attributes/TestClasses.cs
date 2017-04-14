using Attributes;

namespace ConsoleApp.Attributes
{
    [Author("Natalia", "email@example.com")]
    public class FirstClass
    { }

    [Author("Natalia", null)]
    public class SecondClass
    { }

    public class ThirdClass
    { }
}
