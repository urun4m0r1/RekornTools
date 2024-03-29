﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class EnumUtility
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
