using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameContext : MonoBehaviour
{
    public PlayableDirector IntroDirector;
    public GibbingScript HeadGibber;
    public DifficultyManager DifficultyManager;

    public static GameContext Current = null;

    private void OnEnable()
    {
        if (Current != null)
            Debug.LogWarning($"{name} will override {Current.name}", this);
        Current = this;
    }

    private void OnDisable()
    {
        if (Current == this)
            Current = null;
    }
}
