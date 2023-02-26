#nullable enable
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent? label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, "The property has to be a tag for LayerAttribute to work!");
                return;
            }

            if (string.IsNullOrEmpty(property.stringValue)) property.stringValue = "Untagged";

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}
#endif
