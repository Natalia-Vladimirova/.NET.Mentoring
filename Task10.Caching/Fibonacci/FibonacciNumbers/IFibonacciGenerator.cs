using System.Collections.Generic;

namespace FibonacciNumbers
{
    public interface IFibonacciGenerator
    {
        IEnumerable<int> Get(int count);
    }
}
