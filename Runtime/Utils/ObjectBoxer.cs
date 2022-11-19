#nullable enable

namespace Rekorn.Tools.Utils
{
    public sealed class ObjectBoxer<T> where T : notnull, new()
    {
        public readonly object Box;

        public ObjectBoxer() => Box = new T();

        public ObjectBoxer(T obj) => Box = obj;

        public T Unbox() => (T)Box;
    }
}
