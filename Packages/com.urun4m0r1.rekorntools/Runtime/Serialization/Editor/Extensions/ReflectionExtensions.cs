#nullable enable

using System;
using System.Reflection;
using UnityEditor;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class ReflectionExtensions
    {
#region Attribute
        public static readonly BindingFlags Everything = ~BindingFlags.Default;

        public static T? GetAttribute<T>(this SerializedProperty? property, bool inherit = true) where T : Attribute
        {
            var attributes = property.GetAttributes<T>(inherit);
            return attributes?.Length > 0 ? attributes[0] : null;
        }

        public static T[]? GetAttributes<T>(this SerializedProperty? property, bool inherit = true) where T : Attribute
        {
            var member = property?.GetFieldOrProperty();
            return member?.GetCustomAttributes(typeof(T), inherit) as T[];
        }

        static MemberInfo? GetFieldOrProperty(this SerializedProperty property)
        {
            var target = property.serializedObject?.targetObject;
            var paths  = property.propertyPath?.Split('.');
            if (target == null || paths == null) return null;

            var        type   = target.GetType();
            MemberInfo member = null;
            foreach (var name in paths)
            {
                (member, type) = type.GetFieldOrProperty(name);
            }

            return member;
        }
#endregion // Attribute
    }
}
