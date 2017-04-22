using System;
using NUnit.Framework;

namespace ConversionLib.Tests
{
    [TestFixture]
    public class StringConversionTests
    {
        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("13-", ExpectedException = typeof(ArgumentException))]
        [TestCase("qwert1y", ExpectedException = typeof(ArgumentException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        [TestCase("-", ExpectedException = typeof(ArgumentException))]
        [TestCase("0", ExpectedResult = 0)]
        [TestCase("-0", ExpectedResult = 0)]
        [TestCase("+1", ExpectedResult = 1)]
        [TestCase("-13", ExpectedResult = -13)]
        [TestCase("+13", ExpectedResult = 13)]
        [TestCase("13", ExpectedResult = 13)]
        [TestCase("2147483647", ExpectedResult = int.MaxValue)]
        [TestCase("-2147483648", ExpectedResult = int.MinValue)]
        [TestCase("2147483648", ExpectedException = typeof(OverflowException))]
        [TestCase("-2147483649", ExpectedException = typeof(OverflowException))]
        public int Test_ConvertStringToInt(string value)
        {
            return value.ConvertToInt();
        }
    }
}
