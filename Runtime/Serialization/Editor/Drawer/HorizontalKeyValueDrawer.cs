﻿#nullable enable

using UnityEditor;
using UnityEngine;

namespace Rekorn.Tools.Serialization.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalKeyValue<,>), true)]
    public class HorizontalKeyValueDrawer : SerializedKeyValueDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __) =>
            Helper.Update(property).DrawHorizontal(rect, 0.2f, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).MaxHeight;
    }
}
