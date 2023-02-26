#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(position, "The property has to be a layer for LayerAttribute to work!");
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}
#endif
