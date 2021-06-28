using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;

    private void OnDrawGizmos() {
        foreach (Transform waypoint in pathHolder) {
            Gizmos.DrawCube(waypoint.position, Vector3.one*.1f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
