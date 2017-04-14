using System;

namespace Sorting
{
    public static class ArraySorting
    {
        public static void Sort(string[] array, bool ascSortOrder = true)
        {
            var comparer = StringComparer.InvariantCulture;

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (comparer.Compare(array[i], array[j]) > 0 == ascSortOrder)
                    {
                        Swap(array, i, j);
                    }
                }
            }
        }

        private static void Swap(string[] array, int firstIndex, int secondIndex)
        {
            string temp = array[firstIndex];
            array[firstIndex] = array[secondIndex];
            array[secondIndex] = temp;
        }
    }
}
