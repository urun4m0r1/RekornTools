﻿#nullable enable

using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : BasePropertyDrawer
    {
        protected readonly ReorderableListHelper Helper = new ReorderableListHelper(SerializedList.FieldName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent? _, int __) =>
            Helper.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty? property, GUIContent? _) =>
            Helper.Update(property).GetHeight();
    }
}
