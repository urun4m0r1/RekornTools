#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rekorn.Tools.Utils
{
    public static class EnumUtility
    {
        public static List<T> GetValuesList<T>() where T : Enum
        {
            return GetValues<T>().ToList();
        }

        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
