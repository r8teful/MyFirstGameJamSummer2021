using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLevelHandler : MonoBehaviour {

    public void RestartLevelAnimation() {
        Scenemanager.Instance.RestartLevel();
    }
}
