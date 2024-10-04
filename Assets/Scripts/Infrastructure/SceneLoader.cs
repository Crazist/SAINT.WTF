using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class SceneLoader
    {
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        private void Inject(CoroutineCustomRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}