#nullable enable

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Urun4m0r1.RekornTools.Unity
{
    public sealed class AutoRotator : MonoBehaviour
    {
        private enum Axis
        {
            X,
            Y,
            Z,
        }

        [SerializeField] private Axis _axis;
        [SerializeField, Range(-100f, 100f)] private float _speed;
        [SerializeField] private bool _isShake;

        private void Update()
        {
            Vector3 rotationAxis = _axis switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                Axis.Z => Vector3.forward,
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (_isShake)
            {
                transform.Rotate(rotationAxis, Random.Range(-_speed, _speed) * Time.deltaTime);
            }
            else
            {
                transform.Rotate(rotationAxis, _speed * Time.deltaTime);
            }
        }
    }
}
