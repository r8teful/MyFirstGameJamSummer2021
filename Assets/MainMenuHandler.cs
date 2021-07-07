using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour{

    [SerializeField]private Slider musicSlider;
    [SerializeField] private Slider FXSlider;

    public void OnSliderMusicChanged() {
        Scenemanager.Instance.SetMusicVolume(musicSlider.value);
    }
    public void OnSliderFXChanged() {

        Scenemanager.Instance.PlayHitSound();
        Scenemanager.Instance.SetFXVolume(FXSlider.value);
    }
}
