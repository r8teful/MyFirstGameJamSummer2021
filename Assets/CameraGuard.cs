using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraGuard : MonoBehaviour
{

    private Light2D light;
    public Transform pathHolder;
    private Transform player;
    private List<Vector3> waypoints;
    private bool moving = false;
    private int nextPoint;
    private float viewAngle;
    public float waitTime;
    public float lookSpeed;
    public float viewDistance;
    public LayerMask viewMask;

    private void OnDrawGizmos() {
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

    void Start() {
        light = GetComponentInChildren<Light2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waypoints = new List<Vector3>();
        viewAngle = light.pointLightInnerAngle;
        populateWayPoints();
       // transform.LookAt(waypoints[0]);
        StartCoroutine(LookTowards(waypoints[nextPoint]));
    }

    private void populateWayPoints() {
        for (int i = 0; i < pathHolder.childCount; i++) {
            waypoints.Add(pathHolder.GetChild(i).position);
        }
    }

    void Update() {
        if (!moving) {
            StartCoroutine(WaitForNextMove(waitTime));
        }
        if (CanSeePlayer()) {
            light.color = Color.red;
        } else {
            light.color = Color.yellow;
        }

    }

    IEnumerator Move(Vector3 dest) {
        //Look towards waypoint
      
        yield return StartCoroutine(LookTowards(waypoints[nextPoint]));
        nextPoint = (nextPoint + 1) % waypoints.Count;
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

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, lookAngle)) > 0.1f) {
            transform.eulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(transform.eulerAngles.z, lookAngle, Time.deltaTime * lookSpeed);
            yield return null;
        }

    }

    private bool CanSeePlayer() {
        if (Vector2.Distance(transform.position, player.position) < viewDistance) {
            var dirToPlayer = (player.position - transform.position).normalized;
            var angleBetweenGuardAndPlayer = Vector2.Angle(transform.right, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
                if (!Physics2D.Linecast(transform.position, player.position, viewMask)) {
                    return true;
                }
            }

        }
        return false;
    }

}