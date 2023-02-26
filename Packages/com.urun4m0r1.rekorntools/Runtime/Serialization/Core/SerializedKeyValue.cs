#nullable enable

using UnityEngine;
using System;

namespace Urun4m0r1.RekornTools.Serialization
{
    public sealed class SerializedKeyValue : SerializedKeyValue<object, object> { }

    [Serializable] public class HorizontalKeyValue<TKey, TValue> : SerializedKeyValue<TKey, TValue> { }

    [Serializable] public class SerializedKeyValue<TKey, TValue>
    {
        [field: SerializeField] public TKey?   Key   { get; set; }
        [field: SerializeField] public TValue? Value { get; set; }

        protected SerializedKeyValue() { }

        protected SerializedKeyValue(TKey? key, TValue? value)
        {
            Key   = key;
            Value = value;
        }
    }
}
