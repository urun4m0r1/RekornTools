#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [DisallowMultipleComponent]
    public sealed class PhysicsGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField, Range(0f, 1f)]  float    minMovement           = 0.01f;
        [SerializeField, Range(0f, 30f)] float    velocityThickness     = 5f;
        [SerializeField]                 Color    velocityColor         = Color.yellow;
        [SerializeField, Range(0f, 30f)] float    accelerationThickness = 10f;
        [SerializeField]                 Color    accelerationColor     = Color.magenta;

        Vector3 _prevPosition;
        Vector3 _prevVelocity;
        Vector3 _velocity;
        Vector3 _acceleration;
        float   _inversedFixedDeltaTime;
        bool    _wasMoved;

        void Awake()
        {
            _inversedFixedDeltaTime = 1f / Time.fixedDeltaTime;
        }

        void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;

            Vector3 deltaPosition = currentPosition - _prevPosition;
            _velocity = deltaPosition * _inversedFixedDeltaTime;

            Vector3 deltaVelocity = _velocity - _prevVelocity;
            _acceleration = deltaVelocity * _inversedFixedDeltaTime;

            _prevPosition = currentPosition;
            _prevVelocity = _velocity;

            _wasMoved = deltaPosition.sqrMagnitude > minMovement * minMovement;
        }

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected(PhysicsGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected(PhysicsGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos(PhysicsGizmos target)
            {
                Vector3 po = target.transform.position;
                Vector3 pv = po + target._velocity;
                Vector3 pa = po + target._acceleration;

                if (target._wasMoved)
                {
                    Color vColor = target.velocityColor;
                    GizmoUtils.DrawThickLine(po, pv, target.velocityThickness, vColor);
                    DrawArrow.ForGizmoTwoPoints(po, pv, vColor);

                    Color aColor = target.accelerationColor;
                    GizmoUtils.DrawThickLine(po, pa, target.accelerationThickness, aColor);
                    DrawArrow.ForGizmoTwoPoints(po, pa, aColor);
                }
            }
        }
#endif
    }
}
