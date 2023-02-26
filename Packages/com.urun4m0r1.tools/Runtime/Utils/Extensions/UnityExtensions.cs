#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class UnityExtensions
    {
        public static bool IsReferenceNull(this Object? obj) => ReferenceEquals(obj, null);
        public static bool IsFakeNull(this Object? obj) => !ReferenceEquals(obj, null) && obj;
        public static bool IsAssigned(this Object? obj) => obj;

        public static bool Contains(this LayerMask mask, int layer) => mask == (mask | (1 << layer));
    }
}
