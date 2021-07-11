using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject ammocount;
    private float inputHor;
    private float inputVer;
    private float angle;
    public float moveSpeed;
    public float turnSpeed;
    public bool hasLazer;

    void Start() {
        if (!hasLazer) ammocount.SetActive(false);
         rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
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
        if (collision.CompareTag("NextLVL")) {
            Scenemanager.Instance.LoadNextLevel();
        } else if (collision.CompareTag("Lazer")) {
            Destroy(collision.gameObject);
            hasLazer = true;
            ammocount.SetActive(true);
        }
    }


    public void AmmoUsed() {
        ammocount.SetActive(false);
    }
}
