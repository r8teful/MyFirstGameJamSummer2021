using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

 
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider FXSlider;
    // Start is called before the first frame update 
    
    public void MainMenuClicked() {
        Scenemanager.Instance.LoadMainMenu();
    }
    public void OnSliderMusicChanged() {
        Scenemanager.Instance.SetMusicVolume(musicSlider.value);
    }
    public void OnSliderFXChanged() {

        Scenemanager.Instance.PlayHitSound();
        Scenemanager.Instance.SetFXVolume(FXSlider.value);
    }
    public void OnResumeClicked() {
        Time.timeScale = 1; 

    }
}
