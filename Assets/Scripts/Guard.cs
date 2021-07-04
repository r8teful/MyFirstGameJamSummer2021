using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Guard : MonoBehaviour {
    [SerializeField] RectTransform canvas;
    private Light2D light;
    public Transform pathHolder;
    private Transform player;
    public float moveSpeed;
    private List<Vector3> waypoints;
    private bool moving = false;
    private int nextPoint;
    private float viewAngle;
    public float waitTime;
    public float lookSpeed;
    public float viewDistance;
    public LayerMask viewMask;
    private bool shot;
    private float seenTime;
    
    private void OnDrawGizmos() {
        if (pathHolder!=null) {
            Vector2 startPosition = pathHolder.GetChild(0).position;
            Vector2 previousPosition = startPosition;
            foreach (Transform waypoint in pathHolder) {
                Gizmos.DrawCube(waypoint.position, Vector3.one * .1f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
            Gizmos.DrawRay(transform.position, transform.right * viewDistance); 
        }
    }

    void Start() {
        light = GetComponentInChildren<Light2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waypoints = new List<Vector3>();
        viewAngle = light.pointLightInnerAngle;
        if (pathHolder != null) {
        populateWayPoints();
        transform.position = waypoints[0];
        transform.LookAt(waypoints[0]);
        }
    }

    private void populateWayPoints() {
        for (int i = 0; i < pathHolder.childCount; i++) {
            waypoints.Add(pathHolder.GetChild(i).position);
        }
    }

    private void FixedUpdate() {
        
    }
    void Update() {

        canvas.localEulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);


        //canvas.eulerAngles =  Vector3.forward* Mathf.MoveTowardsAngle(canvas.eulerAngles.z, -transform.eulerAngles.z, Time.deltaTime * 400);
        Debug.Log("MINUS transform.eulerAngles.z " + -transform.eulerAngles.z);
        Debug.Log("transform.eulerAngles.z " + transform.eulerAngles.z);
        // Move the goard to the different waypoints, one after the other
        if (!moving && !shot && (pathHolder != null)) {
            StartCoroutine(WaitForNextMove(waitTime));
        }
        if (shot) {
            light.enabled = false;
        } else if (CanSeePlayer()) {
            
            light.color = Color.red;
            seenTime += Time.deltaTime;
            if (seenTime >= .1f) {
                // Game over make animation 
                Debug.Log("GameOver!!");

                Time.timeScale = 0f;
            }
        } else {
            seenTime = 0;
            light.color = Color.yellow;
        }
    }

    IEnumerator Move(Vector3 dest) {
        while ((transform.position != dest) && !shot) {
            transform.position = Vector2.MoveTowards(transform.position, dest, Time.deltaTime * moveSpeed);
            yield return null;
        }
        // Done moving to waypoint, look towards next one
        nextPoint = (nextPoint + 1) % waypoints.Count;
        yield return StartCoroutine(LookTowards(waypoints[nextPoint]));
        
        moving = false;
    }

    IEnumerator WaitForNextMove(float s) {
        moving = true;
        yield return new WaitForSeconds(s);
        StartCoroutine(Move(waypoints[nextPoint]));
    }

    IEnumerator LookTowards(Vector3 dest) {
        var lookDirection = (dest - transform.position).normalized;
        var lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        while ((Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, lookAngle)) > 0.1f)&& !shot) {
            transform.eulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(transform.eulerAngles.z, lookAngle, Time.deltaTime * lookSpeed);
            yield return null;
        }

    }

    private bool CanSeePlayer() {
       if(Vector2.Distance(transform.position,player.position) < viewDistance) {
            var dirToPlayer = (player.position - transform.position).normalized;
            var angleBetweenGuardAndPlayer = Vector2.Angle(transform.right, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle/2f) {
                if (!Physics2D.Linecast(transform.position,player.position, viewMask)) {
                    return true;
                }
            }
            
        }
        return false;
    }

    public void Shot() {
        Debug.Log("OOF");
        shot = true;
        //StopCoroutine();
       // StopCoroutine("Move");
        //StopCoroutine("WaitForNextMove");
    }
   
}
