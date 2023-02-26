#nullable enable

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public sealed class PhysicsGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField]          public  DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField, Range(0f, 1f)]  private float    _minMovement           = 0.01f;
        [SerializeField, Range(0f, 30f)] private float    _velocityThickness     = 5f;
        [SerializeField]                 private Color    _velocityColor         = Color.yellow;
        [SerializeField, Range(0f, 30f)] private float    _accelerationThickness = 10f;
        [SerializeField]                 private Color    _accelerationColor     = Color.magenta;

        private Vector3 _prevPosition;
        private Vector3 _prevVelocity;
        private Vector3 _velocity;
        private Vector3 _acceleration;
        private float   _inversedFixedDeltaTime;
        private bool    _wasMoved;

        private void Awake()
        {
            _inversedFixedDeltaTime = 1f / Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            Vector3 currentPosition = transform.position;

            Vector3 deltaPosition = currentPosition - _prevPosition;
            _velocity = deltaPosition * _inversedFixedDeltaTime;

            Vector3 deltaVelocity = _velocity - _prevVelocity;
            _acceleration = deltaVelocity * _inversedFixedDeltaTime;

            _prevPosition = currentPosition;
            _prevVelocity = _velocity;

            _wasMoved = deltaPosition.sqrMagnitude > _minMovement * _minMovement;
        }

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)] private static void OnSelected(PhysicsGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)] private static void OnNonSelected(PhysicsGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            private static void DrawGizmos(PhysicsGizmos target)
            {
                Vector3 po = target.transform.position;
                Vector3 pv = po + target._velocity;
                Vector3 pa = po + target._acceleration;

                if (target._wasMoved)
                {
                    Color vColor = target._velocityColor;
                    GizmoUtils.DrawThickLine(po, pv, target._velocityThickness, vColor);
                    DrawArrow.ForGizmoTwoPoints(po, pv, vColor);

                    Color aColor = target._accelerationColor;
                    GizmoUtils.DrawThickLine(po, pa, target._accelerationThickness, aColor);
                    DrawArrow.ForGizmoTwoPoints(po, pa, aColor);
                }
            }
        }
#endif
    }
}
