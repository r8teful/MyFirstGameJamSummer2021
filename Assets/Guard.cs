using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;
    public float speed;
    private List<Vector3> waypoints;
    private bool moving = false;
    private int nextPoint;
    public float waitTime;

    private void OnDrawGizmos() {
        Vector2 startPosition = pathHolder.GetChild(0).position;
        Vector2 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder) {
            Gizmos.DrawCube(waypoint.position, Vector3.one*.1f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
    }
    // Start is called before the first frame update
    void Start() {
        waypoints = new List<Vector3>();
        populateWayPoints();
    }

    private void populateWayPoints() {
        for (int i =0;i<pathHolder.childCount;i++) {
            waypoints.Add(pathHolder.GetChild(i).position);
        }
    }
    // Update is called once per frame
    void Update() {
        // Move the goard to the different waypoints, one after the other
        if (!moving) {
            StartCoroutine(WaitForNextMove(waitTime));
        }

    }

    IEnumerator Move(Transform cur, Vector3 dest) {
        Debug.Log(cur + "YA" + dest);
        while (cur.position != dest) {
           
            cur.position = Vector2.MoveTowards(cur.position, dest, Time.deltaTime * speed);
            yield return null;
        }
        nextPoint++;
        moving = false;
    }

    IEnumerator WaitForNextMove(float s) {
        moving = true;
        yield return new WaitForSeconds(s);
        StartCoroutine(Move(transform, waypoints[nextPoint]));
    }
}
