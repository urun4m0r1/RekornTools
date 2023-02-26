using System.Collections;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    public class ScriptableCoWorker : MonoBehaviour
    {
        private static ScriptableCoWorker _instance;

        public static Coroutine StartWork(IEnumerator task)
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Can not run coroutine outside of play mode.");
                return null;
            }

            if (!_instance)
            {
                _instance = new GameObject("CoroutineWorker").AddComponent<ScriptableCoWorker>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance.StartCoroutine(task);
        }

        public static void StopWork(Coroutine task)
        {
            if (!_instance)
            {
                Debug.LogError("Can not stop coroutine outside of play mode.");
                return;
            }

            _instance.StopCoroutine(task);
        }
    }
}
