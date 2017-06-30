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

        //standard quicksort is used
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
            int pivot = 0;

            if (left < right)
            {
                pivot = partition(objects, left, right);
                qsort(objects, left, pivot - 1);
                qsort(objects, pivot + 1, right);
            }
        }

        private static int partition(List<T> objects, int left, int right)
        {
            var pivot = objects[(left + right) / 2];
            int i = left - 1;

            for (int j = left; j < right - 1; j++)
            {
                if (objects[j].CompareTo(pivot) <= 0)
                {
                    i++;
                    swap(objects[i], objects[j]);
                }
            }
            swap(objects[i + 1], objects[right]);
            return i + 1;
        }

        private static void swap(T a, T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
