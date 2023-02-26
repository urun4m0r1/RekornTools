#nullable enable

using UnityEngine;
using UnityEngine.Events;

namespace Urun4m0r1.RekornTools.Unity
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioSourceLifetimeManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAudioEnd;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
            {
                _onAudioEnd.Invoke();
            }
        }
    }
}
