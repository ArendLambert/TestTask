using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask.Extensions
{
    public static class IListExtension
    {
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            int _size = list.Count();
            int freeIndex = 0;

            while (freeIndex < _size && !match(list[freeIndex])) freeIndex++;
            if (freeIndex >= _size)
            {
                return 0;
            }

            int current = freeIndex + 1;
            while (current < _size)
            {
                while (current < _size && match(list[current])) current++;

                if (current < _size)
                {
                    list[freeIndex++] = list[current++];
                }
            }

            for (int i = _size; i > freeIndex; i--)
            {
                list.RemoveAt(i - 1);
            }


            int result = _size - freeIndex;
            _size = freeIndex;
            return result;
        }
    }
}
