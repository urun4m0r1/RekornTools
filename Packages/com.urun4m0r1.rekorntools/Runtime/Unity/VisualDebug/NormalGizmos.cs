#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public sealed class NormalGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField]          public  DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 private bool     _autoScaled    = true;
        [SerializeField, Range(0f, 10f)] private float    _lineLength    = 1f;
        [SerializeField, Range(0f, 30f)] private float    _lineThickness = 5f;

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)] private static void OnSelected(NormalGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)] private static void OnNonSelected(NormalGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            private static void DrawGizmos(NormalGizmos target)
            {
                Transform t = target.transform;
                Vector3   p = t.position;

                float scaledLength = GetScaledLength(target._lineLength);

                Vector3 px = p + t.right   * scaledLength;
                Vector3 py = p + t.up      * scaledLength;
                Vector3 pz = p + t.forward * scaledLength;

                GizmoUtils.DrawThickLine(p, px, target._lineThickness, Color.red);
                GizmoUtils.DrawThickLine(p, py, target._lineThickness, Color.green);
                GizmoUtils.DrawThickLine(p, pz, target._lineThickness, Color.blue);

                float GetScaledLength(float length)
                {
                    float result                   = length;
                    if (!target._autoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}
