#nullable enable

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rekorn.Tools.Serialization.Editor
{
    [CustomEditor(typeof(Object), true)]
    public class DefaultEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (target != null) DrawButton(target);
        }

        static void DrawButton([NotNull] Object obj)
        {
            var methods = obj.GetType()
                             .GetMembers(ReflectionExtensions.Everything)
                             .Where(x => Attribute.IsDefined(x, typeof(ButtonAttribute)));

            foreach (var memberInfo in methods)
            {
                if (GUILayout.Button(memberInfo.Name))
                {
                    (memberInfo as MethodInfo)?.Invoke(obj, null);
                }
            }
        }
    }
}
