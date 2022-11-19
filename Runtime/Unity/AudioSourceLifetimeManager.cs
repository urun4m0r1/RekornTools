using UnityEngine;
using UnityEngine.Events;

namespace Rekorn.Tools.Unity
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioSourceLifetimeManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onAudioEnd;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
            {
                onAudioEnd.Invoke();
            }
        }
    }
}
