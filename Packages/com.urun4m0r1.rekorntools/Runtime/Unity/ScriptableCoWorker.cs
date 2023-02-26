#nullable enable

using System.Collections;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public class ScriptableCoWorker : MonoBehaviour
    {
        private static ScriptableCoWorker s_instance;

        public static Coroutine? StartWork(IEnumerator? task)
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Can not run coroutine outside of play mode.");
                return null;
            }

            if (!s_instance)
            {
                s_instance = new GameObject("CoroutineWorker").AddComponent<ScriptableCoWorker>();
                DontDestroyOnLoad(s_instance.gameObject);
            }

            return s_instance.StartCoroutine(task);
        }

        public static void StopWork(Coroutine? task)
        {
            if (!s_instance)
            {
                Debug.LogError("Can not stop coroutine outside of play mode.");
                return;
            }

            s_instance.StopCoroutine(task);
        }
    }
}
