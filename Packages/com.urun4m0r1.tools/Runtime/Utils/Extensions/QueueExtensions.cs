#nullable enable

using System.Collections.Generic;

namespace Rekorn.Tools.Utils
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
