using System;
using System.Linq;

namespace ConversionLib
{
    public static class StringConversion
    {
        public static int ConvertToInt(this string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            if (number == string.Empty)
            {
                throw new ArgumentException("The argument must not be empty", nameof(number));
            }

            if (number.Length == 1 && !char.IsDigit(number[0]))
            {
                throw new ArgumentException("The argument must contain only digits", nameof(number));
            }
            
            int correctionSign = 1;

            if (IsMinus(number[0]))
            {
                correctionSign = -1;
                number = number.Substring(1);
            }
            else if (IsPlus(number[0]))
            {
                number = number.Substring(1);
            }

            if (!number.All(char.IsDigit))
            {
                throw new ArgumentException("The argument must contain only digits or digits and a sign", nameof(number));
            }

            int resultNumber = 0;

            for (int i = 0; i < number.Length; i++)
            {
                resultNumber = checked(resultNumber * 10 + correctionSign * GetDigitFromChar(number[i]));
            }

            return resultNumber;
        }

        private static int GetDigitFromChar(char value)
        {
            return (int)char.GetNumericValue(value);
        }
        
        private static bool IsMinus(char value)
        {
            return value == '-';
        }

        private static bool IsPlus(char value)
        {
            return value == '+';
        }
    }
}
