using com.runtime.GameJamBois.BGJ20201.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameContext : Context<GameContext>
{
    public PlayableDirector IntroDirector;
    public GibbingScript HeadGibber;
    public DifficultyManager DifficultyManager;
    public ManipulateWithMouseController CameraController;
}
