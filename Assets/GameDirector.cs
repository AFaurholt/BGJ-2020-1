using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string MenuScene = "MainMenu";
    [SerializeField] private string GameScene = "MainScene";
    [SerializeField] private string GameOverScene = "GameOver";



    // Start is called before the first frame update
    void Awake()
    {
        SceneUtils.MakeSureSceneIsLoaded(MenuScene);
        SceneUtils.MakeSureSceneIsLoaded(GameScene, active: true); 

        SceneUtils.SceneLoaded += s =>
        {
            if (s == MenuScene)
            {
                BindMainMenu();
                return;
            }
            if (s == GameOverScene)
            {
                BindGameOver();
                return;
            }
        };
    }

    private void Start()
    {
        SceneUtils.UnloadSceneIfExists(GameOverScene);
    }

    // ========================== Actions     ==========================
    private void StartGame()
    {
        Time.timeScale = 1;
        SceneUtils.UnloadSceneIfExists(MenuScene);

        GameContext ctx = GameContext.Current;

        ctx.IntroDirector.Play();
        ctx.HeadGibber.GibbedToDeath += GameOver;
    }

    private void GoToMenu()
    {
        Time.timeScale = 1;
        SceneUtils.MakeSureSceneIsLoaded(MenuScene);
        SceneUtils.UnloadSceneIfExists(GameOverScene);
        SceneUtils.ReloadScene(GameScene, active: true);
    }

    private void GameOver()
    {
        GameContext.Current.CameraController.enabled = false;
        SceneUtils.MakeSureSceneIsLoaded(GameOverScene);
        DOTween.To(() => Time.timeScale, v => Time.timeScale = v, 0.2f, 0.8f).SetUpdate(true);
    }

    private void Restart()
    {
        Time.timeScale = 1;
        StartCoroutine(Restart_C());

        IEnumerator Restart_C()
        {
            SceneUtils.UnloadSceneIfExists(GameOverScene);
            SceneUtils.ReloadScene(GameScene, active: true);

            yield return null;

            StartGame();
        }
    }

    // ========================== Scene binds ==========================
    private void BindMainMenu()
    {
        MainMenuContext.Current.PlayButton.onClick.AddListener(StartGame);
        MainMenuContext.Current.QuitButton.onClick.AddListener(() => Application.Quit());
    }


    private void BindGameOver()
    {
        GameOverContext.Current.RestartButton.onClick.AddListener(Restart);
        GameOverContext.Current.MenuButton.onClick.AddListener(GoToMenu);
    }
}
