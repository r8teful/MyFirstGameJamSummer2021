using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float inputHor;
    private float inputVer;
    private float angle;
    public float moveSpeed;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputHor = Input.GetAxis("Horizontal");
        inputVer = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        //Vector3 inputDir = ((inputHor * Vector3.right) + (inputVer * Vector3.up)).normalized;
        Vector3 inputDir = new Vector3(inputHor,inputVer,0).normalized;
        Debug.Log(inputDir);
        transform.position += inputDir * Time.deltaTime * moveSpeed; ;
        float targetAngle = Mathf.Atan2(inputDir.x, -inputDir.y) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputDir.magnitude);
        transform.eulerAngles = Vector3.forward*angle;
    }
}
