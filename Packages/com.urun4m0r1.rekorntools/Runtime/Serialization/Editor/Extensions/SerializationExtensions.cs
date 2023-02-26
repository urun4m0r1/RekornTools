#nullable enable

using UnityEditor;
using static Urun4m0r1.RekornTools.Utils.ReflectionExtensions;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class SerializationExtensions
    {
        public static SerializedProperty? ResolveProperty(
            this SerializedObject? obj,
            string            name) =>
            obj?.FindProperty(name) ?? obj?.FindProperty(ResolveFieldName(name));

        public static SerializedProperty? ResolveProperty(
            this SerializedProperty? prop,
            string              name) =>
            prop?.FindPropertyRelative(name) ?? prop?.FindPropertyRelative(ResolveFieldName(name));
    }
}
