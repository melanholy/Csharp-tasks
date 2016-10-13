using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autocomplete
{
    public class ClassForTests
    {
        public static string FindByPrefix(string prefix, string[] items)
        {
            var index = FindIndexByBinarySearch(items, prefix, 1);
            if (index == -1) return null;
            return items[index];
        }

        public static string FindByPrefix(string prefix, int count, string[] items)
        {
            var index = FindIndexByBinarySearch(items, prefix, 1);
            if (index == -1) return null;
            var answer = "";
            for (var i = 0; i < count; i++)
                if (items[index + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) answer += items[index + i] + " ";
            return answer;
        }

        public static int FindCount(string prefix, string[] items)
        {
            var leftIndex = FindIndexByBinarySearch(items, prefix, 1);
            if (prefix.Length == 0 || leftIndex == -1) return 0;
            var rightIndex = FindIndexByBinarySearch(items, prefix, 2);
            return rightIndex - leftIndex;
        }

        static int FindIndexByBinarySearch(string[] array, string prefix, int type)
        {
            if (prefix.Length == 0) return -1;
            int left = 0, right = array.Length - 1;
            while (type == 1 ? left < right : left - right != -1)
            {
                var middle = (right + left) / 2;
                var cmp = -1;
                if (prefix.Length <= array[middle].Length) cmp = array[middle].ToLower().Substring(0, prefix.Length).CompareTo(prefix.ToLower());
                if (cmp == 1)
                    right = middle;
                if (cmp == -1)
                    left = middle + 1;
                if (cmp == 0)
                    if (type == 1) right = middle;
                    else left = middle;
            }
            if (type == 2 || array[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return right;
            return -1;
        }
    }
}
