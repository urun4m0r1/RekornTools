#nullable enable

using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>))]
    public class SerializedKeyValueDrawer : BasePropertyDrawer
    {
        protected static readonly SerializedKeyValueHelper Helper = new SerializedKeyValueHelper();

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent? _, int __) =>
            Helper.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty? property, GUIContent? _) =>
            Helper.Update(property).TotalHeight;
    }
}
