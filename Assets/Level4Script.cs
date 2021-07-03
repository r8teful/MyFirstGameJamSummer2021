using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level4Script : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private TextMeshProUGUI triangle;
    [SerializeField] private GameObject bean;
    [SerializeField] private GameObject image;
    private int input;

    private bool isactive;

    void Start()
    {
        popupText.gameObject.SetActive(false);
        triangle.gameObject.SetActive(false);
        image.SetActive(false);
        StartCoroutine(flashingArrow());
    }

    void Update()
    {
        Debug.Log(input);
        if ((bean == null) && (input<3)) {
            popupText.text = "You picked up a lazer! With it you can disable guards";
            image.SetActive(true);
            popupText.gameObject.SetActive(true);
            Time.timeScale = 0f;

            if ((Input.GetMouseButtonDown(0)) || (Input.GetMouseButtonDown(1)) || (Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Return))) {
                input++;

            }

            if (input == 1) {
                popupText.text = "To use it, aim by holding the left mouse button, then release to shoot";
            } else if (input == 2) {
                popupText.text = "Use it wisely! As you can only dissable one guard a level";
            } else if (input==3) {
                StopCoroutine(flashingArrow());
                Time.timeScale = 1f;
                popupText.gameObject.SetActive(false);
                triangle.gameObject.SetActive(false);
                image.SetActive(false);
            }
        }
    }

    IEnumerator flashingArrow() {
        while (true) {
            triangle.gameObject.SetActive(isactive);
            isactive = !isactive;
            yield return new WaitForSecondsRealtime(.5f);
        }
    }
}
