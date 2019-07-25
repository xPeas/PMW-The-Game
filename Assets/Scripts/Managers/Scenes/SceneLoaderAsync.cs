using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderAsync : Singleton<SceneLoaderAsync>
{
    public float LoadingProgress { get; private set; }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            LoadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            if (asyncScene.progress >= 0.9f)
            {
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}