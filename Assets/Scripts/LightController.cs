using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour {

    [SerializeField] private GameObject player;
    private LineRenderer line;
    private RaycastHit2D hitPoint;
    [SerializeField] private Light2D lazerLight;
   
    // Start is called before the first frame update
    void Start()
    {

        lazerLight.enabled = false;
        line = gameObject.GetComponent<LineRenderer>();
        line.startWidth = .1f;
        line.endWidth = .02f;
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var lookDirection = (mousePosition - player.transform.position).normalized;
        var lookAngle = -90 + Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = lookAngle * Vector3.forward;
        if (Input.GetMouseButton(0)) {
            // Show where the player is aiming att 
            hitPoint = Physics2D.Raycast(transform.position, lookDirection);
            if (hitPoint) {
                // Enable and calculate line
                lazerLight.pointLightOuterRadius = (new Vector3(hitPoint.point.x, hitPoint.point.y) - player.transform.position).magnitude;
                Debug.Log(lazerLight.pointLightOuterRadius);
                line.enabled = true;
                lazerLight.enabled = true;
                line.SetPosition(0, player.transform.position);
                line.SetPosition(1, hitPoint.point);
                
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            // Check if the ray hit a guard, else just do nothing
            if (hitPoint.collider.gameObject.CompareTag("WalkGuard")) {
                var hitGuard = hitPoint.collider.gameObject.GetComponent<Guard>();
                hitGuard.Shot();
            } else if (hitPoint.collider.gameObject.CompareTag("CameraGuard")) {
                var hitGuard = hitPoint.collider.gameObject.GetComponent<CameraGuard>();
                hitGuard.Shot();
            }// else do nothing 

            // Remove the LineRenderer
            line.enabled = false;
            lazerLight.enabled = false;
        }   
    }
}
