#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using static Rekorn.Tools.Utils.ReflectionExtensions;

namespace Rekorn.Tools.Serialization.Editor
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
