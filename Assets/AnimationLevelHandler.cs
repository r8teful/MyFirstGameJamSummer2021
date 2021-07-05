using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLevelHandler : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void RestartLevelAnimation() {
        Scenemanager.Instance.RestartLevel();
    }
/*
    public void RestartLevelAnimation() {
        Scenemanager.Instance.LoadNextLevel();
    }
    public void RestartLevelAnimation() {
        Scenemanager.Instance.LoadPrevLevel();
    }
*/
}
