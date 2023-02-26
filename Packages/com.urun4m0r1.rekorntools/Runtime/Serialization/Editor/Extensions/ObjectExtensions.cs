#nullable enable

using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class ObjectExtensions
    {
#region Editor
        public static void DrawEditor(this Object obj, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            {
                obj.DrawEditor();
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawEditor<T>(this T obj) where T : Object
        {
            var editor = UnityEditor.Editor.CreateEditor(obj);
            if (editor != null) editor.OnInspectorGUI();
            EditorUtility.SetDirty(obj);
        }
#endregion // Editor
    }
}
