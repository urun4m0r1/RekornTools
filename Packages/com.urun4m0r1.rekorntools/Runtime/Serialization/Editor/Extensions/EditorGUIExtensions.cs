#nullable enable

using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class EditorGUIExtensions
    {
#region Decorator
        public static readonly float SingleItemHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
#endregion // Decorator

#region Extensions
        public static bool DisabledPropertyField(this SerializedProperty? obj, Rect rect, bool isDisabled = true)
        {
            BeginDisabledGroup(isDisabled);
            var result = obj.PropertyField(rect);
            EndDisabledGroup();
            return result;
        }

        public static bool PropertyField(this SerializedProperty? obj, Rect rect, string? title = null)
        {
            if (obj == null)
            {
                LabelField(rect, "ERROR:", "SerializedProperty is null");
                return false;
            }

            return EditorGUI.PropertyField(rect, obj, title != null ? new GUIContent(title) : GUIContent.none, true);
        }

        public static float GetHeight(this SerializedProperty? obj) =>
            obj == null ? SingleItemHeight : GetPropertyHeight(obj, true);
#endregion // Extensions
    }
}
