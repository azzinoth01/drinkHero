using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{

    private static AsyncOperation asyncOperation;
    
    private class SceneLoaderDummy : MonoBehaviour
    {
        // Dummy class used to start loading coroutine    
    }

    private static Action onLoaderCallback; 
    public static void Load(GameSceneEnum gameScene)
    {
        // set the loader callback action to to load target scene
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading GameObject");
            loadingGameObject.AddComponent<SceneLoaderDummy>().StartCoroutine(LoadSceneAsync(gameScene));

        };
        
        // load loading scene
        SceneManager.LoadScene(GameSceneEnum.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        // triggered after first update 
        // execute the loader callback action which will then load target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    private static IEnumerator LoadSceneAsync(GameSceneEnum gameScene)
    {
        yield return null;
        
        //TODO: dont use string based lookup
        asyncOperation = SceneManager.LoadSceneAsync(gameScene.ToString());

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }

    public static float GetLoadingProgress()
    {
        if (asyncOperation != null)
        {
            return asyncOperation.progress;
        }

        return 1f;
    }
}
