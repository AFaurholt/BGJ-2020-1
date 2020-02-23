using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private const string HighscoreKey = "{9C76EC89-592E-4AB6-B5B7-D4315EA9278D}";

    [Header("Scenes")]
    [SerializeField] private string MenuScene = "MainMenu";
    [SerializeField] private string GameScene = "MainScene";
    [SerializeField] private string GameOverScene = "GameOver";

    private float currentDistance = 0;

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

        OriginShifter.Shifted += OnShifted;
    }

    private void OnShifted(Vector3 v)
    {
        currentDistance += v.y;
    }

    private void Start()
    {
        SceneUtils.UnloadSceneIfExists(GameOverScene);
    }

    // ========================== Actions     ==========================
    private void StartGame()
    {
        currentDistance = 0;
        Time.timeScale = 1;
        SceneUtils.UnloadSceneIfExists(MenuScene);

        GameContext ctx = GameContext.Current;

        ctx.IntroDirector.Play();
        ctx.HeadGibber.GibbedToDeath += GameOver;
        ctx.DifficultyManager.BeginDifficulty();
    }

    private void GoToMenu()
    {
        Time.timeScale = 1;
        SceneUtils.MakeSureSceneIsLoaded(MenuScene);
        SceneUtils.UnloadSceneIfExists(GameOverScene);
        SceneUtils.UnloadSceneIfExists(GameScene);
        SceneUtils.ReloadScene(GameScene, active: true);
    }

    private void GameOver()
    {
        StartCoroutine(GameOver_C());

        IEnumerator GameOver_C()
        {
            int distance = Mathf.FloorToInt(currentDistance);
            int highscore = PlayerPrefs.HasKey(HighscoreKey)
                ? PlayerPrefs.GetInt(HighscoreKey)
                : 0;

            if(highscore < distance)
            {
                highscore = distance;
                PlayerPrefs.SetInt(HighscoreKey, distance);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameContext.Current.CameraController.enabled = false;
            DOTween.To(() => Time.timeScale, v => Time.timeScale = v, 0.2f, 0.8f).SetUpdate(true);

            SceneUtils.MakeSureSceneIsLoaded(GameOverScene);
            yield return null;

            GameOverContext.Current.GameOverText.text = string.Format(GameOverContext.Current.GameOverText.text, ToDisplay(distance), ToDisplay(highscore));
        }
    }

    private void Restart()
    {
        Time.timeScale = 1;
        StartCoroutine(Restart_C());

        IEnumerator Restart_C()
        {
            SceneUtils.UnloadSceneIfExists(GameOverScene);
            SceneUtils.UnloadSceneIfExists(GameScene);
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
        //GameOverContext.Current.MenuButton.onClick.AddListener(GoToMenu);
        GameOverContext.Current.MenuButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("START SCENE"));
    }

    // ========================== Utils       ==========================
    public string ToDisplay(int value)
    {
        Regex regex = new Regex(@"(?!^)(?=(?:\d{3})+(?:\.|$))");

        string s = value.ToString();
        return regex.Replace(s, " ");
    }
}
