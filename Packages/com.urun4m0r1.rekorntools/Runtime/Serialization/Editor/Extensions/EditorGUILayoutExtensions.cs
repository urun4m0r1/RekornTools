﻿#nullable enable

using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class EditorGUILayoutExtensions
    {
#region Decorator
        public static void HorizontalLine()
        {
            var skin                = GUI.skin;
            var style               = GUIStyle.none;
            if (skin != null) style = skin.horizontalSlider;

            LabelField("", style);
        }
#endregion // Decorator

#region Extensions
        public static T? ObjectField<T>(string? label, T? obj, bool allowSceneObjects)
            where T : UnityEngine.Object =>
            (T)EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects);

        public static T? EnumDropdown<T>(string? label, T selected)
            where T : Enum
        {
            var e = EnumPopup(label, selected);
            return e != null ? (T)e : selected;
        }
#endregion // Extensions
    }
}
