﻿#nullable enable

using UnityEditor;
using UnityEngine;
using static Urun4m0r1.RekornTools.Utils.ReflectionExtensions;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public sealed class SerializedKeyValueHelper
    {
        public    SerializedProperty? Key   { get; private set; }
        public SerializedProperty? Value { get; private set; }

        private readonly string _keyName   = ResolveFieldName(nameof(SerializedKeyValue.Key));
        private readonly string _valueName = ResolveFieldName(nameof(SerializedKeyValue.Value));

        private SerializedProperty? _container;

        public SerializedKeyValueHelper Update(SerializedProperty? container)
        {
            if (_container != container)
            {
                _container = container;

                Key   = container?.ResolveProperty(_keyName);
                Value = container?.ResolveProperty(_valueName);
            }

            return this;
        }

        public float KeyHeight   => Key.GetHeight()   + EditorGUIUtility.standardVerticalSpacing;
        public float ValueHeight => Value.GetHeight() + EditorGUIUtility.standardVerticalSpacing;

        public float MaxHeight   => Mathf.Max(Key.GetHeight(), Value.GetHeight());
        public float TotalHeight => Key.GetHeight() + Value.GetHeight();

        public void DrawVertical(Rect rect, bool keyDisabled = false, bool valueDisabled = false)
        {
            rect.height = 0f;

            rect.AppendHeight(KeyHeight);
            Key.DisabledPropertyField(rect, keyDisabled);

            rect.AppendHeight(ValueHeight);
            Value.DisabledPropertyField(rect, valueDisabled);
        }

        public void DrawHorizontal(Rect rect, float keyWeight = 0.5f, bool keyDisabled = false, bool valueDisabled = false)
        {
            var keyWidth   = rect.width * keyWeight;
            var valueWidth = rect.width - keyWidth;
            rect.width = 0f;

            rect.AppendWidth(keyWidth);
            Key.DisabledPropertyField(rect, keyDisabled);

            rect.AppendWidth(valueWidth);
            Value.DisabledPropertyField(rect, valueDisabled);
        }
    }
}
