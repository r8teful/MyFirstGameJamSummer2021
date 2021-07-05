using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanager : MonoBehaviour {
    [SerializeField] Animator transitions;
    public static Scenemanager Instance;
    public bool loadPrevScene;
    public bool isSpotted;
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
        transitions.SetTrigger("Start");
    }

    
}
