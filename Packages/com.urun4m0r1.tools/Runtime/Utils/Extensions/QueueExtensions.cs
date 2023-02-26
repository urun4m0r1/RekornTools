#nullable enable

using System.Collections.Generic;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class QueueExtensions
    {
        public static bool TryEnqueue<T>(this Queue<T> queue, T target)
        {
            if (queue.Contains(target))
            {
                return false;
            }

            queue.Enqueue(target);
            return true;
        }
    }
}
