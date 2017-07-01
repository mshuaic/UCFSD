using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    public static class SortingUtility<T> where T : IComparable<T>
    {
        //standard quicksort is used
        public static void sort(List<T> objects, int left, int right)
        {
            objects = objects ?? new List<T>();

            qsort(objects, left, right);
        }

        public static void qsort(List<T> objects, int left, int right)
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

                if (i <= j)
                {
                    swap(objects, i, j);

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                qsort(objects, left, j);
            }

            if (i < right)
            {
                qsort(objects, i, right);
            }
        }

        private static void swap(List<T> objects, int a, int b)
        {
            var temp = objects[a];
            objects[a] = objects[b];
            objects[b] = temp;
        }
    }
}
