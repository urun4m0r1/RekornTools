using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Math
{
    public static class VectorExtensions
    {
        public enum CalculateMode
        {
            Add,
            Subtract,
            Cross,
            Scale,
        }

        public enum NormalAxis
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Back,
        }

        public static Vector3 GetNormal(NormalAxis axis) =>
            axis switch
            {
                NormalAxis.Up      => Vector3.up,
                NormalAxis.Down    => Vector3.down,
                NormalAxis.Left    => Vector3.left,
                NormalAxis.Right   => Vector3.right,
                NormalAxis.Forward => Vector3.forward,
                NormalAxis.Back    => Vector3.back,
                _                  => Vector3.zero,
            };

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        public static Vector3 GetNormal(this Transform vector, NormalAxis axis) =>
            axis switch
            {
                NormalAxis.Up      => vector.up,
                NormalAxis.Down    => -vector.up,
                NormalAxis.Left    => -vector.right,
                NormalAxis.Right   => vector.right,
                NormalAxis.Forward => vector.forward,
                NormalAxis.Back    => -vector.forward,
                _                  => Vector3.zero,
            };

        public static Vector2 ConvertXZ(this Vector3 input) => new Vector2(input.x, input.z);
        public static Vector3 ConvertXZ(this Vector2 input) => new Vector3(input.x, 0f, input.y);

        public static Vector2 ClampMagnitude(this Vector2 v, float min, float max)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * max) return v.normalized * max;
            if (sm < (double)min * min) return v.normalized * min;

            return v;
        }

        public static Vector3 ClampMagnitudePlain(this Vector3 v, float min, float max)
        {
            var y = v.y;
            v   = v.ConvertXZ().ClampMagnitude(min, max).ConvertXZ();
            v.y = y;
            return v;
        }

        public static Vector3 ClampMagnitude(this Vector3 v, float min, float max)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * max) return v.normalized * max;
            if (sm < (double)min * min) return v.normalized * min;

            return v;
        }
    }
}
