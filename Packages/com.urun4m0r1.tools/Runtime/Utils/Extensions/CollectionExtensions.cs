#nullable enable

using System.Collections.Generic;

namespace Rekorn.Tools.Utils
{
    public static class CollectionExtensions
    {
        public static bool TryAdd<T>(this ICollection<T> collection, T target)
        {
            if (collection.Contains(target))
            {
                return false;
            }

            collection.Add(target);
            return true;
        }

        public static void RemoveItems<T>(this ICollection<T> collection, Queue<T> target)
        {
            while (target.Count > 0)
            {
                collection.Remove(target.Dequeue());
            }
        }
    }
}
