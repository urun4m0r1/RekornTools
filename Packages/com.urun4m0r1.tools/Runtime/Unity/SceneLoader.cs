#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rekorn.Tools.Unity
{
    public sealed class SceneLoader : MonoBehaviour
    {
        [SerializeField] public ScriptableAction onStageChanged;

        private int _currentStageIndex;

        public void StartChangeScene(int sceneIndex)
        {
            StartCoroutine(ChangeScene(sceneIndex));
        }

        private IEnumerator ChangeScene(int sceneIndex)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) yield return null;

            if (asyncLoad.isDone)
            {
                Debug.Log($"Scene {sceneIndex} loaded!");
                StartCoroutine(UnloadScene(_currentStageIndex));
                _currentStageIndex = sceneIndex;
                onStageChanged.Invoke();
            }
        }

        private IEnumerator UnloadScene(int sceneIndex)
        {
            if (sceneIndex == 0) yield break;

            var asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex);
            while (!asyncUnload.isDone) yield return null;

            if (asyncUnload.isDone)
            {
                Debug.Log($"Scene {sceneIndex} unloaded!");
            }
        }
    }
}
