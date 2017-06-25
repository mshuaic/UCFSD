using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    public static class SortingUtility<T> where T : IComparable<T>
    {
        public static bool isSorted(List<T> objects)
        {
            objects = objects ?? new List<T>();

            int count = objects.Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (objects[i].CompareTo(objects[i + 1]) == 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static void sort(List<T> objects, int left, int right)
        {
            objects = objects ?? new List<T>();

            if (!isSorted(objects))
            {
                qsort(objects, left, right);
            }
        }

        private static void qsort(List<T> objects, int left, int right)
        {
            int i = left, j = right;
            T pivot = objects[(left + right) / 2];

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
                if (i <= j)
                {
                    swap(objects[i], objects[j]);
                    i++;
                    j--;
                }
            }

            if (left < j)
            {
                sort(objects, left, j);
            }
            if (i < right)
            {
                sort(objects, i, right);
            }
        }

        private static void swap(T a, T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
