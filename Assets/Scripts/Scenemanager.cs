using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanager : MonoBehaviour {
    [SerializeField] private AudioSource noticed;

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource hit;
    [SerializeField] private Animator transitions;
    public static Scenemanager Instance;
    public bool loadPrevScene;
    public bool isSpotted;
    private float fxVolume;
    public void SetFXVolume(float value) {
        noticed.volume = value;
        hit.volume = value;
        fxVolume = value;
    }

    public float GetFXVolume() {
        return fxVolume;
    }
    public void SetMusicVolume(float value) {
        music.volume = value;
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void RestartLevel() {
        isSpotted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPrevLevel() {
        Debug.Log("Current scene is" + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Current scene is" + SceneManager.GetActiveScene().buildIndex);
        loadPrevScene = true;
    }

    internal void PlayDeathAnimation() {
        noticed.Play();
        transitions.SetTrigger("Start");
    }

    internal void PlayHitSound() {
        if (!hit.isPlaying)
        hit.Play();
        Debug.Log("Playing Hit");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}
