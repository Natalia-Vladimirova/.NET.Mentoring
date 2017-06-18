using System.Collections.Generic;

namespace FibonacciNumbers.Cache
{
    public interface ICache
    {
        IEnumerable<int> Get(string forUser);

        void Set(string forUser, IEnumerable<int> numbers);
    }
}
