using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string MenuScene = "MainMenu";
    [SerializeField] private string GameScene = "MainScene";

    // Start is called before the first frame update
    void Awake()
    {
        SceneUtils.LoadScene(MenuScene);
        SceneUtils.LoadScene(GameScene);
    }

    private void Start()
    {
        MainMenuContext.Current.PlayButton.onClick.AddListener(StartGame);
        MainMenuContext.Current.QuitButton.onClick.AddListener(() => Application.Quit());
    }

    private void StartGame()
    {
        SceneUtils.UnloadScene(MenuScene);
        GameContext.Current.IntroDirector.Play();
    }
}
