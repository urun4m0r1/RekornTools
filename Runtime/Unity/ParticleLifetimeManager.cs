using UnityEngine;
using UnityEngine.Events;

namespace Rekorn.Tools.Unity
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
