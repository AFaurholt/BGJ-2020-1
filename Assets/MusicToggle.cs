using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

using DG.Tweening;

public class MusicToggle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI buttonText = null;
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] bool isActive;

    string buttonPattern;

    private void Start()
    {
        buttonPattern = buttonText.text;
        UpdateValue();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isActive = !isActive;
        UpdateValue();
    }

    private void UpdateValue()
    {
        mixer.DOSetFloat("Volume", isActive ? 0 : -80f, 2f);
        buttonText.text = string.Format(buttonPattern, isActive ? "ON" : "OFF");
    }
}
