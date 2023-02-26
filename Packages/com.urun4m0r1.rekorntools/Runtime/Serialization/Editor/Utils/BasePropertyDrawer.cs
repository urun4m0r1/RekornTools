#nullable enable

using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty? property, GUIContent? label)
        {
            if (property == null) return;

            var indent = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(rect, label, property);
            {
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUI.indentLevel = 0;
                    rect.ApplyIndent(indent);
                    DrawProperty(rect, property, label, indent);
                }
                if (EditorGUI.EndChangeCheck()) property.serializedObject?.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }

        protected abstract void DrawProperty(Rect rect, SerializedProperty property, GUIContent? label, int indent);

        public override float GetPropertyHeight(SerializedProperty? property, GUIContent? _) => property.GetHeight();
    }
}
