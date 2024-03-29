﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Serialization
{
    [System.Serializable]
    public class ComponentList<T> : ObjectList<T> where T : Component
    {
        private void ShowDialog(string message)
        {
            var header = $"[{nameof(ObjectList<T>)}<{typeof(T).Name}>]";
            Debug.LogWarning($"{header} {message}");
            EditorUtility.DisplayDialog(header, message, "Confirm");
        }

#region UnityObject
        public void DestroyItems()
        {
            var destroyTarget = new List<T>();
            var destroyFailed = new StringBuilder();

            foreach (var o in this)
            {
                if (o == null)
                {
                    // ReSharper disable once ExpressionIsAlwaysNull
                    destroyTarget.Add(o);
                }
                else if (!IsObjectPrefab(o))
                {
                    Undo.DestroyObjectImmediate(o.gameObject);
                    destroyTarget.Add(o);
                }
                else
                {
                    destroyFailed.Append($"{o.name}, ");
                }
            }

            if (destroyFailed.Length > 0)
            {
                var objectsList = destroyFailed.ToString().TrimEnd(',', ' ');
                ShowDialog($"Failed to destroy following objects: {objectsList}\n" +
                           "You might need to unpack prefabs before destroy them.");
            }

            RemoveRange(destroyTarget);
        }

        private static bool IsObjectPrefab(Object o) =>
            o && PrefabUtility.GetPrefabInstanceStatus(o) == PrefabInstanceStatus.Connected;

        public void Initialize(Transform? parent, string? keyword = null)
        {
            var objects = parent == null
                ? GameObjectExtensions.GetAllGameObjectsInScene?.SelectMany(x => x == null ? null : x.GetComponents<T>())
                : parent.GetComponentsInChildren<T>(true);

            if (keyword != null && !string.IsNullOrWhiteSpace(keyword))
                objects = objects?.Where(x => x != null && x.name.Contains(keyword));

            Initialize(objects?.Where(x => x));
        }
#endregion // UnityObject

#region Selection
        public override bool TryGetSelections(out Object[] selections)
        {
            selections = this.Select(x => x == null ? null : x.gameObject as Object).ToArray();
            return selections.Length != 0;
        }
#endregion // Selection
    }
}
