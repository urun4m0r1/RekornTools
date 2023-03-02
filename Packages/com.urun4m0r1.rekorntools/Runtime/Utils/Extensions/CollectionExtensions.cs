#nullable enable

using System.Collections.Generic;

namespace Urun4m0r1.RekornTools.Utils
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
    }
}
