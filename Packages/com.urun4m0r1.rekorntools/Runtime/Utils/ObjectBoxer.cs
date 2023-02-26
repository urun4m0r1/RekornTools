#nullable enable

namespace Urun4m0r1.RekornTools.Utils
{
    public sealed class ObjectBoxer<T> where T : notnull, new()
    {
        public readonly object Box;

        public ObjectBoxer() => Box = new T();

        public ObjectBoxer(T obj) => Box = obj;

        public T Unbox() => (T)Box;
    }
}
