using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanager : MonoBehaviour {
    [SerializeField] private AudioSource noticed;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource hit;
    [SerializeField] private Animator transitions;
    public static Scenemanager Instance;
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
    private void Start() {
        fxVolume = 1;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            if (pauseMenu.activeInHierarchy) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    internal void PlayDeathAnimation() {
        noticed.Play();
        transitions.SetTrigger("Start");
    }

    internal void PlayHitSound() {
        if (!hit.isPlaying)
        hit.Play();
    }

    public void LoadMainMenu() {
        if(pauseMenu.activeInHierarchy) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(0);
    }
}
