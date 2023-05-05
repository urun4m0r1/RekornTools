#nullable enable

namespace Urun4m0r1.RekornTools
{
    public interface IRegisterable<in T>
    {
        bool Contains(T item);
        void Add(T      item);
        bool TryAdd(T   item);
        void Remove(T   item);
    }
}
