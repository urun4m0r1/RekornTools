#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,,>), true)]
    public class SerializedDictionaryDrawer : BasePropertyDrawer
    {
        [NotNull] protected readonly ReorderableListHelper Helper = new ReorderableListHelper(SerializedDictionary.FieldName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __) =>
            Helper.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).GetHeight();
    }
}
