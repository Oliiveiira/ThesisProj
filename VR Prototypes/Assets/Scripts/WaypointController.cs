using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointController : MonoBehaviour
{
    public float velocity = 10f;
    public float angle = 45f;
    public float gravity = 9.8f;
    public float maxDistance = 50f;

    public int raylenght = 10;
    public float delay = 0.1f;
    public Vector3 teleportPos;

    public GameObject pointer;
    public ActiveStateSelector laserPose;
    public ActiveStateSelector tpPose;
    public LineRenderer lr;

    public GameObject playerPosition;
    public bool showLaser;
    public bool canTP;
    public bool canWalk;

    public float cooldown;
    float lastTp;
    public float speed;

    public GameObject ovrCameraRig;

    private Vector3[] positions;

    public NavMeshAgent navMeshAgent;

    void Start()
    {
        laserPose.WhenSelected += () => showLaser = true;
        laserPose.WhenUnselected += () => showLaser = false;

        tpPose.WhenSelected += () => canTP = true;
        tpPose.WhenUnselected += () => canTP = false;
    }

    void Update()
    {
        TeleporttoPoint();
    }

    private void TeleporttoPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raylenght * 10))
        {
            //teleportPos = hit.point;

            if (showLaser)
            {
                pointer.SetActive(true);
                lr.enabled = true;
                // set up the line renderer positions
                positions = CalculateParabolicLine(transform.position, hit.point, 0, Mathf.RoundToInt(maxDistance / delay) + 1);
                lr.positionCount = positions.Length;
                lr.SetPositions(positions);
                pointer.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

                if (canTP && Time.time - lastTp > cooldown && hit.collider.gameObject.tag == "TeleportDestination")
                {
                    //playerPosition.transform.position = Vector3.MoveTowards(playerPosition.transform.position, new Vector3(hit.point.x, playerPosition.transform.position.y, hit.point.z), speed * Time.deltaTime);
                    //ovrCameraRig.transform.position = playerPosition.transform.position;
                    teleportPos = new Vector3(hit.point.x, playerPosition.transform.position.y, hit.point.z);
                    lastTp = Time.time;
                    canWalk = true;

                }
            }
            else
            {
                lr.enabled = false;
                pointer.SetActive(false);
            }
            if (canWalk)
            {
                //playerPosition.transform.position = Vector3.MoveTowards(playerPosition.transform.position, teleportPos, speed * Time.deltaTime);
                navMeshAgent.destination = teleportPos;
                //ovrCameraRig.transform.position = playerPosition.transform.position;
               // ovrCameraRig.transform.position = navMeshAgent.transform.position;

                if(playerPosition.transform.position == teleportPos)
                {
                    canWalk = false;
                }
            }

        }
    }

    private Vector3[] CalculateParabolicLine(Vector3 start, Vector3 end, float height, int numPoints)
    {
        Vector3[] positions = new Vector3[numPoints];

        // Calculate the distance between the start and end points
        float distance = Vector3.Distance(start, end);

        // Calculate the time it would take for an object to travel the distance at the given velocity
        float totalTime = distance / velocity;

        // Calculate the x and z components of the velocity vector
        float vx = (end.x - start.x) / totalTime;
        float vz = (end.z - start.z) / totalTime;

        // Calculate the initial y velocity component to reach the given height
        float vy = (height - start.y + end.y) / totalTime + 0.5f * gravity * totalTime;

        // Calculate the time interval between points
        float timeInterval = totalTime / (numPoints - 1);

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeInterval;

            // Calculate the x and z coordinates at time t
            float x = start.x + vx * t;
            float z = start.z + vz * t;

            // Calculate the y coordinate at time t using the parabolic formula
            float y = start.y + vy * t - 0.5f * gravity * t * t;

            // If the calculated y coordinate is less than 0, set it to 0 to ensure the end point is at ground level
            if (y < 0)
            {
                y = 0;
            }

            // Set the position of the point at index i
            positions[i] = new Vector3(x, y, z);
        }

        return positions;
    }

    void GetDirection()
    {

    }

    private void StartMovement(Vector3 direction)
    {
        playerPosition.transform.position = Vector3.MoveTowards(playerPosition.transform.position, new Vector3(direction.x, playerPosition.transform.position.y, direction.z), speed * Time.deltaTime);
        ovrCameraRig.transform.position = playerPosition.transform.position;
    }
}
