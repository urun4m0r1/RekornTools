#nullable enable

using System;
using System.Collections.Generic;

namespace Rekorn.Tools.Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> CreateNewItems<T>(int count) where T : new()
        {
            for (var i = 0; i < count; i++) yield return new T();
        }

        public static IEnumerable<T?> CreateDefaultItems<T>(int count)
        {
            for (var i = 0; i < count; i++) yield return default;
        }

        public static IEnumerable<T?> CreateItems<T>(int count, Func<T?> generator)
        {
            for (var i = 0; i < count; i++) yield return generator.Invoke();
        }
    }
}
