#nullable enable

using UnityEngine;
using UnityEngine.Events;

namespace Urun4m0r1.RekornTools.Unity
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleLifetimeManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onParticleEnd;

        private void OnParticleSystemStopped()
        {
            onParticleEnd.Invoke();
        }
    }
}
