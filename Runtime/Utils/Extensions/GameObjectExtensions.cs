﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rekorn.Tools.Utils
{
    public static class GameObjectExtensions
    {
        public static void BackupGameObject([NotNull] this Object obj)
        {
            var backup = Object.Instantiate(obj);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
        }

        public static bool IsPrefab([CanBeNull] this GameObject go, out GameObject root)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(go) != PrefabInstanceStatus.Connected)
            {
                root = null;
                return false;
            }

            root = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
            return true;
        }

        public static void UnpackPrefab([CanBeNull] this GameObject go, PrefabUnpackMode unpackMode)
        {
            if (!go.IsPrefab(out var target)) return;
            PrefabUtility.UnpackPrefabInstance(target, unpackMode, InteractionMode.UserAction);
        }

        [CanBeNull]
        public static IEnumerable<GameObject> AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        [CanBeNull]
        public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        static bool IsEditableSceneObject([CanBeNull] GameObject go)
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
