#nullable enable

using System;
using UnityEngine;
using Object = UnityEngine.Object;


#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace Urun4m0r1.RekornTools.Utils
{
    public static class EditorExtensions
    {
        public static void ShowConfirmDialog<T>(this T script, string message) where T : MonoBehaviour
        {
            var header = $"[{typeof(T)}({script.gameObject.name})]";
            Debug.LogWarning($"{header} {message}");
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(header, message, "Confirm");
#endif // UNITY_EDITOR
        }

        public static void UndoableAction(this Object target, Action action) =>
            UndoableAction(target, target.name, action);

        public static void UndoableAction(this Object target, string actionName, Action action)
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(target, actionName);
            action();
            Undo.FlushUndoRecordObjects();
#endif // UNITY_EDITOR
        }
    }
}
