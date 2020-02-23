using System;
using UnityEngine.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad] 
#endif
public static class SceneUtils
{
    public static event Action<string> SceneLoaded;
  
    static SceneUtils()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Debug.Log("sceneutils constructor");
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Debug.Log("init");
    }

    private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode _)
    {
        SceneLoaded?.Invoke(scene.name);
    }

    public static void MakeSureSceneIsLoaded(string name, bool active = false)
    {
        Debug.Log("makeSureSceneIsLoaded: "+name);
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.IsValid())
        {
            if (active && SceneManager.GetActiveScene() != scene)
            {
                SceneManager.sceneLoaded += SetSceneActive;
            }
            return;
        }
        else
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            if (active)
            {
                SceneManager.sceneLoaded += SetSceneActive;
            }
        }

        void SetSceneActive(Scene loadedScene, LoadSceneMode mode)
        {
            if (loadedScene.name != name)
                return;

            SceneManager.SetActiveScene(loadedScene);
            SceneManager.sceneLoaded -= SetSceneActive;
            Debug.Log("SetSceneActive:" + loadedScene);
        }
    }

    public static void ReloadScene(string name, bool active = false)
    {
        Debug.Log("Reload Scene: " + name);
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.IsValid())
        {
            SceneManager.UnloadSceneAsync(scene);

            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            if (active)
            {
                SceneManager.sceneLoaded += SetSceneActive;
            }
        }

        void SetSceneActive(Scene loadedScene, LoadSceneMode mode)
        {
            if (loadedScene.name != name)
                return;

            SceneManager.SetActiveScene(loadedScene);
            SceneManager.sceneLoaded -= SetSceneActive;
        }
    }

    public static void UnloadSceneIfExists(string name)
    {
        Debug.Log("Unload scene: " + name);
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.IsValid())
        {
            SceneManager.UnloadSceneAsync(scene);
            Debug.Log("scene unload valid: " + name);
        }
    }
}