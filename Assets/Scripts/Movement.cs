using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    private Rigidbody2D rigidbody2D;
    private float inputHor;
    private float inputVer;
    private float angle;
    public float moveSpeed;
    public float turnSpeed;
   // public bool fromPrevLevel;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (Scenemanager.Instance.loadPrevScene) {
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                transform.position = new Vector2(9, 4);

            }
            Scenemanager.Instance.loadPrevScene = false;
        }
    }


    void Update()
    {
        inputHor = Input.GetAxis("Horizontal");
        inputVer = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        Vector3 inputDir = new Vector3(inputHor,inputVer,0).normalized;

        rigidbody2D.MovePosition(transform.position + inputDir * Time.deltaTime * moveSpeed);
        float targetAngle = Mathf.Atan2(inputDir.x, -inputDir.y) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputDir.magnitude);
        rigidbody2D.MoveRotation(angle);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PrevLVL")) {
            Scenemanager.Instance.LoadPrevLevel();
        } else if (collision.CompareTag("NextLVL")) {
            Scenemanager.Instance.LoadNextLevel();
        }
    }
}