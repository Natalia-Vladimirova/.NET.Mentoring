using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeNumbers
{
    public class PrimeGenerator
    {
        public IList<int> GeneratePrimeNumbres(int count)
        {
            var result = new List<int>();

            int currentPrimeNumber = 2;

            for (int i = 0; i < count; i++)
            {
                result.Add(currentPrimeNumber);

                currentPrimeNumber = GetNextPrimeNumber(result);
            }

            return result;
        }

        public int GetSpecialSum(IList<int> primeNumbers)
        {
            if (primeNumbers == null)
            {
                return 0;
            }

            double sum = primeNumbers.Select((x, i) => (double) x/(i + 1)).Sum();
            
            return (int)Math.Floor(sum);
        }

        private int GetNextPrimeNumber(IList<int> previousPrimeNumbers)
        {
            int nextPrimeNumber = previousPrimeNumbers.Last();

            while (true)
            {
                nextPrimeNumber++;

                if (previousPrimeNumbers.All(x => nextPrimeNumber % x != 0))
                {
                    return nextPrimeNumber;
                }
            }
        }
    }
}
