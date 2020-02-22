using System;
using UnityEngine.SceneManagement;

public class SceneUtils
{
    public static void LoadScene(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        if (scene.IsValid())
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
    }

    public static void UnloadScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
}