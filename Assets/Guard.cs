using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;
    public float moveSpeed;
    private List<Vector3> waypoints;
    private bool moving = false;
    private int nextPoint;
    public float waitTime;
    public float lookSpeed;
    private void OnDrawGizmos() {
        Vector2 startPosition = pathHolder.GetChild(0).position;
        Vector2 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder) {
            Gizmos.DrawCube(waypoint.position, Vector3.one * .1f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
    // Start is called before the first frame update
    void Start() {
        waypoints = new List<Vector3>();
        populateWayPoints();
    }

    private void populateWayPoints() {
        for (int i = 0; i < pathHolder.childCount; i++) {
            waypoints.Add(pathHolder.GetChild(i).position);
        }
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) StopAllCoroutines();
        // Move the goard to the different waypoints, one after the other
        if (!moving) {
            StartCoroutine(WaitForNextMove(waitTime));
        }

    }

    IEnumerator Move(Vector3 dest) {
        while (transform.position != dest) {
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
        Debug.Log(lookAngle);

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, lookAngle)) > 0.1f) {
            transform.eulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(transform.eulerAngles.z, lookAngle, Time.deltaTime * lookSpeed);
            yield return null;
        }

    }
}
