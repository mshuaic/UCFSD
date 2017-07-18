using System;
using System.Collections.Generic;

namespace CheckupExec.Utilities
{
    public static class SortingUtility<T> where T : IComparable<T>
    {
        //standard quicksort is used
        public static void Sort(List<T> objects, int left, int right)
        {
            objects = objects ?? new List<T>();

            Qsort(objects, left, right);
        }

        public static void Qsort(List<T> objects, int left, int right)
        {
            int i = left, j = right;
            T pivot = objects[(left + right + ((left + right) / 2)) / 3];

            while (i <= j)
            {
                while (objects[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (objects[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i > j) continue;

                Swap(objects, i, j);

                i++;
                j--;
            }

            // Recursive calls
            if (left < j)
            {
                Qsort(objects, left, j);
            }

            if (i < right)
            {
                Qsort(objects, i, right);
            }
        }

        private static void Swap(List<T> objects, int a, int b)
        {
            var temp = objects[a];
            objects[a] = objects[b];
            objects[b] = temp;
        }
    }
}
