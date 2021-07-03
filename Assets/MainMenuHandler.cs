using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuHandler : MonoBehaviour{

    [SerializeField] private GameObject playTextSelect;
    [SerializeField] private GameObject settingTextSelect;
    private bool overwrite;


    void Start()
    {

        settingTextSelect.SetActive(false);
    }


    void FixedUpdate()
    {
        Debug.Log(EventSystem.current);
        if ((EventSystem.current.currentSelectedGameObject != null) && EventSystem.current.currentSelectedGameObject.CompareTag("NextLVL") && !overwrite) {
            // Try Again selected
            playTextSelect.SetActive(true);
            settingTextSelect.SetActive(false);
        } else if (EventSystem.current.currentSelectedGameObject.CompareTag("PrevLVL")) {
            playTextSelect.SetActive(false);
            settingTextSelect.SetActive(true);
        }
    }

}
