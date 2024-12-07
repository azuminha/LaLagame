using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private static ScenesManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("ScenesManager instance initialized.");
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void LoadSceneAsync(string sceneName, Action callback = null)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            callback?.Invoke();
            return;
        }

        //Debug.Log(sceneName);
        if(Instance == null)
            Debug.Log("ASAAAAA");
        else
            Instance.StartCoroutine(LoadSceneAsyncAux(sceneName, callback));
    }

    public static bool IsSceneLoaded(string key)
    {
        return SceneManager.GetActiveScene().name == key;
    }

    private static IEnumerator LoadSceneAsyncAux(string scene, Action callback = null)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);

        Debug.Log("Entrou no carregamento");

        while (!load.isDone)
        {
            yield return null;
        }

        Debug.Log("Carregou");


        callback?.Invoke();
    }
}
