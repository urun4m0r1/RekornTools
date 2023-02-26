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

        [SerializeField] private Axis axis;
        [SerializeField, Range(-100f, 100f)] private float speed;
        [SerializeField] private bool isShake;

        private void Update()
        {
            Vector3 rotationAxis = axis switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                Axis.Z => Vector3.forward,
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (isShake)
            {
                transform.Rotate(rotationAxis, Random.Range(-speed, speed) * Time.deltaTime);
            }
            else
            {
                transform.Rotate(rotationAxis, speed * Time.deltaTime);
            }
        }
    }
}
