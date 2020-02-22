using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuContext : MonoBehaviour
{
    public Button PlayButton;
    public Button QuitButton;
    public Button MusicButton;
    public TMPro.TextMeshProUGUI MusicButtonText;

    public static MainMenuContext Current = null;

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
