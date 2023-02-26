#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using static Urun4m0r1.RekornTools.Utils.ReflectionExtensions;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public static class SerializationExtensions
    {
        [CanBeNull] public static SerializedProperty ResolveProperty(
            [CanBeNull] this SerializedObject obj,
            [NotNull]        string           name) =>
            obj?.FindProperty(name) ?? obj?.FindProperty(ResolveFieldName(name));

        [CanBeNull] public static SerializedProperty ResolveProperty(
            [CanBeNull] this SerializedProperty prop,
            [NotNull]        string             name) =>
            prop?.FindPropertyRelative(name) ?? prop?.FindPropertyRelative(ResolveFieldName(name));
    }
}
