#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools.Utils
{
    public static class GameObjectExtensions
    {
        public static void BackupGameObject(this Object obj)
        {
            var backup = Object.Instantiate(obj);
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
#endif
        }

#if UNITY_EDITOR
        public static bool IsPrefab(this GameObject? go, out GameObject root)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(go) != PrefabInstanceStatus.Connected)
            {
                root = null;
                return false;
            }

            root = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
            return true;
        }

        public static void UnpackPrefab(this GameObject? go, PrefabUnpackMode unpackMode)
        {
            if (!go.IsPrefab(out var target)) return;
            PrefabUtility.UnpackPrefabInstance(target, unpackMode, InteractionMode.UserAction);
        }
#endif

        public static IEnumerable<GameObject>? AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        public static IEnumerable<GameObject>? GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        private static bool IsEditableSceneObject(GameObject? go)
        {
            if (go == null) return false;

            var root               = go.transform.root;
            if (root == null) root = go.transform;

#if UNITY_EDITOR
            var isStoredOnDisk = EditorUtility.IsPersistent(root);
#else
            var isStoredOnDisk = false;
#endif
            return !isStoredOnDisk && !(root.hideFlags == HideFlags.NotEditable || root.hideFlags == HideFlags.HideAndDontSave);
        }
    }
}
