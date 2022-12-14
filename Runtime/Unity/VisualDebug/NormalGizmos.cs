#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [DisallowMultipleComponent]
    public sealed class NormalGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 bool     autoScaled    = true;
        [SerializeField, Range(0f, 10f)] float    lineLength    = 1f;
        [SerializeField, Range(0f, 30f)] float    lineThickness = 5f;

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected(NormalGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected(NormalGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos(NormalGizmos target)
            {
                Transform t = target.transform;
                Vector3   p = t.position;

                float scaledLength = GetScaledLength(target.lineLength);

                Vector3 px = p + t.right   * scaledLength;
                Vector3 py = p + t.up      * scaledLength;
                Vector3 pz = p + t.forward * scaledLength;

                GizmoUtils.DrawThickLine(p, px, target.lineThickness, Color.red);
                GizmoUtils.DrawThickLine(p, py, target.lineThickness, Color.green);
                GizmoUtils.DrawThickLine(p, pz, target.lineThickness, Color.blue);

                float GetScaledLength(float length)
                {
                    float result                   = length;
                    if (!target.autoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}
