using System;
using System.Collections.Generic;

namespace FibonacciNumbers
{
    public class FibonacciGenerator : IFibonacciGenerator
    {
        public IEnumerable<int> Get(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException($"{nameof(count)} must be a positive number");
            }

            if (count == 0)
            {
                yield break;
            }

            yield return 1;

            int previous = 0;
            int current = 1;

            for (int i = 0; i < count - 1; i++)
            {
                int temp = current;
                current = checked(current + previous);
                previous = temp;

                yield return current;
            }
        }
    }
}
