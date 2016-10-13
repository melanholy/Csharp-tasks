using System;
using System.Linq;

namespace autocomplete
{
	class Autocompleter
	{
		private readonly string[] items;

		public Autocompleter(string[] loadedItems)
		{
			items = loadedItems;
		}

		public string FindByPrefix(string prefix)
		{
            var index = FindIndexByBinarySearch(items, prefix, 1);
            if (index == -1) return null;
            return items[index];
		}

        public string[] FindByPrefix(string prefix, int count)
        {
            var index = FindIndexByBinarySearch(items, prefix, 1);
            if (index == -1) return new string[0];
            var answer = new string[10];
            for (var i = 0; i < 10; i++)
                if (items[index + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) answer[i] += items[index + i];
            return answer.Where(x => x != null).ToArray();
        }

        public int FindCount(string prefix)
        {
            var leftIndex = FindIndexByBinarySearch(items, prefix, 1);
            if (prefix.Length == 0 || leftIndex == -1) return 0;
            var rightIndex = FindIndexByBinarySearch(items, prefix, 2);
            return rightIndex - leftIndex;
        }

        int FindIndexByBinarySearch(string[] array, string prefix, int type)
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
