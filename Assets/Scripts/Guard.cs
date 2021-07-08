using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Guard : MonoBehaviour {
    [SerializeField] RectTransform canvas;
    [SerializeField] ParticleSystem deathpParticles;
    [SerializeField] AudioSource aboutToDie;
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
    private bool gameover;
    private Vector2 spottedPos;
    private float timeColided;
    private SpriteRenderer guardSprite;
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
        guardSprite = GetComponent<SpriteRenderer>();
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
        if (Scenemanager.Instance.isSpotted) StopAllCoroutines();
    }
    void Update() {
        //Debug.Log(timeColided);
        // Freeze the players position if they have been spotted
        if (gameover)  player.position = spottedPos;
        if (shot) guardSprite.color = Color.black;
        if (!moving && !shot && (pathHolder != null)) {
            StartCoroutine(WaitForNextMove(waitTime));
        } else if (CanSeePlayer()) {
            if (seenTime < 0.1f) seenTime += Time.deltaTime;
            UpdateColor();
            if (seenTime >= .1f && !gameover) {
                light.color = Color.red;

                spottedPos = player.position;

                // Start animation
               
                canvas.gameObject.SetActive(true);
                canvas.localEulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
                gameover = true;
                Scenemanager.Instance.isSpotted = true;
                Scenemanager.Instance.gameObject.transform.position = spottedPos;
                Scenemanager.Instance.PlayDeathAnimation();
            }
        } else {
            seenTime = 0;
            UpdateColor();
        }
    }

    private void UpdateColor() {
        light.color = new Color(1, 1 - seenTime * 10, 0);
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
        if (!shot) {
            if (Vector2.Distance(transform.position, player.position) < viewDistance) {
                var dirToPlayer = (player.position - transform.position).normalized;
                var angleBetweenGuardAndPlayer = Vector2.Angle(transform.right, dirToPlayer);
                if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
                    if (!Physics2D.Linecast(transform.position, player.position, viewMask)) {
                        return true;
                    }
                }

            } 
        }
        return false;
    }
    
    private void OnCollisionStay2D(Collision2D collision) {
       
        // If you collided with another guard for to long, just turn it off
        if(collision.gameObject.CompareTag("WalkGuard")) {
            
            timeColided += Time.deltaTime;
            guardSprite.color = new Color(1, 1- timeColided, 1- timeColided);
           
            aboutToDie.pitch = timeColided * 10;
            if ((timeColided > 1) && !shot) {
                Shot();
            } else if(!shot){
                aboutToDie.volume = Scenemanager.Instance.GetFXVolume();
                aboutToDie.Play();
            }
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("WalkGuard")) {
            StartCoroutine(SlowDownPitch());
        } 
    }
    
    public void Shot() {
        Scenemanager.Instance.PlayHitSound();
        aboutToDie.Stop();
        shot = true;
        light.enabled = false;
        deathpParticles.Play();
    }
    IEnumerator SlowDownPitch() {
        while (aboutToDie.pitch <= 10 && aboutToDie.pitch > 0 && !shot) {
            aboutToDie.pitch -= (Time.deltaTime*10);
            timeColided -= (Time.deltaTime * 10);
            guardSprite.color = new Color(1, 1 - aboutToDie.pitch/10, 1 - aboutToDie.pitch/10);
            yield return null;
        }
        timeColided = 0;
    }
   
}
